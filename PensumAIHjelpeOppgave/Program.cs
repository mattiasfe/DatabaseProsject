using PensumAIHjelpeOppgave.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<PensumAIHjelpeOppgave>();

var app = builder.Build();

app.MapControllers();
app.Run();using PensumAIHjelpeOppgave.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<PensumAIHjelpeOppgave>();

var app = builder.Build();

app.MapControllers();
app.Run();
