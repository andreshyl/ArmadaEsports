using ArmadaEsports.Core.Interfaces;
using ArmadaEsports.Data.Context;
using ArmadaEsports.Data.Services;
using ArmadaEsports.Web.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddDbContext<ArmadaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ArmadaDb")));

// Auth
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<TtfAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<TtfAuthStateProvider>());
builder.Services.AddAuthentication();
builder.Services.AddAuthorizationCore();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<ICompetitionService, CompetitionService>();
builder.Services.AddScoped<IAttributeService, AttributeService>();
builder.Services.AddScoped<IAiParserService, AiParserService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<Armada.Web.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
