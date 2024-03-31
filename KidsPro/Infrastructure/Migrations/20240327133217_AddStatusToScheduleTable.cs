using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToScheduleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ClassSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 27, 13, 32, 14, 851, DateTimeKind.Utc).AddTicks(6001), "$2a$11$qby27PEU.7yu7iIIa6XCcO42luMteAZiT0PldGXgBIjOgwv19U.3a" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 27, 13, 32, 15, 101, DateTimeKind.Utc).AddTicks(2110), "$2a$11$GnX5Ym58Su2u8Rx00Z3iNOyTge1wNcDVHrDLKvptCm.3gYK2M0XgC" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 27, 13, 32, 15, 511, DateTimeKind.Utc).AddTicks(9825), "$2a$11$RK4hLrgv0Ibkns8vVoJO1.W31wf9OeK2vot2Xi.jPAL1LdZRZQSh6" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 27, 13, 32, 15, 808, DateTimeKind.Utc).AddTicks(8594), "$2a$11$N3eM9vJko3z.V7pbKNI8/enOO35xUJv0ZCjA0QDjKbzdMn8iQoXLK" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 27, 13, 32, 16, 91, DateTimeKind.Utc).AddTicks(8042), "$2a$11$lPXgWoZTUk1kDjAHBooXf.qQXPr5a5k0NKFg2he.o1x/HMf6JMzAW" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ClassSchedules");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 26, 4, 12, 54, 509, DateTimeKind.Utc).AddTicks(4695), "$2a$11$6ha6FueEZiQnabbcJ9Id3.2iJeA8E/TlpvzgVQHew1QqIdcv9fBRC" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 26, 4, 12, 54, 727, DateTimeKind.Utc).AddTicks(2647), "$2a$11$lnl7hfgNLBsFAhWxisdhMuLYn/eC/RdZCHP/LJgG8YSBgNwCsMAZa" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 26, 4, 12, 54, 940, DateTimeKind.Utc).AddTicks(6086), "$2a$11$aNyGHR7kiY2CWHg7wQKEgeZ8Tam7AjIxWg1DygITuo1ItHxP0eW/W" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 26, 4, 12, 55, 159, DateTimeKind.Utc).AddTicks(5593), "$2a$11$GNelZB770TYJGe.NpeXc0.zj5iE0xgepiC8Ky6Y1EhBWpQy.0Em.O" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 26, 4, 12, 55, 385, DateTimeKind.Utc).AddTicks(109), "$2a$11$Kr5kvDNOQr19wrw3HfJ/1ufDEV9VD4xDXYjQSE8lgsh45Hbwfe0lK" });
        }
    }
}
