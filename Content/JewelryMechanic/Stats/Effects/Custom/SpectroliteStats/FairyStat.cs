using PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;
using PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SpectroliteStats;

internal class FairyStat : SpectroliteStatEffect
{
    public override StatType Type => StatType.SpectroliteFairy;
    public override Color Color => new(213, 150, 255);
    public override int ProjId => ModContent.ProjectileType<SpectroliteFairy>();

    protected override float InternalEffectBonus(float multiplier, Player player) => multiplier;
}
