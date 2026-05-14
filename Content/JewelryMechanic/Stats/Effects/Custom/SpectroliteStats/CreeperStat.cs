using PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SpectroliteStats;

internal class CreeperStat : SpectroliteStatEffect
{
    public override StatType Type => StatType.SpectroliteCreeper;
    public override Color Color => new(213, 150, 255);

    public override void Apply(Player player, float strength) { }
    protected override float InternalEffectBonus(float multiplier, Player player) => 1f;
}
