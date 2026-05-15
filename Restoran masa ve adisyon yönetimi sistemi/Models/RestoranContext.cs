using Microsoft.EntityFrameworkCore;

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Models
{
    public class RestoranContext : DbContext
    {
        public RestoranContext(DbContextOptions<RestoranContext> options) : base(options)
        {
        }

        // 📌 Temel tablolarımız
        public DbSet<Masa> Masalar { get; set; }
        public DbSet<Rezervasyon> Rezervasyonlar { get; set; }
        public DbSet<RezervasyonDurum> RezervasyonDurumlar { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Adisyon> Adisyonlar { get; set; }
        public DbSet<Satis> Satislar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1️⃣ REZERVASYON DURUMLARI (SEED DATA)
            modelBuilder.Entity<RezervasyonDurum>().HasData(
                new RezervasyonDurum { Id = 1, Ad = "Beklemede" },
                new RezervasyonDurum { Id = 2, Ad = "Onaylandı" },
                new RezervasyonDurum { Id = 3, Ad = "Reddedildi" }
            );

            // 2️⃣ BAŞLANGIÇ MASALARI (Otomatik 5 Masa)
            modelBuilder.Entity<Masa>().HasData(
                new Masa { Id = 1, MasaNo = "Masa 1", DoluMu = false },
                new Masa { Id = 2, MasaNo = "Masa 2", DoluMu = false },
                new Masa { Id = 3, MasaNo = "Masa 3", DoluMu = false },
                new Masa { Id = 4, MasaNo = "Masa 4", DoluMu = false },
                new Masa { Id = 5, MasaNo = "Masa 5", DoluMu = false }
            );

            // ❌ Admin hesabı buradan kaldırıldı! (Hocanın istediği gibi, doğrudan SQL'den eklenecek)
        }
    }
}