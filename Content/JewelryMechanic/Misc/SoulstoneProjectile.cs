using PeculiarJewelry.Content.Items.Jewels.Rares.Soulstone;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;
using ReLogic.Content;
using System;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc;

internal class SoulstoneProjectile : ModProjectile
{
    private static Asset<Texture2D> AuraTex = null;

    public override string Texture => "PeculiarJewelry/Content/Items/Jewels/Rares/Soulstone/MinorSoulstone";

    public ClassEnum Class => (ClassEnum)Projectile.ai[0];
    public StatType SoulType => (StatType)Projectile.ai[1];

    private int Target
    {
        get => (int)Projectile.ai[2];
        set => Projectile.ai[2] = value;
    }

    private bool Initialized
    {
        get => Projectile.localAI[0] == 1;
        set => Projectile.localAI[0] = value ? 1 : 0;
    }

    public override void SetStaticDefaults()
    {
        AuraTex ??= ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/Misc/SoulstoneAura");

        Main.projFrames[Type] = 5;

        ProjectileID.Sets.TrailCacheLength[Type] = 15;
        ProjectileID.Sets.TrailingMode[Type] = 2;
    }

    public override void SetDefaults()
    {
        Projectile.timeLeft = 2;
        Projectile.Size = new Vector2(24, 22);
        Projectile.tileCollide = false;
        Projectile.friendly = true;
        Projectile.hostile = false;
    }

    public override void AI()
    {
        const float MaxSpeed = 12;

        Projectile.timeLeft++;
        Projectile.rotation = Projectile.velocity.ToRotation();

        if (!Initialized)
        {
            Initialized = true;
            FindTarget();
        }

        ConfirmTarget();

        Projectile.velocity += Projectile.DirectionTo(Main.npc[Target].Center);

        if (Projectile.velocity.LengthSquared() > MaxSpeed * MaxSpeed)
        {
            Projectile.velocity = Vector2.Normalize(Projectile.velocity) * MaxSpeed;
        }

        Projectile.velocity *= 0.98f;
    }

    private void ConfirmTarget()
    {
        NPC target = Main.npc[Target];

        if (target.active && target.CanBeChasedBy())
            return;

        FindTarget();

        if (target.active)
            Projectile.Kill();
    }

    private void FindTarget()
    {
        NPC cur = null;

        foreach (NPC npc in Main.ActiveNPCs)
        {
            if (npc.CanBeChasedBy() && npc.DistanceSQ(Projectile.Center) < 1600 * 1600 && (cur is null || npc.life > cur.life))
            {
                cur = npc;
            }
        }

        if (cur is null)
        {
            Projectile.Kill();
            return;
        }

        Target = cur.whoAmI;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => SoulstoneNPC.AddStack(target, SoulType);

    public override void OnKill(int timeLeft)
    {
        if (Main.dedServ)
            return;
        
        SpawnVFX(Projectile.GetSource_Death(), Projectile.Center, Projectile.velocity, (byte)Projectile.owner);
    }

    public static void SpawnVFX(IEntitySource source, Vector2 position, Vector2 velocity, byte owner)
    {
        for (int i = 0; i < 4; ++i)
        {
            Vector2 vel = velocity.RotatedByRandom(0.7f) * Main.rand.NextFloat(0.5f, 0.9f);
            Gore.NewGore(source, position, vel, ModContent.GoreType<SoulstoneAuraGore>(), Main.rand.NextFloat(0.3f, 0.6f));
        }

        for (int i = 0; i < 4; ++i)
        {
            Vector2 vel = velocity.RotatedByRandom(0.7f) * Main.rand.NextFloat(0.2f, 0.5f);
            Gore.NewGore(source, position, vel, ModContent.GoreType<SoulstoneAuraGore>(), Main.rand.NextFloat(0.6f, 1f));
        }

        for (int i = 0; i < 12; ++i)
        {
            ParticleOrchestraSettings settings = default;
            settings.MovementVector = velocity.RotatedByRandom(0.5f) * Main.rand.NextFloat(0.3f, 0.7f);
            settings.PositionInWorld = position;
            settings.IndexOfPlayerWhoInvokedThis = owner;
            ParticleOrchestrator.SpawnParticlesDirect(Main.rand.NextBool(5) ? ParticleOrchestraType.PaladinsHammer : ParticleOrchestraType.BlackLightningSmall, settings);
        }
    }

    public override Color? GetAlpha(Color lightColor)
    {
        float colorSin = MathF.Sin(Projectile.frameCounter * 0.2f);
        float opacitySin = MathF.Sin(Projectile.frameCounter * 0.05f);
        return Class.GetColor(colorSin * 0.4f + 0.6f) * (opacitySin * 0.25f + 0.75f);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        WispTrailDrawer drawer = default;
        drawer.Draw(Projectile);
        
        Vector2 position = Projectile.Bottom - Main.screenPosition - new Vector2(0, 12);
        Main.EntitySpriteDraw(AuraTex.Value, position, null, Color.White * 0.6f, 0f, AuraTex.Size() / 2f, 1f, SpriteEffects.None);

        Texture2D tex = TextureAssets.Projectile[Type].Value;
        int frameHeight = 24;
        Rectangle src = new(((int)SoulType - (int)StatType.SoulAgony) * 26, 4 * frameHeight, 24, frameHeight - 2);
        Main.EntitySpriteDraw(tex, position, src, Projectile.GetAlpha(lightColor), Projectile.rotation + MathHelper.PiOver2, src.Size() / 2f, 1f, SpriteEffects.None, 0);

        return false;
    }
}

[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct WispTrailDrawer
{
    private static readonly VertexStrip _vertexStrip = new();

    public readonly void Draw(Projectile proj)
    {
        MiscShaderData miscShaderData = GameShaders.Misc["MagicMissile"];
        miscShaderData.UseSaturation(-2f);
        miscShaderData.UseOpacity(2f * proj.Opacity);
        miscShaderData.Apply();
        _vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size / 2f);
        _vertexStrip.DrawTrail();
        Main.pixelShader.CurrentTechnique.Passes[0].Apply();
    }

    private readonly Color StripColors(float progressOnStrip)
    {
        Color result = Color.Lerp(Color.Black, Color.White * 0.2f, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
        result.A = (byte)(result.A * 0.7f);
        return result;
    }

    private readonly float StripWidth(float progress) => MathHelper.Lerp(30, 52f, Utils.GetLerpValue(0f, 0.2f, progress, true)) * Utils.GetLerpValue(0f, 0.07f, progress, true);
}