using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armada.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchOpponentStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "PassAccuracyPctOpp",
                table: "Matches",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "PassesAttemptedOpp",
                table: "Matches",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "PossessionPctOpp",
                table: "Matches",
                type: "tinyint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassAccuracyPctOpp",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "PassesAttemptedOpp",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "PossessionPctOpp",
                table: "Matches");
        }
    }
}
