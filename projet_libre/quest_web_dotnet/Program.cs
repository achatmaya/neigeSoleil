using System.Security.Claims;
using quest_web.Data;
using quest_web.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using quest_web.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuration du contexte
var connectionString = "server=127.0.0.1;database=quest_web;user=root;password=";
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Désactiver HTTPS
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(8090);
});

// Ajouter les services au conteneur.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Ajouter l'authentification JWT
var jwtSecret = builder.Configuration["Jwt:Secret"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddSingleton(new JwtTokenUtil(jwtSecret, jwtIssuer, jwtAudience));

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserRolePolicy", policy => policy.RequireRole("ROLE_USER", "ROLE_OWNER", "ROLE_ADMIN"));
});

// Ajouter AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Configuration des CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Remplacez cette URL par celle de votre application Angular
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configurer le pipeline HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Configurer les fichiers statiques
app.UseStaticFiles(); // Ajoutez cette ligne pour servir les fichiers statiques

app.UseCors("AllowAngularApp"); // Ajouter la configuration CORS
app.UseAuthentication(); // Ajouter l'authentification
app.UseAuthorization();

app.MapControllers();

app.Run();
