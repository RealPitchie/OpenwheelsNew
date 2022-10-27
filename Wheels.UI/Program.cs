using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Wheels.Application.Services;
using Wheels.Persistence;
using Wheels.UI.Areas.Identity;
using Wheels.UI.Data;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.
const string connectionString = "Server=localhost;Port=5432;Database=f1;User Id=root;Password=passmein123;";
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
services.AddDatabaseDeveloperPageExceptionFilter();
services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
services.AddRazorPages();
services.AddServerSideBlazor();
services
    .AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
services.AddSingleton<WeatherForecastService>();
services.AddMudServices();
services.AddDbContext<PostsContext>(options => 
    options.UseNpgsql(connectionString));
services.AddScoped<PostService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();