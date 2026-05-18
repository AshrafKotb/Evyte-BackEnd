using Eventa.Infrastructure.Seeds;
using Eventa.Web.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    options.AddBasePolicy(builder => builder
        .Expire(TimeSpan.FromMinutes(5))
        .SetVaryByHeader("Cookie")
        .SetVaryByQuery("culture"));
    options.AddPolicy("StaticContent", builder => builder.Expire(TimeSpan.FromHours(1)));
});

builder.Services.RegisterDbContext(builder.Configuration);

// ======== Data Protection - مهم جداً ========
// بيخلي مفاتيح التشفير تتحفظ على الـ filesystem
// فلما السيرفر يعمل restart بعد كل رفع، الـ cookies القديمة تفضل شغالة
var dataProtectionKeysPath = Path.Combine(builder.Environment.ContentRootPath, "DataProtectionKeys");
Directory.CreateDirectory(dataProtectionKeysPath);
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeysPath))
    .SetApplicationName("Eventa")
    .SetDefaultKeyLifetime(TimeSpan.FromDays(365));

// Identity
builder.Services.SetIdentityConfigs(builder.Configuration);

// إعدادات الـ cookie - سيشن طويلة جداً (سنة) مع sliding expiration
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/cp/Login";
    options.AccessDeniedPath = "/cp/AccessDenied";

    // السيشن سنة كاملة - متفصلش الا لو سجل خروج بنفسه
    options.ExpireTimeSpan = TimeSpan.FromDays(365);
    options.SlidingExpiration = true; // يتجدد مع كل استخدام

    // الـ cookie يفضل في المتصفح حتى بعد قفله
    options.Cookie.MaxAge = TimeSpan.FromDays(365);
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "Eventa.Auth";
});

// SecurityStampValidator - يتحقق كل 30 يوم بدل 30 دقيقة
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromDays(30);
});

// RegisterCustomServises
builder.Services.RegisterCustomServises();
builder.Services.RegisterSettings(builder.Configuration);

var app = builder.Build();

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    try
    {
        await DesignSeeder.SeedAsync(scope.ServiceProvider);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

if (!app.Environment.IsDevelopment())
{
    // Return JSON for AJAX/API requests instead of redirecting to HTML error page
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var acceptHeader = context.Request.Headers["Accept"].ToString();
            if (acceptHeader.Contains("application/json") ||
                context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(new { success = false, message = "حدث خطأ في الخادم. أعد المحاولة." }));
            }
            else
            {
                context.Response.Redirect("/Home/Error");
            }
        });
    });
    app.UseHsts();
}

app.UseHttpsRedirection();

// Static files with smart caching headers
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        var path = ctx.File.Name;
        // Long cache for images, fonts, and vendor assets
        if (path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".jpeg") ||
            path.EndsWith(".gif") || path.EndsWith(".svg") || path.EndsWith(".ico") ||
            path.EndsWith(".woff") || path.EndsWith(".woff2") || path.EndsWith(".ttf") ||
            path.EndsWith(".eot"))
        {
            ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
        }
        else
        {
            // Short cache for JS, CSS - revalidate frequently
            ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=3600,must-revalidate");
        }
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
