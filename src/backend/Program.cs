using AS_2025.Repository;
using FastEndpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddFastEndpoints();

builder.Services.AddSingleton<ITraitRepository, TraitRepository>();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();
app.UseFastEndpoints();

app.Run();
