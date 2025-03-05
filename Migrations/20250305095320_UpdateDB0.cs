using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlDawarat_W_AlEngazat.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Courses_CourseID",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "CourseID",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Courses_CourseID",
                table: "Employees",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Courses_CourseID",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "CourseID",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Courses_CourseID",
                table: "Employees",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
