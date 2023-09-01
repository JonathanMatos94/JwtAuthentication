using JwtStore;
using JwtStore.Models;
using JwtStore.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<JwtToken>();

builder.Services.AddAuthentication(x => 
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.PrivateKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
});
builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("Acesso Premium", p => p.RequireRole("Premium"));
});

var app = builder.Build();

app.MapGet(pattern:"/",handler:(JwtToken jwtToken) => jwtToken.Create(new User(0, "Joesley", "joesleybatista@jbl.com", 
    "url.com", "kakis", new[] {"Student", "Premium"} )));

app.MapGet("/restrito",(ClaimsPrincipal user) => $"Usuário {user.Identity.Name} logado com sucesso.!").RequireAuthorization();
app.MapGet("/premium",(ClaimsPrincipal user) => $"Usuário {user.Identity.Name} Conta Premium logada com sucesso.!").RequireAuthorization("Acesso Premium");

app.UseAuthentication();
app.UseAuthorization();

app.Run();
