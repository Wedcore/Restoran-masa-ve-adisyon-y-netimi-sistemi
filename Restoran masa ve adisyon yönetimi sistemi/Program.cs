using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Restoran_masa_ve_adisyon_yönetimi_sistemi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVİSLERİ KAYDET (Dependency Injection Container)

// MVC yapısını kullanacağımızı belirtiyoruz
builder.Services.AddControllersWithViews();

// SQL Server Veritabanı Bağlantısı
builder.Services.AddDbContext<RestoranContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// KİMLİK DOĞRULAMA (Cookie Authentication) AYARLARI
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";     // Giriş yapmayan kullanıcıyı buraya at
        options.LogoutPath = "/Account/Logout";   // Çıkış yapınca kullanılacak yol
        options.AccessDeniedPath = "/Account/Login"; // Yetkisi yetmeyen (User olup Admin'e giren) buraya gider
        options.Cookie.Name = "RestoranSistemi.Auth"; // Tarayıcıdaki çerezin adı
    });

var app = builder.Build();

// 2. HTTP İSTEK KANALI (Middleware Pipeline)

// Hata ayıklama modunu kontrol et
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // CSS, JS ve Resim dosyaları için şart

app.UseRouting();

// *KRİTİK SIRALAMA*: Önce kim olduğunu bilmeliyiz (Authentication), 
// sonra neye yetkin olduğunu kontrol etmeliyiz (Authorization).
app.UseAuthentication();
app.UseAuthorization();

// VARSAYILAN ROTA (Uygulama açılınca nereye gitsin?)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"); // İlk açılışta Login ekranı gelsin

app.Run();