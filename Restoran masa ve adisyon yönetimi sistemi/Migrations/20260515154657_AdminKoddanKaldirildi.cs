using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Migrations
{
    /// <inheritdoc />
    public partial class AdminKoddanKaldirildi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Kullanicilar",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Kullanicilar",
                columns: new[] { "Id", "KullaniciAdi", "Rol", "Sifre" },
                values: new object[] { 1, "admin", "Admin", "1234" });
        }
    }
}
