using System.Text;
using Amsberg_digital_universe.Domain.Common;
using Amsberg_digital_universe.Domain.Common.Extensions;
using Amsberg_digital_universe.Features.Auth;
using Amsberg_digital_universe.Features.Auth.ConfirmRegistration;
using Amsberg_digital_universe.Features.Auth.Login;
using Amsberg_digital_universe.Features.Auth.Register;
using Amsberg_digital_universe.Features.Auth.SendNewEmailCode;
using Amsberg_digital_universe.Features.Posts.CreatePost;
using Amsberg_digital_universe.Infrastructure.Auth;
using Amsberg_digital_universe.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;


var builder = WebApplication.CreateBuilder(args);
AuthOptions jwtOptions = builder.Configuration.GetSection("Jwt").Get<AuthOptions>();

builder.Services.Configure<HasherOptions>(builder.Configuration.GetSection("Hasher"));
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Jwt"));
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
    };
});

builder.Services.AddMediatR(options => { options.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly); });
builder.Services.AddAuthorization();
builder.Services.AddSingleton<ITokenProvider, JwtTokenProvider>();
builder.Services.AddOpenApi();
var connectionString = builder.Configuration
    .GetConnectionString("DbConnection");

if (string.IsNullOrEmpty(connectionString))
    throw new Exception("Connection string is missing");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));
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
builder.Services.AddSeeders();


var app = builder.Build();
await app.Services.SeedDb();

app.MapAuth();
CreatePostEndpoint.AddRoute(app);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();