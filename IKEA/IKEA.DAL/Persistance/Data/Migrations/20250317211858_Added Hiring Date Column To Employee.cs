using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IKEA.DAL.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedHiringDateColumnToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GiringDate",
                table: "Employees",
                newName: "HiringDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HiringDate",
                table: "Employees",
                newName: "GiringDate");
        }
    }
}
