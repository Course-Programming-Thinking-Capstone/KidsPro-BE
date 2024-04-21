using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseGameRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseGameId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "CourseGame");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 21, 2, 23, 25, 913, DateTimeKind.Utc).AddTicks(7090), "$2a$11$Y1ZGJfkprmOLeo4Qi41VAeI8PHv7h8QJg20euf5xy7eMR4sP5IXha" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 21, 2, 23, 26, 404, DateTimeKind.Utc).AddTicks(5285), "$2a$11$D64hCBdtIWJFOcjkEeeLzuW9cxdIL7FjtRR9ny6xt8zmNuPP8k37G" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 21, 2, 23, 26, 745, DateTimeKind.Utc).AddTicks(9208), "$2a$11$OuBmYB6.Kx6P5d8.ZfjPxOlHbJYaxcjcFvPvZ0DnxGuxraY0FPx.S" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 21, 2, 23, 27, 51, DateTimeKind.Utc).AddTicks(2955), "$2a$11$VgaBgK/ro1tm71CAGlCM0uiH5IAkm/gTLZsydRpEFboTapMs01HHy" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 21, 2, 23, 27, 404, DateTimeKind.Utc).AddTicks(7743), "$2a$11$eQlAQpt5BTCgUKcQM0JPB.dAg27NmzYUTv9ba/2Tx7r3bRwd3aN1m" });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseGameId",
                table: "Courses",
                column: "CourseGameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseGameId",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "CourseGame",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 19, 14, 38, 54, 801, DateTimeKind.Utc).AddTicks(6358), "$2a$11$GXsljSyPlE75Wjfj3WYCu.7yJj2LuogjXFQravmGPev4bbu3U80RG" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 19, 14, 38, 55, 30, DateTimeKind.Utc).AddTicks(6490), "$2a$11$bu5t4NGI0AFft8qqK2tB.OySs.4FdJVpA.EOm.pnOTWQK26F6Tq9G" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 19, 14, 38, 55, 279, DateTimeKind.Utc).AddTicks(9553), "$2a$11$Ib2NT08XJHi7.CZAOBT94exuPkICIYdxRv.81Q.jlgsvAGVjjn9le" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 19, 14, 38, 55, 498, DateTimeKind.Utc).AddTicks(7845), "$2a$11$UitaKH8rn4wWE8Yz2vvmNelR/dURZhKUSKaa/jEa9fQr8Dxg3G31W" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 19, 14, 38, 55, 723, DateTimeKind.Utc).AddTicks(6819), "$2a$11$B2aLVmUcT.qmWY9YGMp.juAztdEiDzqUW2o7n.L8hCL0dytemgkFi" });

            migrationBuilder.UpdateData(
                table: "CourseGame",
                keyColumn: "Id",
                keyValue: 1,
                column: "CourseId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseGameId",
                table: "Courses",
                column: "CourseGameId",
                unique: true,
                filter: "[CourseGameId] IS NOT NULL");
        }
    }
}
