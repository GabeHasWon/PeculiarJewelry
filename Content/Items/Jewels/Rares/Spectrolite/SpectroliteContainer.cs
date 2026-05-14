using PeculiarJewelry.Content.JewelryMechanic.Stats;

namespace PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;

internal class SpectroliteContainer(StatType category, bool major) : JewelStat(category)
{
    public static bool? CurrentlyMajor = null;

    public bool IsMajor = major;

    public override void Apply(Player player, float add = 0, float multiplier = 0)
    {
        CurrentlyMajor = IsMajor;
        base.Apply(player, add, multiplier);
        CurrentlyMajor = null;
    }
}
