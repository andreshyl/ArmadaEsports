using ArmadaEsports.Core.Enums;

namespace ArmadaEsports.Core.Models;

public class AiMatchParseJob
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public int MatchId { get; set; }
    public string ImageBase64 { get; set; } = string.Empty;
    public string ImageMimeType { get; set; } = string.Empty;
    public string? RawOutput { get; set; }
    public EAiParseStatus ParseStatus { get; set; } = EAiParseStatus.Pending;
    public string? ErrorMessage { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ConfirmedAt { get; set; }

    public Match Match { get; set; } = null!;
    public ICollection<AiParseJobRow> Rows { get; set; } = [];
}
