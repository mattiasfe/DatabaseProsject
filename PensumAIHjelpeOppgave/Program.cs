using System.Reflection;
using PensumAIHjelpeOppgave.Data;
using PensumAIHjelpeOppgave.Services;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Registrer database og tjeneste
builder.Services.AddSingleton<Database>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

// Sørg for at databasen har riktig skjema før vi tar imot kall
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Database>();
    db.EnsureCreated();
    db.SeedIfEmpty(targetCount: 9);
}

app.MapControllers();

app.Run();