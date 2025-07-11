using PeculiarJewelry.Content.JewelryMechanic.Misc;
using System;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SoulstoneStats;

internal abstract class SoulstoneStat : JewelStatEffect
{
    public override Color Color => new(93, 93, 93);

    public override void Apply(Player player, float strength) => player.GetModPlayer<SoulstonePlayer>().StrengthPerSoulstone[Type] += (int)GetEffectBonus(player, strength);
    protected override float InternalEffectBonus(float multiplier, Player player)
    {
        JewelryStatConfig config = ModContent.GetInstance<JewelryStatConfig>();

        return multiplier * Type switch
        {
            StatType.SoulAgony => config.SoulAgonyStrength,
            StatType.SoulGrief => config.SoulGriefStrength,
            StatType.SoulPlague => config.SoulPlagueStrength,
            StatType.SoulBetrayal => config.SoulBetrayalStrength,
            StatType.SoulSacrifice => config.SoulSacrificeStrength,
            StatType.SoulTorture => config.SoulTortureStrength,
            _ => throw new ArgumentException("SoulstoneStat's Type is not a Soul stat type."),
        };
    }
}
