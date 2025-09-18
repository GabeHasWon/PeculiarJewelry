using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;
using System;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SoulstoneStats;

internal abstract class SoulstoneStat : JewelStatEffect
{
    public override Color Color => new(93, 93, 93);

    public ClassEnum Class = ClassEnum.Generic;

    public SoulstoneStat(ClassEnum classType = ClassEnum.Invalid)
    {
        Description = Language.GetText("Mods.PeculiarJewelry.Jewelry.StatTypes." + Type + ".Description");
        DisplayName = Language.GetText("Mods.PeculiarJewelry.Jewelry.StatTypes." + Type + ".DisplayName");
        Class = classType == ClassEnum.Invalid ? (ClassEnum)Main.rand.Next(5) : classType;
    }

    public override void Apply(Player player, float strength) => player.GetModPlayer<SoulstonePlayer>().InfoByInfo[Type].TotalStrength += GetEffectBonus(player, strength);

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

    internal override string GetDescription(Player player, string stars, float str, bool negative = false) => Description.Value;
}
