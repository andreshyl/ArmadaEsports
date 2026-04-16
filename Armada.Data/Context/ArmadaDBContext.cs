using ArmadaEsports.Core.Enums;
using ArmadaEsports.Core.Models;
using ArmadaEsports.Core.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace ArmadaEsports.Data.Context;

public class ArmadaDbContext : DbContext
{
    public ArmadaDbContext(DbContextOptions<ArmadaDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<PlayerAttributeSnapshot> PlayerAttributeSnapshots => Set<PlayerAttributeSnapshot>();
    public DbSet<PlayerAttributeScore> PlayerAttributeScores => Set<PlayerAttributeScore>();
    public DbSet<TrainingAttendance> TrainingAttendances => Set<TrainingAttendance>();
    public DbSet<Competition> Competitions => Set<Competition>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<MatchPerformanceStat> MatchPerformanceStats => Set<MatchPerformanceStat>();
    public DbSet<AiMatchParseJob> AiMatchParseJobs => Set<AiMatchParseJob>();
    public DbSet<AiParseJobRow> AiParseJobRows => Set<AiParseJobRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Position>(e =>
        {
            e.ToTable("Positions");
            e.HasKey(x => x.Id);
            e.Property(x => x.Code).HasMaxLength(10).IsRequired();
            e.Property(x => x.NameEs).HasMaxLength(60).IsRequired();
            e.Property(x => x.PositionGroup).HasConversion<string>().HasMaxLength(20);
            e.HasIndex(x => x.Code).IsUnique();
            e.HasMany(x => x.PrimaryPlayers).WithOne(x => x.PrimaryPosition).HasForeignKey(x => x.PrimaryPositionId).OnDelete(DeleteBehavior.Restrict);
            e.HasMany(x => x.SecondaryPlayers).WithOne(x => x.SecondaryPosition!).HasForeignKey(x => x.SecondaryPositionId).OnDelete(DeleteBehavior.Restrict);
            e.HasMany(x => x.TertiaryPlayers).WithOne(x => x.TertiaryPosition!).HasForeignKey(x => x.TertiaryPositionId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Player>(e =>
        {
            e.ToTable("Players");
            e.Property(x => x.Alias).HasMaxLength(60).IsRequired();
            e.HasIndex(x => x.Alias).IsUnique();
            e.HasIndex(x => x.JerseyNumber).IsUnique();
        });

        modelBuilder.Entity<PlayerAttributeSnapshot>(e =>
        {
            e.ToTable("PlayerAttributeSnapshots");
            e.Property(x => x.OverallRating).HasPrecision(5, 2);
            e.HasIndex(x => new { x.PlayerId, x.SnapshotDate }).IsUnique();
            e.HasOne(x => x.Player).WithMany(x => x.AttributeSnapshots).HasForeignKey(x => x.PlayerId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PlayerAttributeScore>(e =>
        {
            e.ToTable("PlayerAttributeScores");
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.SnapshotId, x.AttributeIndex }).IsUnique();
            e.HasOne(x => x.Snapshot).WithMany(x => x.Scores).HasForeignKey(x => x.SnapshotId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TrainingAttendance>(e =>
        {
            e.ToTable("TrainingAttendances");
            e.HasIndex(x => new { x.PlayerId, x.SessionDate }).IsUnique();
            e.HasOne(x => x.Player).WithMany()
                .HasForeignKey(x => x.PlayerId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Competition>(e =>
        {
            e.ToTable("Competitions");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(120).IsRequired();
            e.Property(x => x.CompetitionType).HasConversion<string>().HasMaxLength(20);
        });

        modelBuilder.Entity<Match>(e =>
        {
            e.ToTable("Matches");
            e.Property(x => x.OpponentName).HasMaxLength(120).IsRequired();
            e.Property(x => x.MatchdayLabel).HasMaxLength(40);
            e.Property(x => x.Venue).HasConversion<string>().HasMaxLength(10);
            e.Property(x => x.Result).HasConversion<string>().HasMaxLength(1);
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(10);
            e.HasOne(x => x.Competition).WithMany(x => x.Matches).HasForeignKey(x => x.CompetitionId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<MatchPerformanceStat>(e =>
        {
            e.ToTable("MatchPerformanceStats");
            e.HasIndex(x => new { x.MatchId, x.PlayerId }).IsUnique();
            e.Property(x => x.ShotAccuracyPct).HasPrecision(5, 2);
            e.Property(x => x.PassAccuracyPct).HasPrecision(5, 2);
            e.Property(x => x.MatchRating).HasPrecision(4, 2);
            e.Property(x => x.AiConfidenceScore).HasPrecision(4, 3);
            e.HasOne(x => x.Match).WithMany(x => x.PerformanceStats).HasForeignKey(x => x.MatchId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Player).WithMany(x => x.PerformanceStats).HasForeignKey(x => x.PlayerId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AiMatchParseJob>(e =>
        {
            e.ToTable("AiMatchParseJobs");
            e.HasKey(x => x.Id);
            e.Property(x => x.ParseStatus).HasConversion<string>().HasMaxLength(15);
            e.Property(x => x.ImageMimeType).HasMaxLength(20);
            e.HasOne(x => x.Match).WithMany(x => x.ParseJobs).HasForeignKey(x => x.MatchId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AiParseJobRow>(e =>
        {
            e.ToTable("AiParseJobRows");
            e.HasKey(x => x.Id);
            e.Property(x => x.RawAlias).HasMaxLength(60);
            e.Property(x => x.ShotAccuracyPct).HasPrecision(5, 2);
            e.Property(x => x.PassAccuracyPct).HasPrecision(5, 2);
            e.Property(x => x.MatchRating).HasPrecision(4, 2);
            e.Property(x => x.AiConfidenceScore).HasPrecision(4, 3);
            e.HasOne(x => x.Job).WithMany(x => x.Rows).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Player).WithMany().HasForeignKey(x => x.PlayerId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Position>().HasData(
            new Position { Id = 1, Code = "PO", NameEs = "Portero", PositionGroup = EPositionGroup.Goalkeeper, SortOrder = 1 },
            new Position { Id = 2, Code = "DFD", NameEs = "Defensa Central Derecho", PositionGroup = EPositionGroup.Defender, SortOrder = 2 },
            new Position { Id = 3, Code = "DFI", NameEs = "Defensa Central Izquierdo", PositionGroup = EPositionGroup.Defender, SortOrder = 3 },
            new Position { Id = 4, Code = "LD", NameEs = "Lateral Derecho", PositionGroup = EPositionGroup.Defender, SortOrder = 4 },
            new Position { Id = 5, Code = "LI", NameEs = "Lateral Izquierdo", PositionGroup = EPositionGroup.Defender, SortOrder = 5 },
            new Position { Id = 6, Code = "MCD", NameEs = "Mediocentro Defensivo", PositionGroup = EPositionGroup.Midfielder, SortOrder = 6 },
            new Position { Id = 7, Code = "MVD", NameEs = "Mediocentro Derecho", PositionGroup = EPositionGroup.Midfielder, SortOrder = 7 },
            new Position { Id = 8, Code = "MI", NameEs = "Extremo Izq / MVI", PositionGroup = EPositionGroup.Midfielder, SortOrder = 8 },
            new Position { Id = 9, Code = "MD", NameEs = "Extremo Der / SDD", PositionGroup = EPositionGroup.Forward, SortOrder = 9 },
            new Position { Id = 10, Code = "DC", NameEs = "Delantero Centro", PositionGroup = EPositionGroup.Forward, SortOrder = 10 },
            new Position { Id = 11, Code = "DCI", NameEs = "Del. Centro Izq / SDI", PositionGroup = EPositionGroup.Forward, SortOrder = 11 }
        );
    }

    public override int SaveChanges()
    {
        SetUpdatedAt();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetUpdatedAt();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetUpdatedAt()
    {
        foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>()
            .Where(e => e.State == EntityState.Modified))
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
