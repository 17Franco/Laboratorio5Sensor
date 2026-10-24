using Laboratorio5Sensor.Client.Pages;
using Laboratorio5Sensor.Components;
using Radzen;
using Laboratorio5Sensor.Hubs;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddRadzenComponents();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Laboratorio5Sensor.Client._Imports).Assembly);

app.MapHub<SensorHub>("/sensorTemperatura");

app.MapGet("/cambiarTemperatura", async (IHubContext<SensorHub> hubContext) =>
{
    int temperatura = Random.Shared.Next(100);

    await hubContext.Clients.All.SendAsync(
        "cambioDeTemperatura", temperatura);

    return Results.Ok($"Nueva temperatura: {temperatura}");
});

app.Run();