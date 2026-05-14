using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Migrations
{
    /// <inheritdoc />
    public partial class MasaKapasiteGuncellemesi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Kapasite",
                table: "Masalar",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kapasite",
                table: "Masalar");
        }
    }
}
