using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Effects;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SoulstoneStats;
using System;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Stats;

internal class SoulstoneContainer(StatType category) : JewelStat(category)
{
    private readonly SoulstonePlayer.ActiveSoulstone activeSoulstone = new();

    private readonly SoulstoneStat stat = category switch
    {
        StatType.SoulAgony => new AgonyStat(),
        StatType.SoulGrief => new GriefStat(),
        StatType.SoulPlague => new PlagueStat(),
        StatType.SoulBetrayal => new BetrayalStat(),
        StatType.SoulSacrifice => new SacrificeStat(),
        StatType.SoulTorture => new TortureStat(),
        _ => throw new ArgumentException("SoulstoneStat's Type is not a Soul stat type."),
    };

    public override void Apply(Player player, float add = 0, float multiplier = 0)
    {
        base.Apply(player, add, multiplier);
        player.GetModPlayer<SoulstonePlayer>().InfoByInfo[Type].Soulstones.Add(activeSoulstone);
    }

    public override JewelStatEffect Get() => stat;
}
