using Microsoft.AspNetCore.Mvc;
using Restoran_masa_ve_adisyon_yönetimi_sistemi.Models;
using System.Linq;
using System;

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Controllers
{
    public class AdisyonController : Controller
    {
        private readonly RestoranContext _context;

        public AdisyonController(RestoranContext context)
        {
            _context = context;
        }

        // 1. SİPARİŞ (ÜRÜN) EKLEME İŞLEMİ
        [HttpPost]
        public IActionResult Ekle(Adisyon yeniSiparis)
        {
            // Yeni siparişi SQL'e ekle
            _context.Adisyonlar.Add(yeniSiparis);

            // Eğer masa önceden boşsa, ilk sipariş girildiğinde masayı otomatik "Dolu" yap
            var masa = _context.Masalar.Find(yeniSiparis.MasaId);
            if (masa != null && masa.DoluMu == false)
            {
                masa.DoluMu = true;
                _context.Masalar.Update(masa);
            }

            _context.SaveChanges();

            // İşlem bitince masanın detay sayfasına geri dön
            return RedirectToAction("Detay", "Masa", new { id = yeniSiparis.MasaId });
        }

        // 2. HESABI KAPAT, KASAYA YAZ VE MASAYI BOŞALT (YENİ SİSTEM)
        [HttpPost]
        public IActionResult HesapKapat(int masaId)
        {
            // A. Bu masaya ait olan tüm siparişleri SQL'den bul
            var siparisler = _context.Adisyonlar.Where(a => a.MasaId == masaId).ToList();

            if (siparisler.Any())
            {
                // B. Masanın toplam tutarını hesapla
                decimal toplam = siparisler.Sum(a => (a.Adet * a.Fiyat));

                // C. Satislar tablosuna bu geliri kaydet (Kasa girişi)
                var yeniSatis = new Satis
                {
                    MasaId = masaId,
                    ToplamTutar = toplam,
                    SatisTarihi = DateTime.Now
                };
                _context.Satislar.Add(yeniSatis);

                // D. Siparişleri adisyon listesinden temizle
                _context.Adisyonlar.RemoveRange(siparisler);
            }

            // E. Masayı bul ve tekrar "Boş" durumuna getir
            var masa = _context.Masalar.Find(masaId);
            if (masa != null)
            {
                masa.DoluMu = false;
                _context.Masalar.Update(masa);
            }

            // F. Tüm değişiklikleri SQL'e işle
            _context.SaveChanges();

            // G. İşlem bitince ana masalar listesine dön
            return RedirectToAction("Index", "Masa");
        }
    }
}