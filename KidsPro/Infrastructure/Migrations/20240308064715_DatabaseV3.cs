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
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Students_StudentId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Courses_CourseId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Vouchers_VoucherId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "GameStudentQuizzes");

            migrationBuilder.DropTable(
                name: "StudentAnswers");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "GameQuizRooms");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CourseId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_StudentId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FromAge",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "GraduateCondition",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "PreRequire",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ToAge",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "OrderDetails",
                newName: "Quantity");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OrderDetails",
                type: "decimal(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "OrderDetails",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "GameVouchers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConvertedPoint = table.Column<int>(type: "int", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(11,2)", precision: 11, scale: 2, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameVouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameVouchers_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MiniGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    JoinCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GameLevelId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiniGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiniGames_GameLevels_GameLevelId",
                        column: x => x.GameLevelId,
                        principalTable: "GameLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MiniGames_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: false),
                    AnswerExplain = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Options_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetailStudent",
                columns: table => new
                {
                    OrderDetailsId = table.Column<int>(type: "int", nullable: false),
                    StudentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetailStudent", x => new { x.OrderDetailsId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_OrderDetailStudent_OrderDetails_OrderDetailsId",
                        column: x => x.OrderDetailsId,
                        principalTable: "OrderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetailStudent_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    OptionId = table.Column<int>(type: "int", nullable: false),
                    QuestionOrder = table.Column<int>(type: "int", nullable: false),
                    StudentQuizStudentId = table.Column<int>(type: "int", nullable: false),
                    StudentQuizQuizId = table.Column<int>(type: "int", nullable: false),
                    StudentQuizId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentOptions_StudentQuizzes_StudentQuizStudentId_StudentQuizQuizId",
                        columns: x => new { x.StudentQuizStudentId, x.StudentQuizQuizId },
                        principalTable: "StudentQuizzes",
                        principalColumns: new[] { "StudentId", "QuizId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentMiniGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JoinTime = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: false),
                    FinishTime = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: false),
                    StepCount = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    MiniGameId = table.Column<int>(type: "int", nullable: false),
                    GameQuizRoomId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentMiniGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentMiniGames_MiniGames_MiniGameId",
                        column: x => x.MiniGameId,
                        principalTable: "MiniGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentMiniGames_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_CourseId",
                table: "OrderDetails",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_GameVouchers_ParentId",
                table: "GameVouchers",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_MiniGames_GameLevelId",
                table: "MiniGames",
                column: "GameLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_MiniGames_TeacherId",
                table: "MiniGames",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Options_Order_QuestionId",
                table: "Options",
                columns: new[] { "Order", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Options_QuestionId",
                table: "Options",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetailStudent_StudentsId",
                table: "OrderDetailStudent",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentMiniGames_MiniGameId",
                table: "StudentMiniGames",
                column: "MiniGameId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentMiniGames_StudentId",
                table: "StudentMiniGames",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentOptions_StudentQuizStudentId_StudentQuizQuizId",
                table: "StudentOptions",
                columns: new[] { "StudentQuizStudentId", "StudentQuizQuizId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Courses_CourseId",
                table: "OrderDetails",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_GameVouchers_VoucherId",
                table: "Orders",
                column: "VoucherId",
                principalTable: "GameVouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Courses_CourseId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_GameVouchers_VoucherId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "GameVouchers");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropTable(
                name: "OrderDetailStudent");

            migrationBuilder.DropTable(
                name: "StudentMiniGames");

            migrationBuilder.DropTable(
                name: "StudentOptions");

            migrationBuilder.DropTable(
                name: "MiniGames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_CourseId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderDetails",
                newName: "StudentId");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Orders",
                type: "decimal(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<byte>(
                name: "FromAge",
                table: "Courses",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GraduateCondition",
                table: "Courses",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Courses",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreRequire",
                table: "Courses",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "ToAge",
                table: "Courses",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                columns: new[] { "OrderId", "StudentId" });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    AnswerExplain = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameQuizRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameLevelId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    JoinCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameQuizRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameQuizRooms_GameLevels_GameLevelId",
                        column: x => x.GameLevelId,
                        principalTable: "GameLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameQuizRooms_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentQuizStudentId = table.Column<int>(type: "int", nullable: false),
                    StudentQuizQuizId = table.Column<int>(type: "int", nullable: false),
                    AnswerId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    QuestionOrder = table.Column<int>(type: "int", nullable: false),
                    StudentQuizId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentAnswers_StudentQuizzes_StudentQuizStudentId_StudentQuizQuizId",
                        columns: x => new { x.StudentQuizStudentId, x.StudentQuizQuizId },
                        principalTable: "StudentQuizzes",
                        principalColumns: new[] { "StudentId", "QuizId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(11,2)", precision: 11, scale: 2, nullable: false),
                    DiscountType = table.Column<byte>(type: "tinyint", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: true),
                    NumberOfUsed = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    UsageLimit = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    VoucherCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vouchers_Accounts_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameStudentQuizzes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameQuizRoomId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    FinishTime = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: false),
                    JoinTime = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: false),
                    StepCount = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStudentQuizzes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameStudentQuizzes_GameQuizRooms_GameQuizRoomId",
                        column: x => x.GameQuizRoomId,
                        principalTable: "GameQuizRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameStudentQuizzes_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CourseId",
                table: "Orders",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_StudentId",
                table: "OrderDetails",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_Order_QuestionId",
                table: "Answers",
                columns: new[] { "Order", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_GameQuizRooms_GameLevelId",
                table: "GameQuizRooms",
                column: "GameLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_GameQuizRooms_TeacherId",
                table: "GameQuizRooms",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_GameStudentQuizzes_GameQuizRoomId",
                table: "GameStudentQuizzes",
                column: "GameQuizRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_GameStudentQuizzes_StudentId",
                table: "GameStudentQuizzes",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_StudentQuizStudentId_StudentQuizQuizId",
                table: "StudentAnswers",
                columns: new[] { "StudentQuizStudentId", "StudentQuizQuizId" });

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_CreatedById",
                table: "Vouchers",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Students_StudentId",
                table: "OrderDetails",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Courses_CourseId",
                table: "Orders",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Vouchers_VoucherId",
                table: "Orders",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
