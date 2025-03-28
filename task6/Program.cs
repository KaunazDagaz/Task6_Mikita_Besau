using Microsoft.EntityFrameworkCore;
using task6.Components;
using task6.Exceptions;
using task6.Hubs;
using task6.Models;
using task6.Services;
using task6.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPresentationService, PresentationService>();
builder.Services.AddScoped<ISlideService, SlideService>();
builder.Services.AddScoped<ISessionStorageService, SessionStorageService>();
builder.Services.AddSingleton<IActiveUserService, ActiveUserService>();

builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 10 * 1024 * 1024;
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseWebSockets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHub<PresentationHub>("/presentationHub");

app.Run();
