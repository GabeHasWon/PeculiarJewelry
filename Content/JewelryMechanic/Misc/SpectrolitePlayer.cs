using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc;

internal class SpectrolitePlayer : ModPlayer
{
    public class ValuePair(int Stacks = 0, float Value = 0)
    {
        public int Stacks = Stacks;
        public float Value = Value;
        public bool[] IsBig = new bool[12];
    }

    internal Dictionary<StatType, ValuePair> Stats = new()
    {
        { StatType.SpectroliteFairy, new() }, { StatType.SpectroliteBeetle, new() }, { StatType.SpectroliteCactus, new() }, { StatType.SpectroliteSlime, new() }, 
        { StatType.SpectroliteKitten, new() }, { StatType.SpectroliteCreeper, new() }
    };

    public HashSet<Guid> ProjGuids = [];
    
    private readonly HashSet<Guid> _oldProjGuids = [];

    public override void ResetEffects()
    {
        foreach (var key in Stats.Keys)
            Stats[key] = new();

        _oldProjGuids.Clear();

        foreach (Guid guid in ProjGuids)
            _oldProjGuids.Add(guid);

        ProjGuids.Clear();
    }

    public bool HasGuid(Guid guid) => ProjGuids.Contains(guid) || _oldProjGuids.Contains(guid);
}
