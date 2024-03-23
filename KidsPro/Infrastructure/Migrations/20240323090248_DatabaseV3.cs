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
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 9, 2, 47, 564, DateTimeKind.Utc).AddTicks(1526), "$2a$11$ZCqGCOYP/EV5rsb8yVof/.5o8vwpw30VIqaHfO5b2y7BdnKXGuStO" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 9, 2, 47, 797, DateTimeKind.Utc).AddTicks(8390), "$2a$11$kD6ejb2DhMzH8nIkFVYKJeIslNVQbARgDx7/sWEF729TEdc8.sW22" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 9, 2, 48, 26, DateTimeKind.Utc).AddTicks(7490), "$2a$11$LnKg/Cjve7f.rfIkRyefy.xMTAPU32Syx9SC.HPmp/vBnt/YVjf5W" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 9, 2, 48, 261, DateTimeKind.Utc).AddTicks(2618), "$2a$11$N6R8v/zTnC6EDcg13/gzwer1qgYIDnb7.v1.5jnC7Mz/D4pcrCD7S" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 9, 2, 48, 490, DateTimeKind.Utc).AddTicks(3024), "$2a$11$4UeUnDi5zexx9raS.mb1H.c3T1RS5NrXL882LZYkwKuwzmDjIKD2a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 5, 16, 55, 124, DateTimeKind.Utc).AddTicks(9398), "$2a$11$axVF32nXT7GmyXy0fSqlOezAnhCd3EaQgNRnU2ICr5qpL6M60/H/q" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 5, 16, 55, 330, DateTimeKind.Utc).AddTicks(3987), "$2a$11$MUY7nWhZkky4OCQuVQ9PleLm1IKjCDXCTrwRP1XKoJWXllsNh6fDO" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 5, 16, 55, 510, DateTimeKind.Utc).AddTicks(2293), "$2a$11$LMpCuFbPw2WzpxLYrW/XK.55bV5JJQ.V5oPI5Z3EVZWrdPRH2/g7u" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 5, 16, 55, 701, DateTimeKind.Utc).AddTicks(5657), "$2a$11$AjY2.QofWS0SDNnhcMG9Cewv5b0BpzgeLSEKRacdCeoBbQv5X9LXq" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 23, 5, 16, 55, 952, DateTimeKind.Utc).AddTicks(93), "$2a$11$GW.6.1g.8YO/ZllAIaibTunrdhjhbiRbUBJ.kxIyEUJLfoEmPMFNK" });
        }
    }
}
