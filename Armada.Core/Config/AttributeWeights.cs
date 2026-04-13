namespace ArmadaEsports.Core.Config;

public static class AttributeWeights
{
    // Attribute order: Comunicacion, JuegoEnEquipo, Posicionamiento, Decisiones,
    //                  Reaccion, Creatividad, Consistencia, Adaptabilidad, Precision, Compostura

    private static readonly decimal[] _po  = [0.10m, 0.05m, 0.15m, 0.10m, 0.20m, 0.05m, 0.18m, 0.05m, 0.09m, 0.03m];
    private static readonly decimal[] _dfc = [0.05m, 0.07m, 0.20m, 0.10m, 0.15m, 0.05m, 0.18m, 0.03m, 0.10m, 0.07m];
    private static readonly decimal[] _lat = [0.08m, 0.13m, 0.10m, 0.18m, 0.10m, 0.17m, 0.08m, 0.05m, 0.08m, 0.03m];
    private static readonly decimal[] _mcd = [0.18m, 0.18m, 0.10m, 0.18m, 0.08m, 0.07m, 0.10m, 0.05m, 0.05m, 0.01m];
    private static readonly decimal[] _mvd = [0.06m, 0.08m, 0.10m, 0.20m, 0.10m, 0.15m, 0.08m, 0.05m, 0.15m, 0.03m];
    private static readonly decimal[] _mi  = [0.07m, 0.13m, 0.10m, 0.15m, 0.10m, 0.20m, 0.08m, 0.05m, 0.09m, 0.03m];
    private static readonly decimal[] _md  = [0.06m, 0.07m, 0.20m, 0.12m, 0.18m, 0.10m, 0.10m, 0.03m, 0.12m, 0.02m];
    private static readonly decimal[] _dci = [0.06m, 0.08m, 0.20m, 0.12m, 0.12m, 0.10m, 0.15m, 0.03m, 0.12m, 0.02m];
    private static readonly decimal[] _dc  = [0.06m, 0.07m, 0.20m, 0.18m, 0.10m, 0.18m, 0.07m, 0.03m, 0.08m, 0.03m];

    public static decimal[] GetWeights(string positionCode) => positionCode switch
    {
        "PO"  => _po,
        "DFD" => _dfc,  // Defensa Central Derecho
        "DFI" => _dfc,  // Defensa Central Izquierdo
        "LD"  => _lat,
        "LI"  => _lat,
        "MCD" => _mcd,  // Mediocentro Defensivo
        "MVD" => _mvd,  // Mediocentro Derecho
        "MI"  => _mi,   // Extremo Izq / MVI
        "MD"  => _md,   // Extremo Der / SDD
        "DC"  => _dc,
        "DCI" => _dci,
        _     => _mcd
    };

    public static decimal ComputeOverallRating(string positionCode, byte[] scores)
    {
        var weights = GetWeights(positionCode);
        decimal total = 0;
        for (int i = 0; i < Math.Min(scores.Length, weights.Length); i++)
            total += scores[i] * weights[i];
        return Math.Round(total * 10, 2);
    }
}
