using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseSlot",
                table: "Syllabus",
                newName: "TotalSlot");

            migrationBuilder.AlterColumn<string>(
                name: "Target",
                table: "Syllabus",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Syllabus",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Courses",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConfirmAccount",
                table: "Accounts",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmAccount", "CreatedDate", "PasswordHash" },
                values: new object[] { null, new DateTime(2024, 3, 23, 15, 14, 25, 284, DateTimeKind.Utc).AddTicks(5728), "$2a$11$pgVAdft7yPxz5kZeXHFMJO.bi5wuaPp9n47ePKBA8kSBXQ4HkUgrG" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConfirmAccount", "CreatedDate", "PasswordHash" },
                values: new object[] { null, new DateTime(2024, 3, 23, 15, 14, 25, 519, DateTimeKind.Utc).AddTicks(6229), "$2a$11$yTR8lrtlUrqBoWxqKfjpNur79zC8jM3o1XIUArPuFdNmouCWK9qOe" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConfirmAccount", "CreatedDate", "PasswordHash" },
                values: new object[] { null, new DateTime(2024, 3, 23, 15, 14, 25, 748, DateTimeKind.Utc).AddTicks(698), "$2a$11$SXpjG7x/b/lKmTtX3Cb0aOCUtMLImzSzvCB2zn9qtIWbwC4OkxmDK" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConfirmAccount", "CreatedDate", "PasswordHash" },
                values: new object[] { null, new DateTime(2024, 3, 23, 15, 14, 25, 982, DateTimeKind.Utc).AddTicks(7169), "$2a$11$afyyZP0qviysonUcAeEdgOntF1T4SwYMH7fa6uLog14n3XHKYqtY." });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConfirmAccount", "CreatedDate", "PasswordHash" },
                values: new object[] { null, new DateTime(2024, 3, 23, 15, 14, 26, 210, DateTimeKind.Utc).AddTicks(3526), "$2a$11$rN78sP.Xoy4hp6p.gOc/LO//8GZ/gtEmUwpaYtHK9elJjI5wErTdm" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Syllabus");

            migrationBuilder.DropColumn(
                name: "ConfirmAccount",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "TotalSlot",
                table: "Syllabus",
                newName: "CourseSlot");

            migrationBuilder.AlterColumn<string>(
                name: "Target",
                table: "Syllabus",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Courses",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 1, 27, 35, 698, DateTimeKind.Utc).AddTicks(4034), "$2a$11$mDjLuP2tsX/H8Evc488KzuZaQJ99GTt8w2GnI3oOI0j5bZSbUadS2" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 1, 27, 35, 913, DateTimeKind.Utc).AddTicks(6276), "$2a$11$t5p/lGSC6eN14zwBhvm.Ue5rsZ2gdDRExn0v.hFVLUzx8AV8eFJ1e" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 1, 27, 36, 135, DateTimeKind.Utc).AddTicks(8496), "$2a$11$NOvoLf88r8.3Ln98ZTonyOhAO7TfM3tE3UjqmuRcpWOPo8DfKqoz." });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 1, 27, 36, 350, DateTimeKind.Utc).AddTicks(638), "$2a$11$MKdqp/GM/vEDGQfNBv1FmOghtGdX8T5umcIEeL/vk5O/gs0zmdNJu" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 1, 27, 36, 574, DateTimeKind.Utc).AddTicks(928), "$2a$11$tXzQ6libSyzfjf56Qjk2keesb/pzdFrXCR59gVJX/zHOIiEZcvgr." });
        }
    }
}
