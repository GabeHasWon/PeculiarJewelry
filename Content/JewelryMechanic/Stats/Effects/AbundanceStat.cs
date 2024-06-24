using System;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects;

/// <summary>
/// Max minions. MP safe.
/// </summary>
internal class AbundanceStat : JewelStatEffect
{
    public override StatType Type => StatType.Abundance;
    public override Color Color => Color.Cyan;
    public override StatExclusivity Exclusivity => StatExclusivity.Summon;

    public override void Apply(Player player, float strength) => player.maxMinions += (int)MathF.Ceiling(GetEffectBonus(player, strength));
    protected override float InternalEffectBonus(float multiplier, Player player) => (int)MathF.Ceiling(PeculiarJewelry.StatConfig.AbundanceStrength * multiplier * 4);
}
