using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

internal class SpectroliteFairy : SpectroliteProjectile
{
    public override StatType StatType => StatType.SpectroliteFairy;

    private ref float Timer => ref Projectile.ai[0];
    private ref float RotationSpeed => ref Projectile.localAI[0];

    public int HealAmount => (int)(Owner.statLifeMax2 * 0.1f);

    public override void SetStaticDefaults() => Main.projFrames[Type] = 8;

    public override void SetDefaults()
    {
        Projectile.timeLeft = 2;
        Projectile.friendly = true;
        Projectile.Size = new Vector2(24, 16);
        Projectile.tileCollide = false;
    }

    public override bool? CanDamage() => false;

    public override void AI()
    {
        Projectile.timeLeft++;
        Projectile.frameCounter++;
        Projectile.Opacity = Math.Min(Timer / MaxTimer, 1) * 0.5f + 0.5f;

        if (IsBig == 1)
            Projectile.frame = 2 + (int)(Projectile.frameCounter / 3f % 2);
        else
            Projectile.frame = (int)(Projectile.frameCounter / 3f % 2);

        if (Timer == 0 && RotationSpeed == 0)
            RotationSpeed = Main.rand.NextFloat(0.01f, 0.04f) * (Main.rand.NextBool() ? -1 : 1);

        Timer++;

        if (Timer >= MaxTimer)
        {
            if (Owner.statLife < Owner.statLifeMax2 - HealAmount)
            {
                Owner.Heal(HealAmount);
                Timer = 0;

                for (int i = 0; i < 12; ++i)
                    Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(Projectile.Hitbox), DustID.PinkFairy, Main.rand.NextVector2CircularEdge(4, 4) * Main.rand.NextFloat(0.5f, 1f));
            }

            if (Main.rand.NextBool(12))
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PinkFairy, Alpha: 255);
        }
        else
            Projectile.frame += 4;

        if (Projectile.DistanceSQ(Owner.Center) > 140 * 140)
        {
            Vector2 old = Projectile.Center;
            Projectile.Center = Vector2.SmoothStep(Projectile.Center, Owner.Center, 0.1f);
            Projectile.velocity = Projectile.Center - old;
        }
        else
            Projectile.velocity = Projectile.velocity.RotatedBy(RotationSpeed);
    }
}
