using ArmadaEsports.Core.Enums;

namespace ArmadaEsports.Core.Models;

public class Position
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string NameEs { get; set; } = string.Empty;
    public EPositionGroup PositionGroup { get; set; }
    public int SortOrder { get; set; }

    public ICollection<Player> PrimaryPlayers { get; set; } = [];
    public ICollection<Player> SecondaryPlayers { get; set; } = [];
    public ICollection<Player> TertiaryPlayers { get; set; } = [];
}
