using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

internal class SpectroliteBeetle : SpectroliteProjectile
{
    public override StatType StatType => StatType.SpectroliteBeetle;

    public override void SetStaticDefaults() => Main.projFrames[Type] = 4;

    public override void SetDefaults()
    {
        Projectile.timeLeft = 2;
        Projectile.friendly = true;
        Projectile.Size = new Vector2(24, 16);
        Projectile.tileCollide = false;
        Projectile.hide = true;
        Projectile.Opacity = 1f;
    }

    public override bool? CanDamage() => false;

    public override void AI()
    {
        Projectile.timeLeft = 2;
        Projectile.frameCounter++;
        Timer++;

        int count = Owner.GetModPlayer<SpectroliteProjPlayer>().ProjCountByStatType[StatType] - 1;

        if (Timer > MaxTimer)
        {
            Vector2 center = Projectile.Center;
            Projectile.Center = Vector2.Lerp(center, Owner.Center - new Vector2(0, 24 + 8 * count), 0.6f);

            if (Owner.velocity.LengthSquared() > 3f)
            {
                if (IsBig == 1)
                    Projectile.frame = 2 + (int)(Projectile.frameCounter / 3f % 2);
                else
                    Projectile.frame = (int)(Projectile.frameCounter / 3f % 2);

                Projectile.spriteDirection = Projectile.direction = MathF.Sign(Owner.Center.X - Projectile.Center.X);
            }
            else
            {
                if (IsBig == 1)
                    Projectile.frame = 3;
                else
                    Projectile.frame = 1;

                Projectile.direction = Projectile.spriteDirection = Owner.direction;
            }
        }
        else
        {
            Projectile.Center = Vector2.Lerp(Projectile.Center, Owner.Center - new Vector2(0, 14 * count), 0.05f - 0.003f * count);

            if (IsBig == 1)
                Projectile.frame = 2 + (int)(Projectile.frameCounter / 3f % 2);
            else
                Projectile.frame = (int)(Projectile.frameCounter / 3f % 2);

            Projectile.spriteDirection = Projectile.direction = MathF.Sign(Owner.Center.X - Projectile.Center.X);
        }
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        if (Projectile.hide)
            overPlayers.Add(index);
    }
}
