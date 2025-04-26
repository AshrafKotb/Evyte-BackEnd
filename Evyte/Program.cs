using Evyte.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



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
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.Run();
