using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseGameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Lessons_LessonId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_LessonId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Quizzes");

            migrationBuilder.AddColumn<int>(
                name: "CourseGameId",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CourseGame",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseGame", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "CourseGame",
                columns: new[] { "Id", "CourseId", "IsDelete", "Name", "Status", "Url" },
                values: new object[] { 1, null, false, "Introduction to programming.", (byte)1, "https://kidspro-capstone.github.io/Capstone-Game-WebGL/" });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseGameId",
                table: "Courses",
                column: "CourseGameId",
                unique: true,
                filter: "[CourseGameId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseGame_CourseGameId",
                table: "Courses",
                column: "CourseGameId",
                principalTable: "CourseGame",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseGame_CourseGameId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "CourseGame");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseGameId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseGameId",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "Quizzes",
                type: "int",
                nullable: true);

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
                name: "IX_Quizzes_LessonId",
                table: "Quizzes",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Lessons_LessonId",
                table: "Quizzes",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }
    }
}
