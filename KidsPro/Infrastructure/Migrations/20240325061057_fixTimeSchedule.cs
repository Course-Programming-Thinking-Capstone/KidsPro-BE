using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixTimeSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "ClassSchedules",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldPrecision: 2);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "ClassSchedules",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldPrecision: 2);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 25, 6, 10, 56, 518, DateTimeKind.Utc).AddTicks(68), "$2a$11$ALQvl2wDbqg72tEG.DgV0uIA4DJkt78jcr7NO8n.cO8bQRI5nZk6." });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 25, 6, 10, 56, 701, DateTimeKind.Utc).AddTicks(3177), "$2a$11$dqmoyLnbRWFADVB4n8mAo.ZU0.R91Mc.Q1GJQNJyfd1fRUmZA6VJq" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 25, 6, 10, 56, 892, DateTimeKind.Utc).AddTicks(6412), "$2a$11$xEbL5WHtCO07GEmYcUkwHulWU7MT0qUINeWd9oLrV0KwvJScogaXq" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 25, 6, 10, 57, 91, DateTimeKind.Utc).AddTicks(8466), "$2a$11$Cz3itB3yRgwNcUiixAtTYesu0/1N9otTJoV3TPVx0bxtwrFvEqQfW" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 25, 6, 10, 57, 260, DateTimeKind.Utc).AddTicks(4345), "$2a$11$PwUDPYCfFh5gALvX7T79r.2whWsLLZeqWT5YbQR8qo3B1ObDTJeCG" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "ClassSchedules",
                type: "datetime2(2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "ClassSchedules",
                type: "datetime2(2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 25, 3, 5, 45, 952, DateTimeKind.Utc).AddTicks(5237), "$2a$11$Y9kO7DrUDO51XIMXg39ECeXSE/ukpm5aZgm6nsLWTPKyhhnp0MqvS" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 25, 3, 5, 46, 187, DateTimeKind.Utc).AddTicks(7994), "$2a$11$uU/zpkziKXhuMgabn9i9KuU264eqHU22A1stcwEsTwhUuKhEZzf/C" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 25, 3, 5, 46, 424, DateTimeKind.Utc).AddTicks(3514), "$2a$11$X6SHsqs.f6vOniNVl3K26O7TtA185NQhcIceHv5eUJxj8Kf6KtTKW" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 25, 3, 5, 46, 637, DateTimeKind.Utc).AddTicks(3984), "$2a$11$YW8WXB9geRpWTp6hYhcQa.29SvbHSWl7LNJfypNtxLn8PhhL2FSiu" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 25, 3, 5, 46, 801, DateTimeKind.Utc).AddTicks(5421), "$2a$11$MW5uZYMZkQJsEiasrdtSM.C.T5JXap820j5rihZqcLETv5vCX.kku" });
        }
    }
}
