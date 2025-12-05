using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc;

internal class SpectrolitePlayer : ModPlayer
{
    internal Dictionary<StatType, float> Stats = new()
    {
        { StatType.SpectroliteFairy, 0 }, { StatType.SpectroliteBeetle, 0 }, { StatType.SpectroliteCactus, 0 }, { StatType.SpectroliteSlime, 0 }, { StatType.SpectroliteKitten, 0 },
        { StatType.SpectroliteCreeper, 0 }
    };

    public override void ResetEffects()
    {
        foreach (var key in Stats.Keys)
            Stats[key] = 0;
    }
}
