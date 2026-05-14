using System; // <--- BUNU EKLEMEZSEK HATA VERİR

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Models
{
    public class Satis
    {
        public int Id { get; set; }
        public int MasaId { get; set; }
        public decimal ToplamTutar { get; set; }
        public DateTime SatisTarihi { get; set; }
        public string OdemeYontemi { get; set; }
    }
}