using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInterview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Interviews_InterviewId",
                table: "Offers");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Interviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "Interviews",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<DateOnly>(
                name: "InterviewDate",
                table: "Interviews",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "RecruiterId",
                table: "Interviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Result",
                table: "Interviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "Interviews",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.CreateTable(
                name: "InterviewInterviewers",
                columns: table => new
                {
                    InterviewId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewInterviewers", x => new { x.InterviewId, x.UserId });
                    table.ForeignKey(
                        name: "FK_InterviewInterviewers_Interviews_InterviewId",
                        column: x => x.InterviewId,
                        principalTable: "Interviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterviewInterviewers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_RecruiterId",
                table: "Interviews",
                column: "RecruiterId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewInterviewers_UserId",
                table: "InterviewInterviewers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_Users_RecruiterId",
                table: "Interviews",
                column: "RecruiterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Interviews_InterviewId",
                table: "Offers",
                column: "InterviewId",
                principalTable: "Interviews",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_Users_RecruiterId",
                table: "Interviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Interviews_InterviewId",
                table: "Offers");

            migrationBuilder.DropTable(
                name: "InterviewInterviewers");

            migrationBuilder.DropIndex(
                name: "IX_Interviews_RecruiterId",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "InterviewDate",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "RecruiterId",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Interviews");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Interviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Interviews_InterviewId",
                table: "Offers",
                column: "InterviewId",
                principalTable: "Interviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
