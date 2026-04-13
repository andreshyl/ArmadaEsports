using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Armada.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    CompetitionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NameEs = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    PositionGroup = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompetitionId = table.Column<int>(type: "int", nullable: false),
                    MatchDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Matchday = table.Column<short>(type: "smallint", nullable: true),
                    MatchdayLabel = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    OpponentName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Venue = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GoalsFor = table.Column<byte>(type: "tinyint", nullable: true),
                    GoalsAgainst = table.Column<byte>(type: "tinyint", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Alias = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    JerseyNumber = table.Column<int>(type: "int", nullable: false),
                    PrimaryPositionId = table.Column<int>(type: "int", nullable: false),
                    SecondaryPositionId = table.Column<int>(type: "int", nullable: true),
                    TertiaryPositionId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    JoinedAt = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Positions_PrimaryPositionId",
                        column: x => x.PrimaryPositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Players_Positions_SecondaryPositionId",
                        column: x => x.SecondaryPositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Players_Positions_TertiaryPositionId",
                        column: x => x.TertiaryPositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AiMatchParseJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    ImageBase64 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageMimeType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ParseStatus = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiMatchParseJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AiMatchParseJobs_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchPerformanceStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    Goals = table.Column<byte>(type: "tinyint", nullable: false),
                    ShotAccuracyPct = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Assists = table.Column<byte>(type: "tinyint", nullable: false),
                    PassAccuracyPct = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Tackles = table.Column<byte>(type: "tinyint", nullable: false),
                    TacklesWon = table.Column<byte>(type: "tinyint", nullable: false),
                    Interceptions = table.Column<byte>(type: "tinyint", nullable: false),
                    Saves = table.Column<byte>(type: "tinyint", nullable: false),
                    MatchRating = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: true),
                    IsAiParsed = table.Column<bool>(type: "bit", nullable: false),
                    ConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchPerformanceStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchPerformanceStats_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchPerformanceStats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerAttributeSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    SnapshotDate = table.Column<DateOnly>(type: "date", nullable: false),
                    OverallRating = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerAttributeSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerAttributeSnapshots_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AiParseJobRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    RawAlias = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    Goals = table.Column<byte>(type: "tinyint", nullable: true),
                    ShotAccuracyPct = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Assists = table.Column<byte>(type: "tinyint", nullable: true),
                    PassAccuracyPct = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Tackles = table.Column<byte>(type: "tinyint", nullable: true),
                    TacklesWon = table.Column<byte>(type: "tinyint", nullable: true),
                    Interceptions = table.Column<byte>(type: "tinyint", nullable: true),
                    Saves = table.Column<byte>(type: "tinyint", nullable: true),
                    MatchRating = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: true),
                    AiConfidenceScore = table.Column<decimal>(type: "decimal(4,3)", precision: 4, scale: 3, nullable: false),
                    IsManuallyEdited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiParseJobRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AiParseJobRows_AiMatchParseJobs_JobId",
                        column: x => x.JobId,
                        principalTable: "AiMatchParseJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AiParseJobRows_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PlayerAttributeScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SnapshotId = table.Column<int>(type: "int", nullable: false),
                    AttributeIndex = table.Column<byte>(type: "tinyint", nullable: false),
                    Score = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerAttributeScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerAttributeScores_PlayerAttributeSnapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "PlayerAttributeSnapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "Id", "Code", "NameEs", "PositionGroup", "SortOrder" },
                values: new object[,]
                {
                    { 1, "PO", "Portero", "Goalkeeper", 1 },
                    { 2, "DFD", "Defensa Central Derecho", "Defender", 2 },
                    { 3, "DFI", "Defensa Central Izquierdo", "Defender", 3 },
                    { 4, "LD", "Lateral Derecho", "Defender", 4 },
                    { 5, "LI", "Lateral Izquierdo", "Defender", 5 },
                    { 6, "MCD", "Mediocentro Defensivo", "Midfielder", 6 },
                    { 7, "MVD", "Mediocentro Derecho", "Midfielder", 7 },
                    { 8, "MI", "Extremo Izq / MVI", "Midfielder", 8 },
                    { 9, "MD", "Extremo Der / SDD", "Forward", 9 },
                    { 10, "DC", "Delantero Centro", "Forward", 10 },
                    { 11, "DCI", "Del. Centro Izq / SDI", "Forward", 11 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AiMatchParseJobs_MatchId",
                table: "AiMatchParseJobs",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_AiParseJobRows_JobId",
                table: "AiParseJobRows",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_AiParseJobRows_PlayerId",
                table: "AiParseJobRows",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_CompetitionId",
                table: "Matches",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPerformanceStats_MatchId_PlayerId",
                table: "MatchPerformanceStats",
                columns: new[] { "MatchId", "PlayerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MatchPerformanceStats_PlayerId",
                table: "MatchPerformanceStats",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerAttributeScores_SnapshotId_AttributeIndex",
                table: "PlayerAttributeScores",
                columns: new[] { "SnapshotId", "AttributeIndex" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerAttributeSnapshots_PlayerId_SnapshotDate",
                table: "PlayerAttributeSnapshots",
                columns: new[] { "PlayerId", "SnapshotDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_Alias",
                table: "Players",
                column: "Alias",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_JerseyNumber",
                table: "Players",
                column: "JerseyNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_PrimaryPositionId",
                table: "Players",
                column: "PrimaryPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_SecondaryPositionId",
                table: "Players",
                column: "SecondaryPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TertiaryPositionId",
                table: "Players",
                column: "TertiaryPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_Code",
                table: "Positions",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AiParseJobRows");

            migrationBuilder.DropTable(
                name: "MatchPerformanceStats");

            migrationBuilder.DropTable(
                name: "PlayerAttributeScores");

            migrationBuilder.DropTable(
                name: "AiMatchParseJobs");

            migrationBuilder.DropTable(
                name: "PlayerAttributeSnapshots");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.DropTable(
                name: "Positions");
        }
    }
}
