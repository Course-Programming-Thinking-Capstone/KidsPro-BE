using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_GameVouchers_VoucherId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_OrderCode",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Orders_VoucherId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderCode",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Transaction");

            migrationBuilder.AlterColumn<int>(
                name: "VoucherId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentType",
                table: "Orders",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "OrderCode",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 16, 9, 34, 56, 533, DateTimeKind.Utc).AddTicks(9351), "$2a$11$U31v77rmpRptEsTvEvG0yO.0dthzmsG.COhsQn0FISrxKTKeaVWsa" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 16, 9, 34, 56, 773, DateTimeKind.Utc).AddTicks(6476), "$2a$11$LlynMoIR6PuLcurFQ16Rtet4Zg/0tclPaL.KruenauxGYE7F./eVq" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 16, 9, 34, 57, 9, DateTimeKind.Utc).AddTicks(3665), "$2a$11$1iqLhaEU4WM.35tK.WjpAuPAjBpEVIRldklGUhL.m9tMbl4Ap0dgy" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 16, 9, 34, 57, 260, DateTimeKind.Utc).AddTicks(124), "$2a$11$9r9kpOFihqMXOK6uGM1LDedxqmLkgRrS5hzkKqe7xE9kZE5g9/eAa" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 16, 9, 34, 57, 503, DateTimeKind.Utc).AddTicks(7435), "$2a$11$hTkv1U.ahgEeNgHJY67Y.eeleRPTKWmoQ4V2TAMOLEuk1NyxWnV8e" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderCode",
                table: "Orders",
                column: "OrderCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_VoucherId",
                table: "Orders",
                column: "VoucherId",
                unique: true,
                filter: "[VoucherId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_GameVouchers_VoucherId",
                table: "Orders",
                column: "VoucherId",
                principalTable: "GameVouchers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_GameVouchers_VoucherId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderCode",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_VoucherId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderCode",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "OrderCode",
                table: "Transaction",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "PaymentType",
                table: "Transaction",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AlterColumn<int>(
                name: "VoucherId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentType",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 15, 14, 48, 43, 166, DateTimeKind.Utc).AddTicks(3527), "$2a$11$miFDucadUxOErDcXHzQC1u/ftYgLqOJZh8MDSwgie01qF4rBrP9oq" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 15, 14, 48, 43, 384, DateTimeKind.Utc).AddTicks(1270), "$2a$11$s/Oha8p0Rl5dIDAZuRSr8ueRAqyZZPZE/mfB.aWMo082DOB6e2hFu" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 15, 14, 48, 43, 597, DateTimeKind.Utc).AddTicks(1445), "$2a$11$ihXJeV5f81KMrXXCKhNqJ.gi./qIq4zbYo4/NxKcvzD1P6.wSJcv6" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 15, 14, 48, 43, 819, DateTimeKind.Utc).AddTicks(8602), "$2a$11$ZosnPUTVZbqcNRiCnIHWXe6L5HoWmLpARSECaVQ8WbwB.cyoKSCQa" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 15, 14, 48, 44, 50, DateTimeKind.Utc).AddTicks(6630), "$2a$11$CtsBkz55lYP5cwHPoQDB7u4.xIzateKgpIPxzTcgHSbC7U.cWI6i." });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_OrderCode",
                table: "Transaction",
                column: "OrderCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_VoucherId",
                table: "Orders",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_GameVouchers_VoucherId",
                table: "Orders",
                column: "VoucherId",
                principalTable: "GameVouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
