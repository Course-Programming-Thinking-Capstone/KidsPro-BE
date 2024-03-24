using System;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Slot",
                table: "ClassSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudyDay",
                table: "ClassSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CloseDate",
                table: "Classes",
                type: "datetime2(2)",
                precision: 2,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StudentClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentClass_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                    principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_StudentClass_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_StudentClass_ClassId",
                table: "StudentClass",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClass_StudentId",
                table: "StudentClass",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentClass");

            migrationBuilder.DropColumn(
                name: "Slot",
                table: "ClassSchedules");

            migrationBuilder.DropColumn(
                name: "StudyDay",
                table: "ClassSchedules");

            migrationBuilder.DropColumn(
                name: "CloseDate",
                table: "Classes");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 15, 14, 25, 284, DateTimeKind.Utc).AddTicks(5728), "$2a$11$pgVAdft7yPxz5kZeXHFMJO.bi5wuaPp9n47ePKBA8kSBXQ4HkUgrG" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 15, 14, 25, 519, DateTimeKind.Utc).AddTicks(6229), "$2a$11$yTR8lrtlUrqBoWxqKfjpNur79zC8jM3o1XIUArPuFdNmouCWK9qOe" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 15, 14, 25, 748, DateTimeKind.Utc).AddTicks(698), "$2a$11$SXpjG7x/b/lKmTtX3Cb0aOCUtMLImzSzvCB2zn9qtIWbwC4OkxmDK" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 15, 14, 25, 982, DateTimeKind.Utc).AddTicks(7169), "$2a$11$afyyZP0qviysonUcAeEdgOntF1T4SwYMH7fa6uLog14n3XHKYqtY." });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 15, 14, 26, 210, DateTimeKind.Utc).AddTicks(3526), "$2a$11$rN78sP.Xoy4hp6p.gOc/LO//8GZ/gtEmUwpaYtHK9elJjI5wErTdm" });
        }
    }
}
