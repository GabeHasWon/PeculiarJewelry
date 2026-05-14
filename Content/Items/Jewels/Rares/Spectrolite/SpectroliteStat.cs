using PeculiarJewelry.Content.JewelryMechanic.Stats.Effects;

namespace PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;

/// <summary>
/// Solely used to remove the + from the description.
/// </summary>
internal abstract class SpectroliteStatEffect : JewelStatEffect
{
    internal override string GetDescription(Player player, string stars, float str, bool negative = false) => Description.Value;
}
