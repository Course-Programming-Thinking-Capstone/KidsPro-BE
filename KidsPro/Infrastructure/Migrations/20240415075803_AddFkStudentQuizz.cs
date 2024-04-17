using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFkStudentQuizz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentOptions_StudentQuizzes_StudentQuizStudentId_StudentQuizQuizId",
                table: "StudentOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentQuizzes",
                table: "StudentQuizzes");

            migrationBuilder.DropIndex(
                name: "IX_StudentOptions_StudentQuizStudentId_StudentQuizQuizId",
                table: "StudentOptions");

            migrationBuilder.DropColumn(
                name: "StudentQuizQuizId",
                table: "StudentOptions");

            migrationBuilder.DropColumn(
                name: "StudentQuizStudentId",
                table: "StudentOptions");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StudentQuizzes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "StudentQuizzes",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentQuizzes",
                table: "StudentQuizzes",
                column: "Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_StudentOptions_StudentQuizId",
                table: "StudentOptions",
                column: "StudentQuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentOptions_StudentQuizzes_StudentQuizId",
                table: "StudentOptions",
                column: "StudentQuizId",
                principalTable: "StudentQuizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentOptions_StudentQuizzes_StudentQuizId",
                table: "StudentOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentQuizzes",
                table: "StudentQuizzes");

            migrationBuilder.DropIndex(
                name: "IX_StudentQuizzes_StudentId",
                table: "StudentQuizzes");

            migrationBuilder.DropIndex(
                name: "IX_StudentOptions_StudentQuizId",
                table: "StudentOptions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StudentQuizzes");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "StudentQuizzes");

            migrationBuilder.AddColumn<int>(
                name: "StudentQuizQuizId",
                table: "StudentOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentQuizStudentId",
                table: "StudentOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentQuizzes",
                table: "StudentQuizzes",
                columns: new[] { "StudentId", "QuizId" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 8, 5, 56, 56, 858, DateTimeKind.Utc).AddTicks(4470), "$2a$11$RHVoEejUmS7n65UQ1T8IT.cmHoFD6vzfeXbwKzLFFe774kMH7X8HW" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 8, 5, 56, 57, 179, DateTimeKind.Utc).AddTicks(1521), "$2a$11$HJqoi7zPTLhrsSlRp9FmKO98I4i.g7CFQD1axv9ErMPrNe7ZNnce6" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 8, 5, 56, 57, 507, DateTimeKind.Utc).AddTicks(3008), "$2a$11$sMhaSq3SmB6GS6.ej60NSeM/XhKQ8Wxs.C2GoIJiNNSU3zKuQ8qPi" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 8, 5, 56, 57, 984, DateTimeKind.Utc).AddTicks(4656), "$2a$11$.jYiA8tjiQHF72vmM9C3WOuhJr1WNmDVKkyK4rhM9//jtvIipJg8y" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 8, 5, 56, 58, 294, DateTimeKind.Utc).AddTicks(7549), "$2a$11$bnfN.OzdKD71fJvq4Lytg.AqNc94FruXLsKbVG/Mvjy6/T7wkF.5S" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentOptions_StudentQuizStudentId_StudentQuizQuizId",
                table: "StudentOptions",
                columns: new[] { "StudentQuizStudentId", "StudentQuizQuizId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentOptions_StudentQuizzes_StudentQuizStudentId_StudentQuizQuizId",
                table: "StudentOptions",
                columns: new[] { "StudentQuizStudentId", "StudentQuizQuizId" },
                principalTable: "StudentQuizzes",
                principalColumns: new[] { "StudentId", "QuizId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
