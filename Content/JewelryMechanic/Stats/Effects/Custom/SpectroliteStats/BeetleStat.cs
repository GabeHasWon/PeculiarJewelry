using PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SpectroliteStats;

internal class BeetleStat : SpectroliteStatEffect
{
    public override StatType Type => StatType.SpectroliteBeetle;
    public override Color Color => new(213, 150, 255);

    public override void Apply(Player player, float strength) { }
    protected override float InternalEffectBonus(float multiplier, Player player) => 1f;
}
