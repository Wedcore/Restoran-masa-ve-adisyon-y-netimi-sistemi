using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restoran_masa_ve_adisyon_yönetimi_sistemi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Controllers
{
    public class RezervasyonController : Controller
    {
        private readonly RestoranContext _context;

        public RezervasyonController(RestoranContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var masalar = await _context.Masalar.ToListAsync();
            return View(masalar);
        }

        [HttpGet]
        public async Task<IActionResult> Yap(int id)
        {
            var masa = await _context.Masalar.FindAsync(id);
            if (masa == null || masa.DoluMu) return RedirectToAction("Index");

            // Rezervasyon formuna masa bilgisini gönderiyoruz
            ViewBag.MasaNo = masa.MasaNo;
            ViewBag.MasaId = masa.Id;

            // ⏳ HZ. İSA ENGELLEYİCİ: Sayfa açılırken tarihi 0001 yerine "Şu An" olarak ayarlıyoruz!
            var yeniRezervasyon = new Rezervasyon
            {
                MasaId = masa.Id,
                Tarih = DateTime.Now
            };

            return View(yeniRezervasyon);
        }

        [HttpPost]
        public async Task<IActionResult> Yap(Rezervasyon r)
        {
            // Formdan "Kaydet"e basıldığında veri boş gelirse sistem patlamasın.
            if (r == null)
            {
                return RedirectToAction("Index");
            }

            // URL'den gelen ID yanlışlıkla Rezervasyon ID'si sanılmasın diye sıfırlıyoruz.
            r.Id = 0;

            // Arka planda işlemi yapan hesabı gizlice mühürlüyoruz!
            r.HesapAdi = User.Identity?.Name;

            // Eğer formdan tarih gelmezse o anki tarihi atıyoruz
            if (r.Tarih == default)
            {
                r.Tarih = DateTime.Now;
            }

            // 🛡️ ANTI-SPAM KALKANI (AYNI GÜN TEK REZERVASYON)
            // Veritabanına bakıp, bu kullanıcının o gün için başka bir kaydı var mı kontrol ediyoruz.
            bool ayniGunVarMi = await _context.Rezervasyonlar.AnyAsync(x =>
                x.HesapAdi == r.HesapAdi &&
                x.Tarih.Date == r.Tarih.Date);

            if (ayniGunVarMi)
            {
                // Eğer daha önce o gün için rezervasyon yapmışsa, kaydetmeden hata mesajı ile geri yolluyoruz!
                TempData["Mesaj"] = "Hata: Aynı gün için sadece 1 adet rezervasyon yapabilirsiniz!";
                TempData["Icon"] = "error";
                return RedirectToAction("Index", "Home");
            }

            // 🛠️ DÜZELTME: Artık metin değil, veritabanındaki 1 numaralı ID'yi (Beklemede) atıyoruz!
            r.DurumId = 1;

            // Veritabanına ekleme ve kaydetme
            _context.Rezervasyonlar.Add(r);
            await _context.SaveChangesAsync();

            // İşlem başarılı olduktan sonra mesajı gönderiyoruz
            TempData["Mesaj"] = "Masanız başarıyla ayırtıldı! Onay bekleniyor.";
            TempData["Icon"] = "success";

            // İşlem bitince ana sayfaya veya menüye geri yolluyoruz
            return RedirectToAction("Index", "Home");
        }
    }
}