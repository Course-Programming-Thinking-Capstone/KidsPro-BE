using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Classes_ClassId",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "OrderDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "GameLevels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "GameItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Classes_ClassId",
                table: "OrderDetails",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Classes_ClassId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "GameLevels");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "GameItems");

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 7, 5, 35, 14, 267, DateTimeKind.Utc).AddTicks(8066), "$2a$11$ACTEiyoB9T22PU6jytLmDOEaaRQeP7bns9RJHMTSNUOQGKk6ycP6y" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 7, 5, 35, 14, 443, DateTimeKind.Utc).AddTicks(8802), "$2a$11$rUlq4ABq2hfkzcI8CgT6ye1bdd9LTHjmhdSleSa4jR5JDMOMRSwqq" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 7, 5, 35, 14, 605, DateTimeKind.Utc).AddTicks(4891), "$2a$11$fiDRGrB9.kpC7Nqle9gl2eu2DHv/hWN57NRxba5oykajG03NjHukO" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 7, 5, 35, 14, 772, DateTimeKind.Utc).AddTicks(4108), "$2a$11$/pgJQtEnr5G7NfBK6AS37.atfyd1rCb0jHoCKS8ewNB0nRnigDb9." });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 4, 7, 5, 35, 14, 963, DateTimeKind.Utc).AddTicks(7351), "$2a$11$VTsJ97HE0d8U.rYmdw7zoOALtj44RSRXM3kQ9Me2oHdEbuP69XOz2" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Classes_ClassId",
                table: "OrderDetails",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
