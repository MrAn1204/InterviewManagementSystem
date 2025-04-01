using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedOfferDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfferDepartments");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Offers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_DepartmentId",
                table: "Offers",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Departments_DepartmentId",
                table: "Offers",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Departments_DepartmentId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_DepartmentId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Offers");

            migrationBuilder.CreateTable(
                name: "OfferDepartments",
                columns: table => new
                {
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferDepartments", x => new { x.OfferId, x.DepartmentId });
                    table.ForeignKey(
                        name: "FK_OfferDepartments_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferDepartments_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfferDepartments_DepartmentId",
                table: "OfferDepartments",
                column: "DepartmentId");
        }
    }
}
