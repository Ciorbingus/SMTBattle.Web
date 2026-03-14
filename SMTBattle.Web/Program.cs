using SMTBattle.Web.Components;
using SMTBattle.Web.Data;
using SMTBattle.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc; 
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();


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

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


//Logout endpoint
app.MapPost("Account/Logout", async (
    SignInManager<User> signInManager,
    [FromForm] string returnUrl) =>
{
    await signInManager.SignOutAsync();
    return Results.LocalRedirect(returnUrl ?? "/");
});

//Delete account endpoint
app.MapPost("Account/Delete", async (
    SignInManager<User> signInManager, 
    UserManager<User> userManager,
    ApplicationDbContext dbContext,
    ClaimsPrincipal userPrincipal) =>
{
    var user = await userManager.GetUserAsync(userPrincipal);
    if (user != null)
    {
        var profile = await dbContext.Set<UserProfile>().FindAsync(user.Id);
        if (profile != null) dbContext.Remove(profile);
        await dbContext.SaveChangesAsync();

        await userManager.DeleteAsync(user);
        
        await signInManager.SignOutAsync();
    }
    return Results.LocalRedirect("/");
});

app.Run();
