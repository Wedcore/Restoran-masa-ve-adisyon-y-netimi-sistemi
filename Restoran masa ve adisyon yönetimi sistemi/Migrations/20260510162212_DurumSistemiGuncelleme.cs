using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Migrations
{
    /// <inheritdoc />
    public partial class DurumSistemiGuncelleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Durum",
                table: "Rezervasyonlar");

            migrationBuilder.AddColumn<int>(
                name: "DurumId",
                table: "Rezervasyonlar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RezervasyonDurumlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RezervasyonDurumlar", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "RezervasyonDurumlar",
                columns: new[] { "Id", "Ad" },
                values: new object[,]
                {
                    { 1, "Beklemede" },
                    { 2, "Onaylandı" },
                    { 3, "Reddedildi" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rezervasyonlar_DurumId",
                table: "Rezervasyonlar",
                column: "DurumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervasyonlar_RezervasyonDurumlar_DurumId",
                table: "Rezervasyonlar",
                column: "DurumId",
                principalTable: "RezervasyonDurumlar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervasyonlar_RezervasyonDurumlar_DurumId",
                table: "Rezervasyonlar");

            migrationBuilder.DropTable(
                name: "RezervasyonDurumlar");

            migrationBuilder.DropIndex(
                name: "IX_Rezervasyonlar_DurumId",
                table: "Rezervasyonlar");

            migrationBuilder.DropColumn(
                name: "DurumId",
                table: "Rezervasyonlar");

            migrationBuilder.AddColumn<string>(
                name: "Durum",
                table: "Rezervasyonlar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
