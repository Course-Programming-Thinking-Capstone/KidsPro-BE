using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetailStudent_OrderDetails_OrderDetailsId",
                table: "OrderDetailStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetailStudent_Students_StudentsId",
                table: "OrderDetailStudent");

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentType = table.Column<byte>(type: "tinyint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(11,2)", precision: 11, scale: 2, nullable: false),
                    TransactionCode = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 15, 14, 48, 43, 166, DateTimeKind.Utc).AddTicks(3527), "$2a$11$miFDucadUxOErDcXHzQC1u/ftYgLqOJZh8MDSwgie01qF4rBrP9oq" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 15, 14, 48, 43, 384, DateTimeKind.Utc).AddTicks(1270), "$2a$11$s/Oha8p0Rl5dIDAZuRSr8ueRAqyZZPZE/mfB.aWMo082DOB6e2hFu" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 15, 14, 48, 43, 597, DateTimeKind.Utc).AddTicks(1445), "$2a$11$ihXJeV5f81KMrXXCKhNqJ.gi./qIq4zbYo4/NxKcvzD1P6.wSJcv6" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 15, 14, 48, 43, 819, DateTimeKind.Utc).AddTicks(8602), "$2a$11$ZosnPUTVZbqcNRiCnIHWXe6L5HoWmLpARSECaVQ8WbwB.cyoKSCQa" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 15, 14, 48, 44, 50, DateTimeKind.Utc).AddTicks(6630), "$2a$11$CtsBkz55lYP5cwHPoQDB7u4.xIzateKgpIPxzTcgHSbC7U.cWI6i." });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_OrderCode",
                table: "Transaction",
                column: "OrderCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_OrderId",
                table: "Transaction",
                column: "OrderId",
                unique: true,
                filter: "[OrderId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetailStudent_OrderDetails_OrderDetailsId",
                table: "OrderDetailStudent",
                column: "OrderDetailsId",
                principalTable: "OrderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetailStudent_Students_StudentsId",
                table: "OrderDetailStudent",
                column: "StudentsId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetailStudent_OrderDetails_OrderDetailsId",
                table: "OrderDetailStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetailStudent_Students_StudentsId",
                table: "OrderDetailStudent");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 14, 15, 12, 5, 191, DateTimeKind.Utc).AddTicks(6185), "$2a$11$CjspdyErdQ1cSyMwGF6AMeStlyE2GZlZ6W.fTLiTYxWT0HKql19c2" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 14, 15, 12, 5, 523, DateTimeKind.Utc).AddTicks(1324), "$2a$11$hiM0/B471yMQfCMG9cXH2OjtspQ/x/YRY131511/ECQvoqsVlIV6." });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 14, 15, 12, 5, 813, DateTimeKind.Utc).AddTicks(8629), "$2a$11$3rrUgIRl3aciCRtXllhXSO5zteRadS9XfqQNd/DMQLbSxk1HU1DlC" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 14, 15, 12, 6, 111, DateTimeKind.Utc).AddTicks(9642), "$2a$11$rFtXmsAhwabCqA1mfqYiZ.x9rPvXowv2BT5h2is82Nkbn3wsUNrDy" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 3, 14, 15, 12, 6, 403, DateTimeKind.Utc).AddTicks(8352), "$2a$11$SuJ5w4u7IlG4imQAbxeD4eDuQNkFoPXSOOZcHKkrPpPpcYvQ2p8Iu" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetailStudent_OrderDetails_OrderDetailsId",
                table: "OrderDetailStudent",
                column: "OrderDetailsId",
                principalTable: "OrderDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetailStudent_Students_StudentsId",
                table: "OrderDetailStudent",
                column: "StudentsId",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}
