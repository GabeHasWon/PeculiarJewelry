using PeculiarJewelry.Content.JewelryMechanic.Stats;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

internal abstract class SpectroliteProjectile : ModProjectile
{
    public abstract StatType StatType { get; }

    public virtual float MaxTimer => 20 * 60 / Strength;

    protected Player Owner => Main.player[Projectile.owner];

    public ref float Strength => ref Projectile.ai[1];
    public ref float IsBig => ref Projectile.ai[2];

    public sealed override bool PreAI()
    {
        Player owner = Main.player[Projectile.owner];

        if (owner.GetModPlayer<SpectrolitePlayer>().Stats.TryGetValue(StatType, out SpectrolitePlayer.ValuePair value) && value.Stacks > 0)
        {
            if (owner.ownedProjectileCounts[Type] > value.Stacks)
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
