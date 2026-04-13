namespace ArmadaEsports.Web.Extensions;

public static class AssetExtensions
{
    public static string PlayerImage(string alias) =>
        $"img/players/{alias.ToLower().Replace(" ", "-")}.png";

    public static string TeamLogo(string name = "ttf") =>
        $"img/logo/{name.ToLower()}-logo.svg";

    public static string ImagePath(string path) =>
        $"img/{path}";
}
