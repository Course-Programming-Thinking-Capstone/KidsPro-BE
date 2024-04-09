using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGameItemFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemRateType",
                table: "GameItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemType",
                table: "GameItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "GameItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 5, 14, 8, 41, 122, DateTimeKind.Utc).AddTicks(8016), "$2a$11$TKsb0KG9jsiiz9yVAzq0nO3iFhy9kSmPsQek24nt2H6SSPFwvdf72" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 5, 14, 8, 41, 342, DateTimeKind.Utc).AddTicks(9625), "$2a$11$gRW3SFBv5tPU.4UHKk1opeLNtpcAi.TMpZY7wES016ersyrl5TNse" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 5, 14, 8, 41, 564, DateTimeKind.Utc).AddTicks(1864), "$2a$11$0qukRYzHTe4ANwYonADAkePoHap0PrhL2oLE4QaGPgLBAiKOd8Z72" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 5, 14, 8, 41, 795, DateTimeKind.Utc).AddTicks(9380), "$2a$11$PqNu.ZeqMR.CYbvw7Dyv1e7QeHpmH4FjBqi9I0E.pMuOaMFoE21My" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 5, 14, 8, 42, 31, DateTimeKind.Utc).AddTicks(9571), "$2a$11$mySKqJxDob9uxlMpXxi6FO4Eif7rjg1/YyvwrCAtG/wiafaXOP6hK" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemRateType",
                table: "GameItems");

            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "GameItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "GameItems");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 2, 12, 16, 5, 604, DateTimeKind.Utc).AddTicks(5791), "$2a$11$cvyQRCBgS9Qk.OSmC8otjOQNekSpjztsqWwfwnHJtpUPk7LgrLthm" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 2, 12, 16, 5, 944, DateTimeKind.Utc).AddTicks(6548), "$2a$11$NDMCGhdjJWDD.30sd8BzVeaJv8YX2Hm2/o2dbk9N/uf1jiiy0xd0u" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 2, 12, 16, 6, 156, DateTimeKind.Utc).AddTicks(4988), "$2a$11$Eqw13V4OQ4KkHiJjp9Lf6OL.kBI78waRko9rY1/Hwpe1evEBs/uAq" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 2, 12, 16, 6, 414, DateTimeKind.Utc).AddTicks(7153), "$2a$11$iO1fVSGVSDn1ZGU8to.VuOUSo1vx7Qs.RugU4bLe6YoHLuUu950ca" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 2, 12, 16, 6, 693, DateTimeKind.Utc).AddTicks(3301), "$2a$11$AVmgnYgLO9c4FDWBizjtWecVuoTu1pEcP/W7qOD7XIYWij60W/Sra" });
        }
    }
}
