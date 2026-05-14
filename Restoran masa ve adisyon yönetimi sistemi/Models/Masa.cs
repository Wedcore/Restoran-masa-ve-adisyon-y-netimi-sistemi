namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Models
{
    public class Masa
    {
        public int Id { get; set; }
        public string MasaNo { get; set; }
        public bool DoluMu { get; set; } // Masanın durumunu takip eder
    }
}