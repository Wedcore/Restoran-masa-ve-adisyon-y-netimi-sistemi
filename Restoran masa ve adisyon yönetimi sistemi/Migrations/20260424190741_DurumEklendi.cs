using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restoran_masa_ve_adisyon_yönetimi_sistemi.Migrations
{
    /// <inheritdoc />
    public partial class DurumEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervasyonlar_Masalar_MasaId",
                table: "Rezervasyonlar");

            migrationBuilder.DropIndex(
                name: "IX_Rezervasyonlar_MasaId",
                table: "Rezervasyonlar");

            migrationBuilder.DropColumn(
                name: "KisiSayisi",
                table: "Rezervasyonlar");

            migrationBuilder.DropColumn(
                name: "Kapasite",
                table: "Masalar");

            migrationBuilder.RenameColumn(
                name: "Tarih",
                table: "Satislar",
                newName: "SatisTarihi");

            migrationBuilder.RenameColumn(
                name: "Telefon",
                table: "Rezervasyonlar",
                newName: "MusteriAdi");

            migrationBuilder.RenameColumn(
                name: "RezervasyonZamani",
                table: "Rezervasyonlar",
                newName: "Tarih");

            migrationBuilder.RenameColumn(
                name: "MusteriAd",
                table: "Rezervasyonlar",
                newName: "Durum");

            migrationBuilder.AddColumn<string>(
                name: "OdemeYontemi",
                table: "Satislar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "OdendiMi",
                table: "Adisyonlar",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Tarih",
                table: "Adisyonlar",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OdemeYontemi",
                table: "Satislar");

            migrationBuilder.DropColumn(
                name: "OdendiMi",
                table: "Adisyonlar");

            migrationBuilder.DropColumn(
                name: "Tarih",
                table: "Adisyonlar");

            migrationBuilder.RenameColumn(
                name: "SatisTarihi",
                table: "Satislar",
                newName: "Tarih");

            migrationBuilder.RenameColumn(
                name: "Tarih",
                table: "Rezervasyonlar",
                newName: "RezervasyonZamani");

            migrationBuilder.RenameColumn(
                name: "MusteriAdi",
                table: "Rezervasyonlar",
                newName: "Telefon");

            migrationBuilder.RenameColumn(
                name: "Durum",
                table: "Rezervasyonlar",
                newName: "MusteriAd");

            migrationBuilder.AddColumn<int>(
                name: "KisiSayisi",
                table: "Rezervasyonlar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Kapasite",
                table: "Masalar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rezervasyonlar_MasaId",
                table: "Rezervasyonlar",
                column: "MasaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervasyonlar_Masalar_MasaId",
                table: "Rezervasyonlar",
                column: "MasaId",
                principalTable: "Masalar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
