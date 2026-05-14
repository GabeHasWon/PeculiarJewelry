using PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SpectroliteStats;

internal class SlimeStat : SpectroliteStatEffect
{
    public override StatType Type => StatType.SpectroliteSlime;
    public override Color Color => new(255, 132, 195);

    public override void Apply(Player player, float strength) { }
    protected override float InternalEffectBonus(float multiplier, Player player) => 1f;
}
