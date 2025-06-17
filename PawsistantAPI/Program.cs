using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System;
using PawsistantAPI.Repository.config;
using System.Text;
using Library.Shared.Model;
using Microsoft.AspNetCore.Identity;
using PawsistantAPI.Services.Interfaces;
using PawsistantAPI.Services;
using PawsistantAPI.Adapters.Interfaces;
using PawsistantAPI.Adapters;
using PawsistantAPI.Helpers;
using DotNetEnv;


var builder = WebApplication.CreateBuilder(args);

//load secrets from .env file
AppSecrets.LoadSecrets(builder);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
builder.Services.AddScoped<IPawsistantService, PawsistantService>();
builder.Services.AddScoped<IAiChatProviderAdapter, OpenRouterChatProviderAdapter>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7222") // <-- din Blazor WebAssembly klient-url
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.

builder.Services.AddControllers();

//Extracting secrets from configuration to use in JWT setup
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

// JWT Authentication setup
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
            ClockSkew = TimeSpan.Zero,  // Optional: remove default clock skew for token expiration
            ValidIssuer = jwtIssuer, // E.g., "https://localhost:7213"
            ValidAudience = jwtAudience // E.g., "https://localhost:5000"
        };
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazorClient"); // Denne SKAL komme fÃ¸r UseAuthorization()


// Use authentication and authorization for the JWT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
