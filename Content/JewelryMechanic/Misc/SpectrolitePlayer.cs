using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc;

internal class SpectrolitePlayer : ModPlayer
{
    public class ValuePair(int Stacks = 0, float Value = 0)
    {
        public int Stacks = Stacks;
        public float Value = Value;
    }

    internal Dictionary<StatType, ValuePair> Stats = new()
    {
        { StatType.SpectroliteFairy, new() }, { StatType.SpectroliteBeetle, new() }, { StatType.SpectroliteCactus, new() }, { StatType.SpectroliteSlime, new() }, 
        { StatType.SpectroliteKitten, new() }, { StatType.SpectroliteCreeper, new() }
    };

    public override void ResetEffects()
    {
        foreach (var key in Stats.Keys)
            Stats[key] = new();
    }
}
