using PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;
using PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SpectroliteStats;

internal class CreeperStat : SpectroliteStatEffect
{
    public override StatType Type => StatType.SpectroliteCreeper;
    public override Color Color => new(160, 255, 199);
    public override int ProjId => ModContent.ProjectileType<SpectroliteCactus>();

    protected override float InternalEffectBonus(float multiplier, Player player) => 1f;
}
