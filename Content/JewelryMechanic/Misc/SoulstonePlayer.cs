using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc;

internal class SoulstonePlayer : ModPlayer
{
    public readonly Dictionary<StatType, float> StrengthPerSoulstone = new() { { StatType.SoulAgony, 0 }, { StatType.SoulGrief, 0 }, { StatType.SoulBetrayal, 0 },
        { StatType.SoulPlague, 0 }, { StatType.SoulTorture, 0 }, { StatType.SoulSacrifice, 0 } };

    public override void ResetEffects()
    {
        foreach (var key in StrengthPerSoulstone.Keys)
            StrengthPerSoulstone[key] = 0;
    }

    public override void PostUpdateEquips()
    {
        foreach (var key in StrengthPerSoulstone.Keys)
        {

        }
    }
}
