using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Effects;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SoulstoneStats;
using System;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Stats;

internal class SoulstoneContainer(StatType category, float cooldown, ClassEnum classType = ClassEnum.Invalid) : JewelStat(category)
{
    public static JewelryStatConfig Config => ModContent.GetInstance<JewelryStatConfig>();

    public readonly float Cooldown = cooldown;

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
        activeSoulstone.Strength = Strength * ConfigModifier(Type) * multiplier;
        activeSoulstone.MaxCooldown = Cooldown;
        player.GetModPlayer<SoulstonePlayer>().InfoByStatType[Type].Soulstones.Add(activeSoulstone);
    }

    public override JewelStatEffect Get() => stat;

    public static float ConfigModifier(StatType type) => type switch
    {
        StatType.SoulAgony => Config.SoulAgonyStrength,
        StatType.SoulGrief => Config.SoulGriefStrength,
        StatType.SoulPlague => Config.SoulPlagueStrength,
        StatType.SoulBetrayal => Config.SoulBetrayalStrength,
        StatType.SoulSacrifice => Config.SoulSacrificeStrength,
        StatType.SoulTorture => Config.SoulTortureStrength,
        _ => throw new ArgumentException("SoulstoneStat's Type is not a Soul stat type."),
    };
}
