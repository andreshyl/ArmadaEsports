using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armada.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixPendingModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_JerseyNumber",
                table: "Players");

            migrationBuilder.AlterColumn<int>(
                name: "JerseyNumber",
                table: "Players",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Players_JerseyNumber",
                table: "Players",
                column: "JerseyNumber",
                unique: true,
                filter: "[JerseyNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_JerseyNumber",
                table: "Players");

            migrationBuilder.AlterColumn<int>(
                name: "JerseyNumber",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_JerseyNumber",
                table: "Players",
                column: "JerseyNumber",
                unique: true);
        }
    }
}
