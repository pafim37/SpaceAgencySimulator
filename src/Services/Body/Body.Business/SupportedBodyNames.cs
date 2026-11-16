namespace Sas.Body.Business;

internal static class SupportedBodyNames
{
    public static IReadOnlyList<string> All { get; } = Array.AsReadOnly<string>(
        ["Sun", "Earth", "Moon"]);
}
