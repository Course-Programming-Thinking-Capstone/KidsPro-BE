using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationOrderDetailClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "OrderDetails",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 7, 5, 35, 14, 267, DateTimeKind.Utc).AddTicks(8066), "$2a$11$ACTEiyoB9T22PU6jytLmDOEaaRQeP7bns9RJHMTSNUOQGKk6ycP6y" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 7, 5, 35, 14, 443, DateTimeKind.Utc).AddTicks(8802), "$2a$11$rUlq4ABq2hfkzcI8CgT6ye1bdd9LTHjmhdSleSa4jR5JDMOMRSwqq" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 7, 5, 35, 14, 605, DateTimeKind.Utc).AddTicks(4891), "$2a$11$fiDRGrB9.kpC7Nqle9gl2eu2DHv/hWN57NRxba5oykajG03NjHukO" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 7, 5, 35, 14, 772, DateTimeKind.Utc).AddTicks(4108), "$2a$11$/pgJQtEnr5G7NfBK6AS37.atfyd1rCb0jHoCKS8ewNB0nRnigDb9." });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 7, 5, 35, 14, 963, DateTimeKind.Utc).AddTicks(7351), "$2a$11$VTsJ97HE0d8U.rYmdw7zoOALtj44RSRXM3kQ9Me2oHdEbuP69XOz2" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ClassId",
                table: "OrderDetails",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Classes_ClassId",
                table: "OrderDetails",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Classes_ClassId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ClassId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "OrderDetails");

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
    }
}
