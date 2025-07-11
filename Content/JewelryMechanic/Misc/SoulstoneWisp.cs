namespace PeculiarJewelry.Content.JewelryMechanic.Misc;

internal class SoulstoneWisp : ModProjectile
{
    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 9;
    }

    public override void SetDefaults()
    {
        Projectile.timeLeft = 2;
        Projectile.Size = new Vector2(28);
    }

    public override void AI()
    {
        Projectile.frameCounter++;
        Projectile.timeLeft++;

        if (Projectile.frameCounter < 25)
            Projectile.frame = Projectile.frameCounter / 4;
        else
            Projectile.frame = (int)(Projectile.frameCounter / 4f % 4) + 5;

        if (Projectile.Hitbox.Intersects(Main.player[Projectile.owner].Hitbox))
        {
            Projectile.active = false;
        }
    }
}
