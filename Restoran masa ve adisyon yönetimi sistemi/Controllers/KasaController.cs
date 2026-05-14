using Microsoft.AspNetCore.Mvc;
using Restoran_masa_ve_adisyon_yönetimi_sistemi.Models;
using System.Linq;

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize] // Sadece adminler görebilsin
    public class KasaController : Controller
    {
        private readonly RestoranContext _context;

        public KasaController(RestoranContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Tüm satışları tarihe göre (en yeni en üstte) çekiyoruz
            var satislar = _context.Satislar.OrderByDescending(s => s.SatisTarihi).ToList();

            // Toplam Ciroyu hesapla
            ViewBag.ToplamCiro = satislar.Sum(s => s.ToplamTutar);

            return View(satislar);
        }
    }
}