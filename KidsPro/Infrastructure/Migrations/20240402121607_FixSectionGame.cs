using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixSectionGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Lessons_LessonId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_LessonId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Games_SectionId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Lessons");

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

            migrationBuilder.CreateIndex(
                name: "IX_Games_SectionId",
                table: "Games",
                column: "SectionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Games_SectionId",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Transaction",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "TeacherProfiles",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "SlotTime",
                table: "Syllabus",
                newName: "SlotDuration");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Quizzes",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "GameLevelModifiers",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Courses",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "CourseResources",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Certificates",
                newName: "description");

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "Lessons",
                type: "int",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_LessonId",
                table: "Lessons",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_SectionId",
                table: "Games",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Lessons_LessonId",
                table: "Lessons",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }
    }
}
