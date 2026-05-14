using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Restoran_masa_ve_adisyon_yönetimi_sistemi.Models;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly RestoranContext _context;

        public HomeController(RestoranContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // --- 1. GENEL ÖZET VERİLERİ (Kartlar için) ---
            ViewBag.ToplamMasa = _context.Masalar.Count();
            ViewBag.DoluMasa = _context.Masalar.Count(m => m.DoluMu);

            ViewBag.BekleyenTutar = _context.Adisyonlar.Any()
                ? _context.Adisyonlar.Sum(a => a.Adet * a.Fiyat)
                : 0;

            ViewBag.ToplamCiro = _context.Satislar.Any()
                ? _context.Satislar.Sum(s => s.ToplamTutar)
                : 0;

            // --- 2. AYLIK CİRO VE REZERVASYON VERİLERİ ---
            var buAy = DateTime.Now.Month;
            var buYil = DateTime.Now.Year;

            decimal aylikCiro = _context.Satislar
                .Where(s => s.SatisTarihi.Month == buAy && s.SatisTarihi.Year == buYil)
                .Sum(s => (decimal?)s.ToplamTutar) ?? 0;

            ViewBag.AylikCiro = aylikCiro.ToString("N2");

            // 🛠️ AMELİYAT BURADA: Metin yerine ID sistemine (1 = Beklemede) geçtik!
            ViewBag.RezervasyonSayisi = _context.Rezervasyonlar.Count(r => r.DurumId == 1);


            // --- 3. GRAFİK VERİSİ HAZIRLAMA (Son 7 Gün) ---
            var sonYediGun = DateTime.Now.Date.AddDays(-6);

            var haftalikSatislar = _context.Satislar
                .Where(s => s.SatisTarihi >= sonYediGun)
                .ToList()
                .GroupBy(s => s.SatisTarihi.Date)
                .Select(g => new {
                    Tarih = g.Key,
                    Toplam = g.Sum(x => x.ToplamTutar)
                })
                .OrderBy(g => g.Tarih)
                .ToList();

            var grafikGunler = new List<string>();
            var grafikRakamlar = new List<decimal>();

            for (int i = 0; i < 7; i++)
            {
                var suAnkiGun = sonYediGun.AddDays(i);
                grafikGunler.Add(suAnkiGun.ToString("dd MMM"));
                var gunlukToplam = haftalikSatislar.FirstOrDefault(s => s.Tarih == suAnkiGun)?.Toplam ?? 0;
                grafikRakamlar.Add(gunlukToplam);
            }

            ViewBag.GrafikGunler = grafikGunler;
            ViewBag.GrafikRakamlar = grafikRakamlar;


            // --- 4. MASA BAZLI CİRO DAĞILIMI ---
            var masaCiroDagilimi = _context.Satislar
                .GroupBy(s => s.MasaId)
                .Select(g => new {
                    MasaNo = g.Key,
                    Toplam = g.Sum(x => x.ToplamTutar)
                })
                .OrderByDescending(x => x.Toplam)
                .Take(5)
                .ToList();

            ViewBag.MasaCiroListesi = masaCiroDagilimi;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}