using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

internal class SpectroliteProjPlayer : ModPlayer
{
    public Dictionary<StatType, int> ProjCountByStatType = new()
    {
        { StatType.SpectroliteKitten, 0 },
        { StatType.SpectroliteCactus, 0 },
        { StatType.SpectroliteFairy, 0 },
        { StatType.SpectroliteBeetle, 0 },
        { StatType.SpectroliteCreeper, 0 },
        { StatType.SpectroliteSlime, 0 },
    };

    public override void ResetEffects()
    {
        foreach (StatType key in ProjCountByStatType.Keys)
            ProjCountByStatType[key] = 0;
    }
}
