using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sections_Order_CourseId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_Order_SectionId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "GameReward",
                table: "GameLevels",
                newName: "GemReward");

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "Quizzes",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ResourceUrl",
                table: "Lessons",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "Lessons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_LessonId",
                table: "Quizzes",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_LessonId",
                table: "Lessons",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Lessons_LessonId",
                table: "Lessons",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Lessons_LessonId",
                table: "Quizzes",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Lessons_LessonId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Lessons_LessonId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_LessonId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_LessonId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "GemReward",
                table: "GameLevels",
                newName: "GameReward");

            migrationBuilder.AlterColumn<string>(
                name: "ResourceUrl",
                table: "Lessons",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "Lessons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_Order_CourseId",
                table: "Sections",
                columns: new[] { "Order", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_Order_SectionId",
                table: "Lessons",
                columns: new[] { "Order", "SectionId" },
                unique: true);
        }
    }
}
