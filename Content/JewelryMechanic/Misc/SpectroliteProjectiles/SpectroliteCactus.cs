using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

internal class SpectroliteCactus : SpectroliteProjectile
{
    private Player Owner => Main.player[Projectile.owner];

    public ref float SineTimerX => ref Projectile.localAI[0];
    public ref float SineTimerY => ref Projectile.localAI[1];

    public ref float UsedTimer => ref Projectile.ai[0];
    public ref float Strength => ref Projectile.ai[1];

    public override StatType StatType => StatType.SpectroliteCactus;

    public override void SetStaticDefaults() => Main.projFrames[Type] = 2;

    public override void SetDefaults()
    {
        Projectile.timeLeft = 2;
        Projectile.friendly = true;
        Projectile.Size = new Vector2(24, 16);
    }

    public override bool? CanDamage() => false;

    public override void AI()
    {
        const float XMod = 0.05f;

        Projectile.timeLeft++;
        UsedTimer--;

        if (Strength < 1)
            Projectile.frame = 1;
        else
            Projectile.frame = 0;

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
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        if (Projectile.hide)
            overPlayers.Add(index);
    }

    public override Color? GetAlpha(Color lightColor) => UsedTimer > 0 ? Color.Lerp(lightColor, Color.Black, 0.5f) : lightColor;
}
