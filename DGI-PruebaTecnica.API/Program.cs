using DGI_PruebaTecnica.Aplicacion.Extensions;
using DGI_PruebaTecnica.Infraestructura.Extensions;

var builder = WebApplication.CreateBuilder(args);

// ---------- Servicios ----------
builder.Services.AddControllers();

var Configuration = builder.Configuration;
builder.Services.AddInjectionInfrastructure(Configuration);
builder.Services.AddInjectionApplication(Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS: permitir cualquier origen (incluye credenciales)
const string CorsPolicy = "CorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsPolicy, policy =>
    {
        policy
            .SetIsOriginAllowed(_ => true) // permite cualquier origen (no usar AllowAnyOrigin con credenciales)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();           // si tu front usa cookies o auth header
    });
});

var app = builder.Build();

// ---------- Pipeline ----------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Opcional: HSTS para prod
    app.UseHsts();
}

app.UseHttpsRedirection();

// Habilitar CORS antes de Auth/Authorization
app.UseCors(CorsPolicy);

// Si más adelante agregas autenticación, esta va aquí:
// app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
