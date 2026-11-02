using System.Text;
using Amsberg_digital_universe.Domain.Common;
using Amsberg_digital_universe.Domain.Common.Extensions;
using Amsberg_digital_universe.Features.Auth.Login;
using Amsberg_digital_universe.Features.Auth.Register;
using Amsberg_digital_universe.Infrastructure;
using Amsberg_digital_universe.Infrastructure.Auth;
using Amsberg_digital_universe.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
AuthOptions jwtOptions = builder.Configuration.GetSection("Jwt").Get<AuthOptions>();

builder.Services.Configure<HasherOptions>(builder.Configuration.GetSection("Hasher"));
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

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly);
    options.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly);
});
builder.Services.AddAuthorization();
builder.Services.AddSingleton<ITokenProvider, JwtTokenProvider>();
builder.Services.AddOpenApi();
var connectionString = builder.Configuration
    .GetConnectionString("DbConnection");

if (string.IsNullOrEmpty(connectionString))
    throw new Exception("Connection string is missing");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();


RegisterEndpoint.AddRoutes(app);
LoginEndpoint.AddRoutes(app);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();