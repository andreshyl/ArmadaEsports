using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArmadaEsports.Data.Migrations;

/// <inheritdoc />
public partial class MakeJerseyNumberNullable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Drop the unique index before altering the column
        migrationBuilder.DropIndex(
            name: "IX_Players_JerseyNumber",
            table: "Players");

        // Make the column nullable
        migrationBuilder.AlterColumn<int>(
            name: "JerseyNumber",
            table: "Players",
            type: "int",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int");

        // Recreate index as unique but filtered — nulls are excluded from uniqueness
        migrationBuilder.Sql(
            "CREATE UNIQUE INDEX IX_Players_JerseyNumber ON Players (JerseyNumber) WHERE JerseyNumber IS NOT NULL;");
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
            oldNullable: true,
            oldType: "int");

        migrationBuilder.CreateIndex(
            name: "IX_Players_JerseyNumber",
            table: "Players",
            column: "JerseyNumber",
            unique: true);
    }
}
