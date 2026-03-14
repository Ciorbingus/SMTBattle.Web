using SMTBattle.Web.Components;
using SMTBattle.Web.Data;
using SMTBattle.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc; 
using System.Security.Claims;
using SMTBattle.Web.Services;
using Microsoft.AspNetCore.Components.Server.Circuits;

var builder = WebApplication.CreateBuilder(args);

// UI Services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Database 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Identity and Authentication
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddIdentityCore<User>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();


// Application Services
builder.Services.AddScoped<IProfileService, ProfileService>();

builder.Services.AddSingleton<UserPresenceService>();
builder.Services.AddScoped<CircuitHandler, PresenceCircuitHandler>();

var app = builder.Build();

// Pipeline configuration
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Logout endpoint
app.MapPost("Account/Logout", async (
    SignInManager<User> signInManager,
    [FromForm] string returnUrl) =>
{
    await signInManager.SignOutAsync();
    return Results.LocalRedirect(returnUrl ?? "/");
});

// Delete Account endpoint
app.MapPost("Account/Delete", async (
    SignInManager<User> signInManager, 
    UserManager<User> userManager,
    IProfileService profileService, 
    ClaimsPrincipal userPrincipal) =>
{
    var user = await userManager.GetUserAsync(userPrincipal);
    if (user != null)
    {
    
        var profile = await profileService.GetProfileByIdAsync(user.Id);
        
        await userManager.DeleteAsync(user);
        await signInManager.SignOutAsync();
    }
    return Results.LocalRedirect("/");
});

app.Run();