using AspNetCoreRateLimit;
using GLMS.Data;
using GLMS.Manager.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<IdentityUser<int>, IdentityRole<int>>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultUI()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
// needed to load configuration from appsettings.json
builder.Services.AddOptions();

// needed to store rate limit counters and ip rules
builder.Services.AddMemoryCache();

//load general configuration from appsettings.json
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));

//load ip rules from appsettings.json
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));

// inject counter and rules stores
builder.Services.AddInMemoryRateLimiting();
//services.AddDistributedRateLimiting<AsyncKeyLockProcessingStrategy>();
//services.AddDistributedRateLimiting<RedisProcessingStrategy>();
//services.AddRedisRateLimiting();


// configuration (resolvers, counter key builders)
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();


var app = builder.Build();
app.UseIpRateLimiting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();