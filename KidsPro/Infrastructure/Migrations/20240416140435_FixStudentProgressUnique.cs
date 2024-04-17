using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixStudentProgressUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentProgresses_StudentId_CourseId",
                table: "StudentProgresses");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 16, 14, 4, 33, 599, DateTimeKind.Utc).AddTicks(6476), "$2a$11$LZWeYBAuAf4x/CYIm68zpum5Dv6yS3jwtd1IPR2r36QFC9mDJWIze" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 16, 14, 4, 33, 823, DateTimeKind.Utc).AddTicks(5095), "$2a$11$RUo8l6h9sIJH5atUZFfJZeN88LJtGDO5NRndn7a443knrShGYHaiG" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 16, 14, 4, 34, 61, DateTimeKind.Utc).AddTicks(6735), "$2a$11$qon0TmLSIBbMHrpCAJVoweP1810E9dTqWQ6u/Hh/KwKPQ8FVYL/3C" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 16, 14, 4, 34, 347, DateTimeKind.Utc).AddTicks(7824), "$2a$11$d1ICdaNzrm.LtjD2IsGMY.hsCVdWqmAizGAhCpt5Mw3Xj5PNc6Ola" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 16, 14, 4, 34, 579, DateTimeKind.Utc).AddTicks(3782), "$2a$11$DY3HgfYWKQQfiODN3rR6K.CzFVRSaAmeK5tbWkynmriTTgvP2YtOS" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgresses_StudentId_SectionId",
                table: "StudentProgresses",
                columns: new[] { "StudentId", "SectionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentProgresses_StudentId_SectionId",
                table: "StudentProgresses");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 16, 12, 11, 44, 870, DateTimeKind.Utc).AddTicks(3035), "$2a$11$kGxmoEAto4PtU9U/iUvUeOkEadxBFpLoTIztkdNOf4HH/nUq.vBBG" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 16, 12, 11, 45, 119, DateTimeKind.Utc).AddTicks(2119), "$2a$11$RIikhBhJ.2uDHglJngE5XObbhAdr5YixmPvtmYQ5U0G1sCcf2Cm4i" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 16, 12, 11, 45, 492, DateTimeKind.Utc).AddTicks(5439), "$2a$11$7S4H.qoZnmcksC6JOZ5/c.2v1HRAFXuQWtqbp7aNptiMMzQPHBWQC" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 16, 12, 11, 45, 942, DateTimeKind.Utc).AddTicks(1225), "$2a$11$h0uzFy5KiabJEWBS3.hcVe5Dr6yUZyak25n7HZoxuSzppusahoeoa" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 16, 12, 11, 46, 389, DateTimeKind.Utc).AddTicks(1498), "$2a$11$sXx1.9rhbzIokYb1UJ58wOX/pi5VWTmZANyFjcqIYhaw8./2LccCu" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgresses_StudentId_CourseId",
                table: "StudentProgresses",
                columns: new[] { "StudentId", "CourseId" },
                unique: true);
        }
    }
}
