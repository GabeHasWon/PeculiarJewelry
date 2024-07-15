namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects;

/// <summary>
/// Max life. MP safe.
/// </summary>
internal class NoneStat : JewelStatEffect
{
    public override StatType Type => StatType.None;
    public override Color Color => Color.Transparent;

    public override void Apply(Player player, float strength) { }
    protected override float InternalEffectBonus(float multiplier, Player player) => 0;
}
