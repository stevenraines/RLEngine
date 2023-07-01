using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using RLEngine.Server.Infrastructure;
using Microsoft.AspNetCore.Hosting;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<GameContext>(opt =>
    opt.UseSqlite($"Data Source={nameof(GameContext)}.db")
    .EnableSensitiveDataLogging(false)
    .EnableDetailedErrors(false));

// Add services to the container.
builder.WebHost.UseIISIntegration()
                .UseKestrel();

builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<RLEngine.Server.GameServer>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GameContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
