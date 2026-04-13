namespace ArmadaEsports.Core.Enums;

public enum EPositionGroup
{
    Goalkeeper,
    Defender,
    Midfielder,
    Forward
}

public enum EPositionCode
{
    PO,
    DFD,
    DFI,
    LD,
    LI,
    MCD,
    MVD,
    MI,
    MD,
    DC,
    DCI
}

public enum EMatchResult
{
    W,
    D,
    L
}

public enum EMatchStatus
{
    Upcoming,
    Finished
}

public enum EVenue
{
    Home,
    Away
}

public enum ECompetitionType
{
    League,
    Cup,
    Friendly,
    Tournament
}

public enum EAiParseStatus
{
    Pending,
    Parsed,
    Confirmed,
    Failed
}
