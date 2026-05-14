using Microsoft.AspNetCore.Mvc;
using Restoran_masa_ve_adisyon_yönetimi_sistemi.Models;
using System.Linq; // Veritabanında filtreleme (Where) yapmak için bu şart

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class MasaController : Controller
    {
        // SQL Bağlantımız
        private readonly RestoranContext _context;

        public MasaController(RestoranContext context)
        {
            _context = context;
        }

        // 1. ANA SAYFA: Tüm Masaları Listeler
        public IActionResult Index()
        {
            var masalar = _context.Masalar.ToList();
            return View(masalar);
        }

        // 2. YENİ MASA EKLEME (Boş Formu Açar)
        public IActionResult Ekle()
        {
            return View();
        }

        // 3. YENİ MASA EKLEME (Formdan Gelen Veriyi SQL'e Kaydeder)
        [HttpPost]
        public IActionResult Ekle(Masa yeniMasa)
        {
            _context.Masalar.Add(yeniMasa);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // 4. MASA DETAYI VE ADİSYON GÖRÜNTÜLEME (Yeni Eklediğimiz Kısım)
        public IActionResult Detay(int id)
        {
            // Tıklanan masayı bul
            var masa = _context.Masalar.Find(id);
            if (masa == null)
            {
                // Eğer masa yoksa hata sayfası ver
                return NotFound();
            }

            // Sadece bu masaya ait olan siparişleri getir
            var siparisler = _context.Adisyonlar.Where(a => a.MasaId == id).ToList();

            // Sayfada (View) yeni sipariş eklerken kullanmak için Masa'nın ID'sini gönder
            ViewBag.MasaId = id;

            // Sipariş listesini sayfaya yansıt
            return View(siparisler);
        }
    }
}   