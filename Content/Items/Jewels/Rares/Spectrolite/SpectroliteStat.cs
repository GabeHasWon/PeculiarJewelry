using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Effects;

namespace PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;

/// <summary>
/// Solely used to remove the + from the description.
/// </summary>
internal abstract class SpectroliteStatEffect : JewelStatEffect
{
    public abstract int ProjId { get; }

    internal override string GetDescription(Player player, string stars, float str, bool negative = false) => Description.Value;

    public sealed override void Apply(Player player, float strength)
    {
        player.GetModPlayer<SpectrolitePlayer>().Stats[Type].Stacks++;
        player.GetModPlayer<SpectrolitePlayer>().Stats[Type].Value += InternalEffectBonus(strength, player);

        if (player.ownedProjectileCounts[ProjId] < player.GetModPlayer<SpectrolitePlayer>().Stats[Type].Stacks)
        {
            Vector2 position = player.Center - new Vector2(0, 60);
            float isMajor = SpectroliteContainer.CurrentlyMajor is true ? 0 : 1;
            Projectile.NewProjectile(player.GetSource_FromThis(), position, Vector2.Zero, ProjId, 0, 0, player.whoAmI, 0, strength, isMajor);
        }
    }
}
