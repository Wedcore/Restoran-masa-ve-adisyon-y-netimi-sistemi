using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Migrations
{
    /// <inheritdoc />
    public partial class AdminVeMasaSeedEkle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Kullanicilar",
                columns: new[] { "Id", "KullaniciAdi", "Rol", "Sifre" },
                values: new object[] { 1, "admin", "Admin", "1234" });

            migrationBuilder.InsertData(
                table: "Masalar",
                columns: new[] { "Id", "DoluMu", "MasaNo" },
                values: new object[,]
                {
                    { 1, false, "Masa 1" },
                    { 2, false, "Masa 2" },
                    { 3, false, "Masa 3" },
                    { 4, false, "Masa 4" },
                    { 5, false, "Masa 5" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Kullanicilar",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Masalar",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Masalar",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Masalar",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Masalar",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Masalar",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
