namespace PeculiarJewelry.Content.Items.Jewels;

public readonly struct GrindInfo()
{
    public delegate void ModifySupportChanceDelegate(ref float chance, ref float threshold);

    public bool DropSubstats { get; init; } = true;
    public float DustMultiplier { get; init; } = 1f;
    public float SubstatRatio { get; init; } = 1f;
    public ModifySupportChanceDelegate ModifySupportChance { get; init; } = DefaultSupportChance;

    private static void DefaultSupportChance(ref float chance, ref float threshold) { }
}