using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLevelForCandidate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "Candidates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_LevelId",
                table: "Candidates",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_Levels_LevelId",
                table: "Candidates",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_Levels_LevelId",
                table: "Candidates");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_LevelId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "Candidates");
        }
    }
}
