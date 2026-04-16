namespace ArmadaEsports.Core.Models;

public class LineupSnapshot
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public string Name { get; set; } = string.Empty;
    public string Formation { get; set; } = "4-4-2";
    public string Date { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    public string Notes { get; set; } = string.Empty;

    // SlotId -> PlayerId (int, matching Player.Id)
    public Dictionary<string, int> Lineup { get; set; } = [];
    public List<int> Bench { get; set; } = [];
}
