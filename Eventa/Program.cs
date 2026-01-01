using Eventa.Web.Extensions;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// إعدادات اللغة
var supportedCultures = new[] { "en", "ar" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[1]) // العربية افتراضية
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

builder.Services.AddControllersWithViews()
    .AddViewLocalization();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var cultureInfo = new CultureInfo("ar-EG");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Response Caching
builder.Services.AddResponseCaching();
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromMinutes(5)));
    options.AddPolicy("StaticContent", builder => builder.Expire(TimeSpan.FromHours(1)));
});

builder.Services.RegisterDbContext(builder.Configuration);

// Identity
builder.Services.SetIdentityConfigs(builder.Configuration);
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/cp/Login";
    options.AccessDeniedPath = "/cp/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

// RegisterCustomServises
builder.Services.RegisterCustomServises();
builder.Services.RegisterSettings(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Static files with caching headers
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
    }
});

app.UseRequestLocalization(localizationOptions);

app.UseRouting();

// Response Caching Middleware
app.UseResponseCaching();
app.UseOutputCache();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
