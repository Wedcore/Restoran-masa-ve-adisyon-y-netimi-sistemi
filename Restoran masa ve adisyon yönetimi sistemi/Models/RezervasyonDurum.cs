using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Models
{
    public class RezervasyonDurum
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Ad { get; set; } // "Beklemede", "Onaylandı", "Reddedildi" gibi

        // Bu duruma sahip tüm rezervasyonları listelemek için (Opsiyonel)
        public ICollection<Rezervasyon> Rezervasyonlar { get; set; }
    }
}