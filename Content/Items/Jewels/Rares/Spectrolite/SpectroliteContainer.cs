using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;

namespace PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;

internal class SpectroliteContainer(StatType category, bool major, Guid guid) : JewelStat(category)
{
    public static bool? CurrentlyMajor = null;
    public static Guid? CurrentGuid = null;

    public bool IsMajor = major;
    public Guid SelfGuid = guid;

    public override void Apply(Player player, float add = 0, float multiplier = 0)
    {
        CurrentlyMajor = IsMajor;
        CurrentGuid = SelfGuid;
        base.Apply(player, add, multiplier);
        CurrentGuid = null;
        CurrentlyMajor = null;
    }
}
