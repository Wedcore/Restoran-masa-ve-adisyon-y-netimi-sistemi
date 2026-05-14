using System; // <--- EKSİK OLAN BU!

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Models
{
    public class Adisyon
    {
        public int Id { get; set; }
        public int MasaId { get; set; }
        public string UrunAdi { get; set; }
        public int Adet { get; set; }
        public decimal Fiyat { get; set; }
        public DateTime Tarih { get; set; }
        public bool OdendiMi { get; set; }
    }
}