using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Effects;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SoulstoneStats;
using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;
using System;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Stats;

internal class SoulstoneContainer(StatType category, ClassEnum classType = ClassEnum.Invalid) : JewelStat(category)
{
    private readonly SoulstonePlayer.ActiveSoulstone activeSoulstone = new();

    internal readonly SoulstoneStat stat = category switch
    {
        StatType.SoulAgony => new AgonyStat(classType),
        StatType.SoulGrief => new GriefStat(classType),
        StatType.SoulPlague => new PlagueStat(classType),
        StatType.SoulBetrayal => new BetrayalStat(classType),
        StatType.SoulSacrifice => new SacrificeStat(classType),
        StatType.SoulTorture => new TortureStat(classType),
        _ => throw new ArgumentException("SoulstoneStat's Type is not a Soul stat type."),
    };

    public override void Apply(Player player, float add = 0, float multiplier = 0)
    {
        base.Apply(player, add, multiplier);
        activeSoulstone.Class = stat.Class;
        player.GetModPlayer<SoulstonePlayer>().InfoByInfo[Type].Soulstones.Add(activeSoulstone);
    }

    public override JewelStatEffect Get() => stat;
}
