using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateManyToManyRelationCandidateAndSkillModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CandidateId",
                table: "Skills",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CandidateId",
                table: "Skills",
                column: "CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Candidates_CandidateId",
                table: "Skills",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Candidates_CandidateId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_CandidateId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "Skills");
        }
    }
}
