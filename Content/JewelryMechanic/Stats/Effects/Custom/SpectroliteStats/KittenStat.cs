using PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;
using PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SpectroliteStats;

internal class KittenStat : SpectroliteStatEffect
{
    public override StatType Type => StatType.SpectroliteKitten;
    public override Color Color => new(213, 150, 255);
    public override int ProjId => ModContent.ProjectileType<SpectroliteCat>();

    protected override float InternalEffectBonus(float multiplier, Player player) => 1f;
}
