using System.Security.Claims;
using System.Text;
using Amsberg_digital_universe.Domain;
using Amsberg_digital_universe.Domain.Common;
using Amsberg_digital_universe.Domain.Common.Storage;
using Amsberg_digital_universe.Domain.Extensions;
using Amsberg_digital_universe.Features.Auth;
using Amsberg_digital_universe.Features.Auth.ConfirmRegistration;
using Amsberg_digital_universe.Features.Auth.Login;
using Amsberg_digital_universe.Features.Auth.Register;
using Amsberg_digital_universe.Features.Auth.SendNewEmailCode;
using Amsberg_digital_universe.Features.Posts;
using Amsberg_digital_universe.Features.Posts.CreatePost;
using Amsberg_digital_universe.Features.Profile;
using Amsberg_digital_universe.Infrastructure.Auth;
using Amsberg_digital_universe.Infrastructure.Persistence;
using Amsberg_digital_universe.Infrastructure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;


var builder = WebApplication.CreateBuilder(args);
AuthOptions jwtOptions = builder.Configuration.GetSection("Jwt").Get<AuthOptions>();

builder.Services.Configure<HasherOptions>(builder.Configuration.GetSection("Hasher"));
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<BlobStorageOptions>(builder.Configuration.GetSection("BlobStorage"));
builder.Services.Configure<ImageValidationOptions>(nameof(ImageValidationProperty.Avatar),builder.Configuration.GetSection("ImageValidation:Avatar"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateActor = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
        ValidAudience = jwtOptions.Audience,
        ValidIssuer = jwtOptions.Issuer,
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddMediatR(options => { options.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly); });
builder.Services.AddAuthorization();
builder.Services.AddSingleton<ITokenProvider, JwtTokenProvider>();
builder.Services.AddSingleton<IImageValidator, ImageValidator>();

// cors
builder.Services.AddCors(o => { o.AddDefaultPolicy(p => { p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }); });


builder.Services.AddOpenApi();
var connectionString = builder.Configuration
    .GetConnectionString("DbConnection");

if (string.IsNullOrEmpty(connectionString))
    throw new Exception("Connection string is missing");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));


var blobOptions = builder.Configuration.GetSection("BlobStorage").Get<BlobStorageOptions>();


//AZURE CONFIGS
builder.Services.AddSingleton(x => { return new BlobServiceClient(blobOptions.ConnectionString); });
if (!EF.IsDesignTime)
{
    var serviceClient = new BlobServiceClient(blobOptions.ConnectionString);
    var properties = await serviceClient.GetPropertiesAsync();
    properties.Value.Cors.Clear();
    var corsRule = new BlobCorsRule
    {
        AllowedOrigins = "http://localhost:5173",
        AllowedMethods = "GET, PUT, POST, DELETE, OPTIONS",
        AllowedHeaders = "*",
        ExposedHeaders = "*",
        MaxAgeInSeconds = 3600
    };

    properties.Value.Cors.Add(corsRule);

    await serviceClient.SetPropertiesAsync(properties.Value);
}


builder.Services.AddSingleton<IBlobStorage, AzureBlobStorage>();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token"
    });

    options.AddSecurityRequirement(doc =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", doc)] = []
        });
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHostedService<CleanupConfirmTokens>();
builder.Services.AddHostedService<CleanupOrphanFiles>();
builder.Services.AddSeeders();


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
}

await app.Services.SeedDb();

app.UseCors();
app.MapAuth();
app.MapPosts();
app.MapProfiles();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseStaticFiles();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();