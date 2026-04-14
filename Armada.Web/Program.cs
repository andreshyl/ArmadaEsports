using ArmadaEsports.Core.Interfaces;
using ArmadaEsports.Data.Context;
using ArmadaEsports.Data.Services;
using ArmadaEsports.Web.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

// SignalR: raise message size limit for image/CSV uploads and extend disconnect timeout
builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 50 * 1024 * 1024; // 50 MB
    options.ClientTimeoutInterval     = TimeSpan.FromSeconds(60);
    options.HandshakeTimeout          = TimeSpan.FromSeconds(30);
});

builder.Services.AddServerSideBlazor(options =>
{
    options.DetailedErrors              = builder.Environment.IsDevelopment();
    options.DisconnectedCircuitMaxRetained = 100;
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
});

builder.Services.AddDbContextPool<ArmadaDbContext>(options =>
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

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/var/www/ttfesports-keys"))
    .SetApplicationName("ttfesports");

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
