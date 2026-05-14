using PeculiarJewelry.Content.JewelryMechanic.Stats;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

internal abstract class SpectroliteProjectile : ModProjectile
{
    public abstract StatType StatType { get; }

    public sealed override bool PreAI()
    {
        Player owner = Main.player[Projectile.owner];

        if (owner.GetModPlayer<SpectrolitePlayer>().Stats.TryGetValue(StatType, out float value) && value > 0)
        {
            if (owner.ownedProjectileCounts[Type] > value)
            {
                owner.ownedProjectileCounts[Type]--;
                Projectile.Kill();
                return false;
            }
        }
        else
        {
            Projectile.Kill();
            return false;
        }

        return true;
    }
}
