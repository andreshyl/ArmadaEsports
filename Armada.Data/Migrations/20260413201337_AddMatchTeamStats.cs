using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Armada.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchTeamStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "PassAccuracyPct",
                table: "Matches",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "PassesAttempted",
                table: "Matches",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "PossessionPct",
                table: "Matches",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "ShotsAgainst",
                table: "Matches",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "ShotsFor",
                table: "Matches",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "TacklesAgainst",
                table: "Matches",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "TacklesFor",
                table: "Matches",
                type: "tinyint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassAccuracyPct",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "PassesAttempted",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "PossessionPct",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "ShotsAgainst",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "ShotsFor",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TacklesAgainst",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TacklesFor",
                table: "Matches");
        }
    }
}
