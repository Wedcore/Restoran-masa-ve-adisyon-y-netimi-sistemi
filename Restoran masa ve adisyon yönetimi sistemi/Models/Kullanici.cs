namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Models
{
    public class Kullanici
    {
        public int Id { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public string Rol { get; set; } // Burası "Admin" veya "User" olacak
    }
}