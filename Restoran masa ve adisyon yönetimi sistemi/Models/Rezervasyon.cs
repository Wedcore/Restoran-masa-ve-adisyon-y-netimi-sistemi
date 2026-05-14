using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Models
{
    public class Rezervasyon
    {
        [Key]
        public int Id { get; set; }

        public int MasaId { get; set; }

        public string MusteriAdi { get; set; }

        public DateTime Tarih { get; set; }

        // 🛠️ AMELİYAT BURADA: Eski 'string Durum' gitti, yerine ID ve Tablo bağlantısı geldi!
        public int DurumId { get; set; }

        [ForeignKey("DurumId")]
        public RezervasyonDurum Durum { get; set; }

        public string? HesapAdi { get; set; }
    }
}