using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Restoran_masa_ve_adisyon_yönetimi_sistemi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Controllers
{
    [Authorize(Roles = "User")]
    public class MusteriController : Controller
    {
        private readonly RestoranContext _context;

        public MusteriController(RestoranContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Rezervasyonlarim()
        {
            var rezervasyonlar = await _context.Rezervasyonlar
                .Include(r => r.Durum) // 🛠️ DÜZELTME: Veritabanındaki diğer tabloyu (Durumlar) da listeye bağlıyoruz!
                .Where(r => r.HesapAdi == User.Identity.Name)
                .OrderByDescending(r => r.Tarih) // En yeni en üstte görünsün
                .ToListAsync();

            return View(rezervasyonlar);
        }

        // ❌ REZERVASYON İPTAL ET (SİL)
        [HttpPost]
        public async Task<IActionResult> RezervasyonSil(int id)
        {
            var rez = await _context.Rezervasyonlar.FindAsync(id);

            // 🛠️ DÜZELTME: Artık metin değil, DurumId == 1 (Beklemede) kontrolü yapıyoruz
            if (rez != null && rez.HesapAdi == User.Identity.Name && rez.DurumId == 1)
            {
                _context.Rezervasyonlar.Remove(rez);
                await _context.SaveChangesAsync();
                TempData["Mesaj"] = "Rezervasyonunuz başarıyla iptal edildi.";
                TempData["Icon"] = "success";
            }
            else
            {
                TempData["Mesaj"] = "Bu işlem gerçekleştirilemez. (Zaten onaylanmış veya silinmiş olabilir)";
                TempData["Icon"] = "error";
            }

            return RedirectToAction("Rezervasyonlarim");
        }

        // 📅 TARİH GÜNCELLEME SAYFASINI AÇ (GET)
        [HttpGet]
        public async Task<IActionResult> Guncelle(int id)
        {
            var rez = await _context.Rezervasyonlar.FindAsync(id);

            // 🛠️ DÜZELTME: Artık metin değil, DurumId != 1 (Beklemede değilse) kontrolü yapıyoruz
            if (rez == null || rez.HesapAdi != User.Identity.Name || rez.DurumId != 1)
            {
                return RedirectToAction("Rezervasyonlarim");
            }

            return View(rez);
        }

        // 💾 YENİ TARİHİ KAYDET (POST)
        [HttpPost]
        public async Task<IActionResult> Guncelle(int id, DateTime yeniTarih)
        {
            var rez = await _context.Rezervasyonlar.FindAsync(id);

            // 🛠️ DÜZELTME: Artık metin değil, DurumId == 1 (Beklemede) kontrolü yapıyoruz
            if (rez != null && rez.HesapAdi == User.Identity.Name && rez.DurumId == 1)
            {
                // Geçmiş tarihe güncelleme yapmasın diye kontrol
                if (yeniTarih < DateTime.Now)
                {
                    TempData["Mesaj"] = "Geçmiş bir tarihe rezervasyon yapılamaz!";
                    TempData["Icon"] = "error";
                    return RedirectToAction("Rezervasyonlarim");
                }

                rez.Tarih = yeniTarih;
                await _context.SaveChangesAsync();
                TempData["Mesaj"] = "Rezervasyon tarihiniz başarıyla güncellendi.";
                TempData["Icon"] = "success";
            }

            return RedirectToAction("Rezervasyonlarim");
        }

        [AllowAnonymous]
        public IActionResult menu()
        {
            return View();
        }
    }
}