using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlDawarat_W_AlEngazat.Migrations {
	/// <inheritdoc />
	public partial class addEtntity : Migration {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder) {
			migrationBuilder.AddColumn<DateTime>(
				name: "StartDate",
				table: "PreviousCourses",
				type: "datetime2",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder) {
			migrationBuilder.DropColumn(
				name: "StartDate",
				table: "PreviousCourses");

		}
	}
}
