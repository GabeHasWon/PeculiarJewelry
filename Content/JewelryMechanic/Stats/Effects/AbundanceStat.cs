﻿namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects;

/// <summary>
/// Max minions. MP safe.
/// </summary>
internal class AbundanceStat : JewelStatEffect
{
    public override StatType Type => StatType.Abundance;
    public override Color Color => Color.LightCyan;
    public override StatExclusivity Exclusivity => StatExclusivity.Summon;

    public override void Apply(Player player, float strength) => player.maxMinions += (int)GetEffectBonus(player, strength);
    protected override float InternalEffectBonus(float multiplier, Player player) => PeculiarJewelry.StatConfig.AbundanceStrength * multiplier * 2;
}
