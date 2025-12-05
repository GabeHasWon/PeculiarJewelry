namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SpectroliteStats;

internal class CactusStat : JewelStatEffect
{
    public override StatType Type => StatType.SpectroliteCactus;
    public override Color Color => new(73, 120, 17);

    public override void Apply(Player player, float strength) { }
    protected override float InternalEffectBonus(float multiplier, Player player) => 1f;
}
