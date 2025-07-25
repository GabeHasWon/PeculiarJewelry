using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;
using System;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc;

internal class SoulstoneWisp : ModProjectile
{
    public MajorSoulstoneInfo.ClassEnum Class => (MajorSoulstoneInfo.ClassEnum)Projectile.ai[0];

    public override void SetStaticDefaults() => Main.projFrames[Type] = 9;

    public override void SetDefaults()
    {
        Projectile.timeLeft = 2;
        Projectile.Size = new Vector2(28);
    }

    public override void AI()
    {
        Projectile.frameCounter++;
        Projectile.timeLeft++;

        if (Projectile.velocity.LengthSquared() < 0.2f * 0.2f)
        {
            Projectile.velocity = new Vector2(0.2f, 0).RotatedByRandom(MathHelper.TwoPi);
            Projectile.netUpdate = true;
        }
        else
        {
            Projectile.velocity = Projectile.velocity.RotatedBy(0.02f);
        }

        if (Projectile.frameCounter < 25)
            Projectile.frame = Projectile.frameCounter / 4;
        else
            Projectile.frame = (int)(Projectile.frameCounter / 4f % 4) + 5;

        Player player = Main.player[Projectile.owner];

        if (Projectile.Hitbox.Intersects(player.Hitbox))
            Projectile.active = false;

        if (player.DistanceSQ(Projectile.Center) > 1000 * 1000)
            Projectile.active = false;
    }

    public override Color? GetAlpha(Color lightColor) => MajorSoulstoneInfo.GetClassColor(Class, MathF.Sin(Projectile.frameCounter * 0.2f) * 0.4f + 0.6f);
}
