using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editFeildToFacebook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Field",
                table: "Teachers",
                newName: "Facebook");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 30, 3, 35, 49, 276, DateTimeKind.Utc).AddTicks(3190), "$2a$11$SpvU2JLJC.wHZ01QTB3xbuclvGp3EiHFvZGnav1gnOM/NUeotQaJ2" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 30, 3, 35, 49, 507, DateTimeKind.Utc).AddTicks(6981), "$2a$11$5RIxaJ6T.eFtwJByOCni6OhrXnjDH3DyYQvgTgNbP.sNSD2zMB7JK" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 30, 3, 35, 49, 744, DateTimeKind.Utc).AddTicks(9216), "$2a$11$F5E82iGB4odkaykxr/0aIuM1nhSe4buny0mcdUF4BjCfFHdwXjuze" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 30, 3, 35, 49, 969, DateTimeKind.Utc).AddTicks(5223), "$2a$11$h5QucuArXdKVPFaVQAbpvua8dEF5TKCiCEZ3lRDo1K9zwhaRFyZvm" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 30, 3, 35, 50, 186, DateTimeKind.Utc).AddTicks(3703), "$2a$11$XPhzEnDt5WTZ7LWm61Ch1uhDD4XyFNnqMGt6x4n6MjFIEb5qP/zVK" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Facebook",
                table: "Teachers",
                newName: "Field");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 29, 17, 1, 50, 970, DateTimeKind.Utc).AddTicks(5438), "$2a$11$R/31MFj1jDj/6WWZm/6GSuwV1wwXrobiSMvvYwFUzEWb7s/Ptgn56" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 29, 17, 1, 51, 266, DateTimeKind.Utc).AddTicks(7700), "$2a$11$EyKuzj9CKx4y3jbW9x3TJ.TSrPj0py3o2tedD19SCB/Vz2dx9VBIC" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 29, 17, 1, 51, 475, DateTimeKind.Utc).AddTicks(2247), "$2a$11$n2ne5IkrF2jmjQTPJDCrEuTQWnTTew5HNQt7O2Lx71czF/ZTsMABm" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 29, 17, 1, 51, 829, DateTimeKind.Utc).AddTicks(2743), "$2a$11$z4ZU9SFb5x1KVGxXYRnYoug8FiBXNo70aVWqo9O2jXc/E5j7/mtt." });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 29, 17, 1, 52, 150, DateTimeKind.Utc).AddTicks(5873), "$2a$11$qRFkeqRgQevNNz.r1LrsEemi5Xps/5ke7gW5fGvlLhMMHp1LYeV2e" });
        }
    }
}
