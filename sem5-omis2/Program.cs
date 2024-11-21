using sem5_omis2.Models;
using sem5_omis2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;


QuestPDF.Settings.License = LicenseType.Community;
var builder = WebApplication.CreateBuilder(args);
 
// получаем строку подключения из файла конфигурации
var connection = builder.Configuration.GetConnectionString("DefaultConnection")!;

// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

// Настройка Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = true; // Настройка, если необходимо подтверждение 
}).AddEntityFrameworkStores<ApplicationContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Путь, куда будет перенаправлен неавторизованный пользователь
});

builder.Services.AddTransient<IEventReportGenerator, EventReportGenerator>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

// Middleware для аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "login",
    pattern: "{controller=Account}/{action?}");

app.MapControllerRoute(
    name: "event",
    pattern: "{controller=Event}/{action}/{id?}");

app.MapControllerRoute(
    name: "eventGroup",
    pattern: "{controller=EventGroup}/{action=Index}/{id?}");

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.Run();
