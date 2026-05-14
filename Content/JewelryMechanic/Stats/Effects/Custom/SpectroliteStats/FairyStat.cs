using PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;
using PeculiarJewelry.Content.JewelryMechanic.Misc;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SpectroliteStats;

internal class FairyStat : SpectroliteStatEffect
{
    public override StatType Type => StatType.SpectroliteFairy;
    public override Color Color => new(213, 150, 255);

    public override void Apply(Player player, float strength) => player.GetModPlayer<SpectrolitePlayer>().Stats[StatType.SpectroliteFairy] += InternalEffectBonus(strength, player);
    protected override float InternalEffectBonus(float multiplier, Player player) => multiplier;
}
