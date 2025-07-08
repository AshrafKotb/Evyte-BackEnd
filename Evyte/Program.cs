using Evyte.Web.Extensions;
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
app.UseStaticFiles();
app.UseRequestLocalization(localizationOptions);

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapControllers();

app.Run();
