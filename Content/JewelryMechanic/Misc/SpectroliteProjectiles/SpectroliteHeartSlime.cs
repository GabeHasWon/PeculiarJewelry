using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

internal class SpectroliteHeartSlime : SpectroliteProjectile
{
    public override StatType StatType => StatType.SpectroliteSlime;

    private ref float Timer => ref Projectile.ai[0];

    private Vector2 AttachPosition
    {
        get => new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
        set => (Projectile.localAI[0], Projectile.localAI[1]) = (value.X, value.Y);
    }

    internal ref float Health => ref Projectile.localAI[2];

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

        Timer++;

        
    }
}
