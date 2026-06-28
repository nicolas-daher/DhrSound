using System.Text;
using DhrSound.Api.Data;
using DhrSound.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<DhrSoundContext>(opcoes =>
    opcoes.UseSqlServer(
        builder.Configuration.GetConnectionString("DhrSound"),
        sql => sql.EnableRetryOnFailure()));

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<AssinaturaService>();
builder.Services.AddScoped<NotificacaoService>();
builder.Services.AddScoped<TransacaoService>();
builder.Services.AddScoped<BuscaService>();
builder.Services.AddScoped<FavoritoService>();

var chaveJwt = builder.Configuration["Jwt:Chave"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opcoes =>
    {
        opcoes.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Emissor"],
            ValidAudience = builder.Configuration["Jwt:Audiencia"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveJwt))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddCors(opcoes =>
    opcoes.AddDefaultPolicy(p => p
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()));

var app = builder.Build();

using (var escopo = app.Services.CreateScope())
{
    var contexto = escopo.ServiceProvider.GetRequiredService<DhrSoundContext>();
    contexto.Database.EnsureCreated();
    SeedDados.Popular(contexto);
}

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
