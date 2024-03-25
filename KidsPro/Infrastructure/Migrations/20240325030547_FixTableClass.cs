using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTableClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TotalStudent",
                table: "Classes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TotalSlot",
                table: "Classes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "Classes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "Duration",
                table: "Classes",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 25, 3, 5, 45, 952, DateTimeKind.Utc).AddTicks(5237), "$2a$11$Y9kO7DrUDO51XIMXg39ECeXSE/ukpm5aZgm6nsLWTPKyhhnp0MqvS" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 25, 3, 5, 46, 187, DateTimeKind.Utc).AddTicks(7994), "$2a$11$uU/zpkziKXhuMgabn9i9KuU264eqHU22A1stcwEsTwhUuKhEZzf/C" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 25, 3, 5, 46, 424, DateTimeKind.Utc).AddTicks(3514), "$2a$11$X6SHsqs.f6vOniNVl3K26O7TtA185NQhcIceHv5eUJxj8Kf6KtTKW" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 25, 3, 5, 46, 637, DateTimeKind.Utc).AddTicks(3984), "$2a$11$YW8WXB9geRpWTp6hYhcQa.29SvbHSWl7LNJfypNtxLn8PhhL2FSiu" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 25, 3, 5, 46, 801, DateTimeKind.Utc).AddTicks(5421), "$2a$11$MW5uZYMZkQJsEiasrdtSM.C.T5JXap820j5rihZqcLETv5vCX.kku" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TotalStudent",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TotalSlot",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Duration",
                table: "Classes",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 24, 10, 32, 0, 98, DateTimeKind.Utc).AddTicks(5875), "$2a$11$1DZFFl2mVTzyQC87ib5SienS.7vn9pDM7bOQLNdScv03BOAo6yZRG" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 24, 10, 32, 0, 268, DateTimeKind.Utc).AddTicks(9957), "$2a$11$gU1tmamkB9LmnDKI6rEkWO9..LRebg.uaTodzHIDabvJ4NJJMzy7O" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 24, 10, 32, 0, 494, DateTimeKind.Utc).AddTicks(4107), "$2a$11$1ObPzZBIUOBfPtVYU6BXO.VVrz4Fc3GZ2KjVkIMyfYG6OQGk2aG.e" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 24, 10, 32, 0, 704, DateTimeKind.Utc).AddTicks(3545), "$2a$11$rS1gFf9fkNm125GIpA4PMukYm6PwT7C1XDY53r0kZ3LtbrQvKg27W" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 24, 10, 32, 0, 924, DateTimeKind.Utc).AddTicks(9407), "$2a$11$prnzrPaz16BoxcxVNQq7jelgCtuHW/25yrb.Knc/wKYBwBDJIJdG2" });
        }
    }
}
