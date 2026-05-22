using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

internal class SpectroliteCactus : SpectroliteProjectile
{
    public ref float SineTimerX => ref Projectile.localAI[0];
    public ref float SineTimerY => ref Projectile.localAI[1];
    public ref float SineTimerZ => ref Projectile.localAI[2];

    public ref float UsedTimer => ref Projectile.ai[0];

    public override StatType StatType => StatType.SpectroliteCactus;

    public override void SetStaticDefaults() => Main.projFrames[Type] = 2;

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
        const float XMod = 0.05f;

        if (SineTimerX == 0)
        {
            SineTimerX = Main.rand.NextFloat(200);
            SineTimerY = Main.rand.NextFloat(200);
        }

        Projectile.timeLeft++;
        UsedTimer--;
        SineTimerZ += 1 - MathF.Max(0, UsedTimer / MaxTimer);

        Projectile.frame = (int)IsBig;

        float xOff = (float)Math.Sin(SineTimerX++ * XMod);

        Projectile.Center = Owner.Center + new Vector2(0, Owner.gfxOffY - 20);
        Projectile.position.X += xOff * 42f;
        Projectile.position.Y += (float)Math.Sin(SineTimerY++ * 0.06f) * 8f;
        Projectile.rotation = xOff * 0.2f;

        Lighting.AddLight(Projectile.Center, new Vector3(0.4f, 0.12f, 0.24f) * 0.8f);

        if (Math.Cos(SineTimerX * XMod) > 0)
            Projectile.hide = true;
        else
            Projectile.hide = false;

        if (Math.Sin(SineTimerX * XMod) > 0)
            Projectile.spriteDirection = 1;
        else
            Projectile.spriteDirection = -1;
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        if (Projectile.hide)
            overPlayers.Add(index);
    }

    public override Color? GetAlpha(Color lightColor) => UsedTimer > 0 ? Color.Lerp(lightColor, Color.Black, MathF.Sin(SineTimerZ) * 0.5f) : lightColor;
}
