using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Migrations
{
    /// <inheritdoc />
    public partial class HesapAdiEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HesapAdi",
                table: "Rezervasyonlar",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HesapAdi",
                table: "Rezervasyonlar");
        }
    }
}
