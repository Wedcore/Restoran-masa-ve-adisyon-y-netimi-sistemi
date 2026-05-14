using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Restoran_masa_ve_adisyon_yönetimi_sistemi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly RestoranContext _context;

        public AdminController(RestoranContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. BÖLÜM: PATRON ANA PANELİ VE REZERVASYONLAR
        // ==========================================

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Rezervasyonlar()
        {
            // 🛠️ DÜZELTME: Durum tablosunu da listeye dahil ediyoruz
            var rezervasyonlar = await _context.Rezervasyonlar
                .Include(r => r.Durum)
                .ToListAsync();
            return View(rezervasyonlar);
        }

        public async Task<IActionResult> Onayla(int id)
        {
            var rez = await _context.Rezervasyonlar.FindAsync(id);
            if (rez != null)
            {
                // 🛠️ AMELİYAT BURADA: Metin yerine ID (2 = Onaylandı)
                rez.DurumId = 2;

                var masa = await _context.Masalar.FindAsync(rez.MasaId);
                if (masa != null) masa.DoluMu = true;

                await _context.SaveChangesAsync();
                TempData["Mesaj"] = "Masa başarıyla kapatıldı ve onaylandı.";
                TempData["Icon"] = "success";
            }
            return RedirectToAction("Rezervasyonlar");
        }

        public async Task<IActionResult> Reddet(int id)
        {
            var rez = await _context.Rezervasyonlar.FindAsync(id);
            if (rez != null)
            {
                // 🛠️ AMELİYAT BURADA: Metin yerine ID (3 = Reddedildi)
                rez.DurumId = 3;

                await _context.SaveChangesAsync();
                TempData["Mesaj"] = "Rezervasyon talebi reddedildi.";
                TempData["Icon"] = "error";
            }
            return RedirectToAction("Rezervasyonlar");
        }

        // 🛠️ YENİ EKLENEN İPTAL METODU
        public async Task<IActionResult> Iptal(int id)
        {
            var rez = await _context.Rezervasyonlar.FindAsync(id);
            if (rez != null)
            {
                // Durumu 3 (Reddedildi/İptal) yapıyoruz
                rez.DurumId = 3;

                // 🎯 KRİTİK NOKTA: Önceden onaylandığı için dolu olan masayı boşa çıkarıyoruz!
                var masa = await _context.Masalar.FindAsync(rez.MasaId);
                if (masa != null) masa.DoluMu = false;

                await _context.SaveChangesAsync();
                TempData["Mesaj"] = "Rezervasyon iptal edildi ve masa tekrar boşa çıkarıldı.";
                TempData["Icon"] = "warning";
            }
            return RedirectToAction("Rezervasyonlar");
        }

        // ==========================================
        // 2. BÖLÜM: ADİSYON VE CİRO SİSTEMİ
        // ==========================================

        public async Task<IActionResult> MasaYonetimi()
        {
            var masalar = await _context.Masalar.ToListAsync();
            return View(masalar);
        }

        public async Task<IActionResult> Adisyon()
        {
            var aktifAdisyonlar = await _context.Adisyonlar
                .Where(a => !a.OdendiMi)
                .ToListAsync();
            return View(aktifAdisyonlar);
        }

        public async Task<IActionResult> MasaDetay(int id)
        {
            var masa = await _context.Masalar.FindAsync(id);
            if (masa == null) return RedirectToAction("MasaYonetimi");

            var siparisler = await _context.Adisyonlar
                .Where(a => a.MasaId == id && !a.OdendiMi)
                .ToListAsync();

            ViewBag.MasaNo = masa.MasaNo;
            ViewBag.MasaId = masa.Id;
            return View(siparisler);
        }

        [HttpPost]
        public async Task<IActionResult> SiparisEkle(int masaId, string urunAdi, int adet, decimal fiyat)
        {
            var yeniSiparis = new Adisyon
            {
                MasaId = masaId,
                UrunAdi = urunAdi,
                Adet = adet,
                Fiyat = fiyat,
                Tarih = DateTime.Now,
                OdendiMi = false
            };
            _context.Adisyonlar.Add(yeniSiparis);
            var masa = await _context.Masalar.FindAsync(masaId);
            if (masa != null) masa.DoluMu = true;
            await _context.SaveChangesAsync();
            TempData["Mesaj"] = "Sipariş eklendi!";
            TempData["Icon"] = "success";
            return RedirectToAction("MasaDetay", new { id = masaId });
        }

        [HttpPost]
        public async Task<IActionResult> HesapKapat(int masaId, string odemeYontemi)
        {
            var siparisler = await _context.Adisyonlar.Where(a => a.MasaId == masaId && !a.OdendiMi).ToListAsync();
            decimal toplam = siparisler.Sum(a => a.Adet * a.Fiyat);
            if (toplam > 0)
            {
                var satis = new Satis { MasaId = masaId, ToplamTutar = toplam, OdemeYontemi = odemeYontemi, SatisTarihi = DateTime.Now };
                _context.Satislar.Add(satis);
                foreach (var s in siparisler) s.OdendiMi = true;
                var masa = await _context.Masalar.FindAsync(masaId);
                if (masa != null) masa.DoluMu = false;
                await _context.SaveChangesAsync();
                TempData["Mesaj"] = $"Hesap Kapatıldı! Tutar: {toplam} ₺";
                TempData["Icon"] = "success";
            }
            return RedirectToAction("MasaYonetimi");
        }

        public async Task<IActionResult> Ciro()
        {
            var satislar = await _context.Satislar.OrderByDescending(s => s.SatisTarihi).ToListAsync();
            return View(satislar);
        }

        // ==========================================
        // 3. BÖLÜM: MASA AYARLARI (EKLE/SİL)
        // ==========================================

        [HttpPost]
        public async Task<IActionResult> MasaEkle(string masaNo)
        {
            if (!string.IsNullOrEmpty(masaNo))
            {
                var yeniMasa = new Masa { MasaNo = masaNo, DoluMu = false };
                _context.Masalar.Add(yeniMasa);
                await _context.SaveChangesAsync();
                TempData["Mesaj"] = "Yeni masa başarıyla eklendi.";
                TempData["Icon"] = "success";
            }
            return RedirectToAction("MasaYonetimi");
        }

        [HttpPost]
        public async Task<IActionResult> MasaSil(int id)
        {
            var masa = await _context.Masalar.FindAsync(id);
            if (masa != null)
            {
                var aktifSiparisVarMi = await _context.Adisyonlar.AnyAsync(a => a.MasaId == id && !a.OdendiMi);
                if (aktifSiparisVarMi)
                {
                    TempData["Mesaj"] = "Bu masada açık hesap var! Önce hesabı kapatmalısınız.";
                    TempData["Icon"] = "error";
                }
                else
                {
                    _context.Masalar.Remove(masa);
                    await _context.SaveChangesAsync();
                    TempData["Mesaj"] = "Masa sistemden silindi.";
                    TempData["Icon"] = "success";
                }
            }
            return RedirectToAction("MasaYonetimi");
        }
    }
}