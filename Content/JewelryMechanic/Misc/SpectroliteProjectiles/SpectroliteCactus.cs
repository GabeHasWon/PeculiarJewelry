using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

internal class SpectroliteCactus : ModProjectile
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
        const float XMod = 0.2f;

        Projectile.timeLeft++;

        float xOff = (float)Math.Sin(Projectile.ai[0]++ * XMod);

        Projectile.Center = Owner.Center + new Vector2(0, Owner.gfxOffY);
        Projectile.position.X += xOff * 42f;
        Projectile.position.Y += (float)Math.Sin(Projectile.ai[1]++ * 0.06f) * 8f;
        Projectile.rotation = xOff * 0.2f;

        Lighting.AddLight(Projectile.Center, new Vector3(0.4f, 0.12f, 0.24f) * 0.8f);

        if (Math.Cos(Projectile.ai[0] * XMod) > 0)
            Projectile.hide = true;
        else
            Projectile.hide = false;

        if (Owner.GetModPlayer<SpectrolitePlayer>().Stats.TryGetValue(StatType.SpectroliteFairy, out float value) && value > 0)
        {

        }
        else
        {
            Projectile.Kill();
        }
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        if (Projectile.hide)
            overPlayers.Add(index);
    }
}
