using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Games_SectionId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GameQuizRoomId",
                table: "StudentMiniGames");

            migrationBuilder.DropColumn(
                name: "EndSaleDate",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "RequireAdminApproval",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "StartSaleDate",
                table: "Courses");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 23, 15, 23, 52, 479, DateTimeKind.Utc).AddTicks(3482), "$2a$11$jJeglJ0gGdVTledAVRzvXeTCYxQhjONq3ptt594skn9rErEIlelQi" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 23, 15, 23, 52, 777, DateTimeKind.Utc).AddTicks(5807), "$2a$11$OJ3xIz9YqHVhsFeY856v3ujbptHXxfNO0TSsVIHswfpw0EHAJQtfK" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 23, 15, 23, 53, 99, DateTimeKind.Utc).AddTicks(394), "$2a$11$prBhys2x4ncvlawNbSxMG.j1MWOjgfRbQU1LkNFN/LArcqEZFM2T6" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 23, 15, 23, 53, 401, DateTimeKind.Utc).AddTicks(433), "$2a$11$8XDI6nJFzIkxxttDwC0MeuOvdY1keAZttnvB8MlilhV/U4/Xd.KWu" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 23, 15, 23, 53, 717, DateTimeKind.Utc).AddTicks(6325), "$2a$11$/UIvewuLPqzKMd.SjFD6fOB/6YkBhiq5myYX144j8GSMSL71IdO9y" });

            migrationBuilder.CreateIndex(
                name: "IX_Games_SectionId",
                table: "Games",
                column: "SectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Games_SectionId",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "GameQuizRoomId",
                table: "StudentMiniGames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndSaleDate",
                table: "Courses",
                type: "datetime2(2)",
                precision: 2,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequireAdminApproval",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartSaleDate",
                table: "Courses",
                type: "datetime2(2)",
                precision: 2,
                nullable: true);

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
                name: "IX_Games_SectionId",
                table: "Games",
                column: "SectionId",
                unique: true);
        }
    }
}
