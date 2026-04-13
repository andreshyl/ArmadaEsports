using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armada.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AiConfidenceScore",
                table: "MatchPerformanceStats",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RawOutput",
                table: "AiMatchParseJobs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AiConfidenceScore",
                table: "MatchPerformanceStats");

            migrationBuilder.DropColumn(
                name: "RawOutput",
                table: "AiMatchParseJobs");
        }
    }
}
