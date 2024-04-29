using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSlotPerWeekInSyllabus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SlotPerWeek",
                table: "Syllabus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 29, 17, 1, 50, 970, DateTimeKind.Utc).AddTicks(5438), "$2a$11$R/31MFj1jDj/6WWZm/6GSuwV1wwXrobiSMvvYwFUzEWb7s/Ptgn56" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 29, 17, 1, 51, 266, DateTimeKind.Utc).AddTicks(7700), "$2a$11$EyKuzj9CKx4y3jbW9x3TJ.TSrPj0py3o2tedD19SCB/Vz2dx9VBIC" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 29, 17, 1, 51, 475, DateTimeKind.Utc).AddTicks(2247), "$2a$11$n2ne5IkrF2jmjQTPJDCrEuTQWnTTew5HNQt7O2Lx71czF/ZTsMABm" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 29, 17, 1, 51, 829, DateTimeKind.Utc).AddTicks(2743), "$2a$11$z4ZU9SFb5x1KVGxXYRnYoug8FiBXNo70aVWqo9O2jXc/E5j7/mtt." });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 29, 17, 1, 52, 150, DateTimeKind.Utc).AddTicks(5873), "$2a$11$qRFkeqRgQevNNz.r1LrsEemi5Xps/5ke7gW5fGvlLhMMHp1LYeV2e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlotPerWeek",
                table: "Syllabus");

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
        }
    }
}
