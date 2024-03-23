using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseTarget",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Syllabus",
                newName: "Status");

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

            migrationBuilder.InsertData(
                table: "PassCondition",
                columns: new[] { "Id", "PassRatio" },
                values: new object[,]
                {
                    { 1, 60 },
                    { 2, 70 },
                    { 3, 80 },
                    { 4, 90 },
                    { 5, 100 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PassCondition",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PassCondition",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PassCondition",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PassCondition",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PassCondition",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Syllabus",
                newName: "status");

            migrationBuilder.AddColumn<string>(
                name: "CourseTarget",
                table: "Courses",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 22, 14, 16, 59, 119, DateTimeKind.Utc).AddTicks(5667), "$2a$11$2PayLXtOg3PVcryKYeZivOwQTLS/WXswbPKl4QIE60IPCficU5RSi" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 22, 14, 16, 59, 365, DateTimeKind.Utc).AddTicks(7887), "$2a$11$iz3dN59ymsl9ai6KyQvQPeD1ONxRoACXT3BmSK7.O8URSzugbwWB." });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 22, 14, 16, 59, 639, DateTimeKind.Utc).AddTicks(3816), "$2a$11$r3YcIw0awx2Z5NVeGTKUS.Alp23zBldxOYGhOIgp4EH7JaZYKhLTi" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 22, 14, 16, 59, 847, DateTimeKind.Utc).AddTicks(2784), "$2a$11$zOhD8PFSq0FPzLJBGyctveUK3LP9x7pj813qopu1QMxF.UN8/cVzy" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 22, 14, 17, 0, 61, DateTimeKind.Utc).AddTicks(7829), "$2a$11$o6FFpFsrdMsUhJr61JMw/.Hay.pwmmN02K9oUbpbWs5lVlvD2vmmG" });
        }
    }
}
