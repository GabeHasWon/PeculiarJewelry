using PeculiarJewelry.Content.JewelryMechanic.Stats;
using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;
using ReLogic.Content;
using System;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc;

internal class SoulstoneWisp : ModProjectile
{
    public static Dictionary<StatType, Asset<Texture2D>> TexByType = [];

    public override string Texture => "Terraria/Images/NPC_0";

    public ClassEnum Class => (ClassEnum)Projectile.ai[0];
    public StatType SoulType => (StatType)Projectile.ai[1];

    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 8;

        TexByType.Add(StatType.SoulAgony, ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/Misc/SoulstoneWispAgony"));
        TexByType.Add(StatType.SoulSacrifice, ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/Misc/SoulstoneWispSacrifice"));
        TexByType.Add(StatType.SoulPlague, ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/Misc/SoulstoneWispPlague"));
        TexByType.Add(StatType.SoulGrief, ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/Misc/SoulstoneWispGrief"));
        TexByType.Add(StatType.SoulTorture, ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/Misc/SoulstoneWispTorture"));
    }

    public override void SetDefaults()
    {
        Projectile.timeLeft = 2;
        Projectile.Size = new Vector2(34, 68);
        Projectile.tileCollide = false;
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

        if (Projectile.frameCounter < 12)
            Projectile.frame = Projectile.frameCounter / 4 + 5;
        else
            Projectile.frame = (int)(Projectile.frameCounter / 4f % 5);

        Player player = Main.player[Projectile.owner];

        if (Projectile.Hitbox.Intersects(player.Hitbox))
            Projectile.active = false;

        if (player.DistanceSQ(Projectile.Center) > 1000 * 1000)
            Projectile.active = false;
    }

    public override Color? GetAlpha(Color lightColor)
    {
        float colorSin = MathF.Sin(Projectile.frameCounter * 0.2f);
        float opacitySin = MathF.Sin(Projectile.frameCounter * 0.05f);
        return MajorSoulstoneInfo.GetClassColor(Class, colorSin * 0.4f + 0.6f) * (opacitySin * 0.25f + 0.75f);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        if (!TexByType.TryGetValue(SoulType, out var val))
            val = TexByType[StatType.SoulAgony];

        Texture2D tex = val.Value;
        int frameHeight = tex.Height / Main.projFrames[Type];
        Rectangle src = new(0, Projectile.frame * frameHeight, tex.Width, frameHeight - 2);
        Vector2 position = Projectile.Center - Main.screenPosition - new Vector2(0, 12);
        Main.EntitySpriteDraw(tex, position, src, Projectile.GetAlpha(lightColor), Projectile.rotation, src.Size() / 2f, 1f, SpriteEffects.None, 0);

        return false;
    }
}
