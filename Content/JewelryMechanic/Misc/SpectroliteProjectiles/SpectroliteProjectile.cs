using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

internal abstract class SpectroliteProjectile : ModProjectile
{
    public abstract StatType StatType { get; }

    public virtual float MaxTimer => 20 * 60 / Strength;

    protected Player Owner => Main.player[Projectile.owner];

    public ref float Timer => ref Projectile.ai[0];
    public ref float Strength => ref Projectile.ai[1];
    public ref float IsBig => ref Projectile.ai[2];

    internal Guid selfGuid = default;

    public sealed override bool PreAI()
    {
        Player owner = Main.player[Projectile.owner];
        SpectrolitePlayer spec = owner.GetModPlayer<SpectrolitePlayer>();
        
        if (spec.Stats.TryGetValue(StatType, out SpectrolitePlayer.ValuePair value) && value.Stacks > 0)
        {
            if (Main.myPlayer == Projectile.owner && !spec.HasGuid(selfGuid))
            {
                owner.ownedProjectileCounts[Type]--;
                Projectile.Kill();
                return false;
            }

            owner.GetModPlayer<SpectroliteProjPlayer>().ProjCountByStatType[StatType]++;
        }
        else
        {
            Projectile.Kill();
            return false;
        }

        return true;
    }
}
