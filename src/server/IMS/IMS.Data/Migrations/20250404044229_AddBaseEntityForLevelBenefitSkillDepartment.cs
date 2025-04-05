using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseEntityForLevelBenefitSkillDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Skills",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Skills",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ResetPasswordTokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "ResetPasswordTokens",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ResetPasswordTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "RefreshTokens",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Levels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Levels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Levels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Departments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Departments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Departments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Benefits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Benefits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Benefits",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ResetPasswordTokens");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "ResetPasswordTokens");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ResetPasswordTokens");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Benefits");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Benefits");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Benefits");
        }
    }
}
