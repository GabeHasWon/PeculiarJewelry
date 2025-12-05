using PeculiarJewelry.Content.JewelryMechanic.Stats;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

internal class SpectroliteFairy : ModProjectile
{
    private Player Owner => Main.player[Projectile.owner];

    public override void SetStaticDefaults() => Main.projFrames[Type] = 2;

    public override void SetDefaults()
    {
        Projectile.timeLeft = 2;
        Projectile.friendly = true;
        Projectile.Size = new Vector2(24, 16);
    }

    public override void AI()
    {
        Projectile.timeLeft++;

        if (Projectile.DistanceSQ(Owner.Center) > 80 * 80)
        {
            Vector2 old = Projectile.Center;
            Projectile.Center = Vector2.SmoothStep(Projectile.Center, Owner.Center, 0.1f);
            Projectile.velocity = Projectile.Center - old;
        }
        else
        {
            Projectile.velocity = Projectile.velocity.RotatedBy(-0.01f);
        }

        if (Owner.GetModPlayer<SpectrolitePlayer>().Stats.TryGetValue(StatType.SpectroliteFairy, out float value) && value > 0)
        {

        }
        else
        {
            Projectile.Kill();
        }
    }
}
