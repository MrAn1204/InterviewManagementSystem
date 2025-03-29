using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRecruiterForCandidate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecruiterId",
                table: "Candidates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_RecruiterId",
                table: "Candidates",
                column: "RecruiterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_Users_RecruiterId",
                table: "Candidates",
                column: "RecruiterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_Users_RecruiterId",
                table: "Candidates");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_RecruiterId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "RecruiterId",
                table: "Candidates");
        }
    }
}
