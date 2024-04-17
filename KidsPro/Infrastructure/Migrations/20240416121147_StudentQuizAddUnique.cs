using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StudentQuizAddUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentQuizzes_StudentId",
                table: "StudentQuizzes");

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
                name: "IX_StudentQuizzes_StudentId_QuizId",
                table: "StudentQuizzes",
                columns: new[] { "StudentId", "QuizId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentQuizzes_StudentId_QuizId",
                table: "StudentQuizzes");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 15, 7, 58, 2, 189, DateTimeKind.Utc).AddTicks(2833), "$2a$11$QL5ya5pE4KTLoEQPAdZfD.BEzUjE9npWOj1/N6J.Wa/Hf6QSZBnFC" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 15, 7, 58, 2, 498, DateTimeKind.Utc).AddTicks(5987), "$2a$11$ylr3HaXfR1jlXbZXM1qqeO6dvBdrH5fdlyBRs6u5ol1ZpFBAN0HUK" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 15, 7, 58, 2, 726, DateTimeKind.Utc).AddTicks(4559), "$2a$11$Wz8yKt8kWYCcl44LNAfMiOLt2AbkYzhXhF/tRWJEwaZdYGHpdyJj2" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 15, 7, 58, 2, 988, DateTimeKind.Utc).AddTicks(3092), "$2a$11$G15zWqEOUvrDMGpddF/dceAvYQCdHjsHPWwoiDMOmpZGxsa9bhWzK" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 15, 7, 58, 3, 212, DateTimeKind.Utc).AddTicks(7496), "$2a$11$7vIfaxcz78S1HkxNpupxj.hByvKSkpbA3JSKXPMGW7M4Lj9RC6mXy" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizzes_StudentId",
                table: "StudentQuizzes",
                column: "StudentId");
        }
    }
}
