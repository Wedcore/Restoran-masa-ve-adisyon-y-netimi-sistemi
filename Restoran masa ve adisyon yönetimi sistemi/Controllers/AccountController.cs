using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Restoran_masa_ve_adisyon_yönetimi_sistemi.Models;

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Controllers
{
    public class AccountController : Controller
    {
        private readonly RestoranContext _context;

        public AccountController(RestoranContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. MÜŞTERİ GİRİŞ KAPISI (Sadece User)
        // ==========================================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Sadece Rolü "User" olanları arıyoruz
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(u => u.KullaniciAdi == username && u.Sifre == password && u.Rol == "User");

            if (kullanici != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, kullanici.KullaniciAdi),
                    new Claim(ClaimTypes.Role, kullanici.Rol)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Musteri"); // Müşteri paneline yolla
            }

            ViewBag.Error = "Kullanıcı adı veya şifre hatalı knk!";
            return View();
        }

        // ==========================================
        // 2. GİZLİ ADMİN GİRİŞ KAPISI (Sadece Admin)
        // ==========================================
        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(string username, string password)
        {
            // Sadece Rolü "Admin" olanları arıyoruz
            var admin = await _context.Kullanicilar
                .FirstOrDefaultAsync(u => u.KullaniciAdi == username && u.Sifre == password && u.Rol == "Admin");

            if (admin != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.KullaniciAdi),
                    new Claim(ClaimTypes.Role, admin.Rol)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home"); // Admin paneline yolla
            }

            ViewBag.Error = "Yönetici bilgileri hatalı veya yetkisiz giriş denemesi!";
            return View();
        }

        // ==========================================
        // 3. MÜŞTERİ KAYIT İŞLEMLERİ
        // ==========================================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Kullanici yeniMusteri)
        {
            // Kullanıcı adı daha önce alınmış mı kontrolü
            bool kullaniciVarMi = await _context.Kullanicilar
                .AnyAsync(u => u.KullaniciAdi == yeniMusteri.KullaniciAdi);

            if (kullaniciVarMi)
            {
                ViewBag.Error = "Bu kullanıcı adı zaten alınmış knk, başka bir şey dene.";
                return View();
            }

            // Kayıt olan kişiye otomatik "User" rolünü veriyoruz
            yeniMusteri.Rol = "User";

            _context.Kullanicilar.Add(yeniMusteri);
            await _context.SaveChangesAsync();

            // Kayıt başarılıysa Müşteri giriş sayfasına yönlendir
            return RedirectToAction("Login");
        }

        // ==========================================
        // 4. ÇIKIŞ YAPMA
        // ==========================================
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}