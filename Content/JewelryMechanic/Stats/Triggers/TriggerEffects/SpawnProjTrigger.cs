using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.GameContent;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers.TriggerEffects;

internal class SpawnProjTrigger : TriggerEffect
{
    public override TriggerType Type => TriggerType.InstantOther;

    protected override void InternalInstantEffect(TriggerContext context, Player player, float coefficient, JewelTier tier)
    {
        if (Main.myPlayer == player.whoAmI) // Run only on local client
        {
            int damage = (int)TotalTriggerPower(player, coefficient, tier);
            var vel = new Vector2(0, 9).RotatedByRandom(MathHelper.TwoPi);
            var source = player.GetSource_Misc("JewelryTrigger:" + context);

            Color color = Color.White;

            var majorJewels = player.GetModPlayer<JewelPlayer>().TriggerJewelInfos.Where(i => i.effect is SpawnProjTrigger).ToList();
            var jewel = Main.rand.Next(majorJewels);
            color = jewel.Major.Get().Color;

            int proj = Projectile.NewProjectile(source, player.Center, vel, ModContent.ProjectileType<TriggerProj>(), damage, 2f, player.whoAmI);
            (Main.projectile[proj].ModProjectile as TriggerProj).color = color;

            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
        }
    }

    protected override float InternalTriggerPower() => 140;

    private class TriggerProj : ModProjectile
    {
        public override string Texture => "PeculiarJewelry/Assets/Textures/TriggerProj";

        private ref float Timer => ref Projectile.ai[0];
        private ref float Frame => ref Projectile.ai[1];
        private ref float TargetWhoAmI => ref Projectile.ai[2];

        public Color color = Color.White;

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bullet);
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 600;
            Projectile.Opacity = 1;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Timer++;

            if (Frame == 0)
                Frame = Main.rand.Next(3) + 1;
            else
            {
                if (Frame == 1)
                    Projectile.rotation += 0.04f * MathF.Sign(Projectile.velocity.X);
                else
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }

            if (Timer < 120)
                Projectile.velocity *= 0.97f;
            else if (Timer == 120)
                FindTarget();
            else
            {
                if (TargetWhoAmI == -1)
                    Fade();
                else if (Timer < 130)
                    Projectile.velocity += Projectile.DirectionTo(Main.npc[(int)TargetWhoAmI].Center);
            }

            if (Projectile.timeLeft <= 30)
                Fade();
        }

        private void Fade()
        {
            Projectile.Opacity *= 0.96f;
            Projectile.velocity *= 0.98f;

            if (Projectile.Opacity <= 0.1f)
                Projectile.Kill();
        }

        private void FindTarget()
        {
            HashSet<int> validTargets = [];

            for (int i = 0; i < Main.maxNPCs; ++i)
            {
                NPC npc = Main.npc[i];

                if (npc.CanBeChasedBy() && npc.DistanceSQ(Projectile.Center) < 1200 * 1200)
                    validTargets.Add(i);
            }

            if (validTargets.Count > 0)
                TargetWhoAmI = Main.rand.Next(validTargets.ToArray());
            else
                TargetWhoAmI = -1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var src = new Rectangle(28 * (int)(Frame - 1), 0, 26, 26);
            var tex = TextureAssets.Projectile[Type].Value;
            var col = Lighting.GetColor(Projectile.Center.ToTileCoordinates(), color) * Projectile.Opacity;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, src, col, Projectile.rotation, tex.Size() / new Vector2(6f, 2), 1f, SpriteEffects.None);
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(color.R);
            writer.Write(color.G);
            writer.Write(color.B);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            byte r = reader.ReadByte();
            byte g = reader.ReadByte();
            byte b = reader.ReadByte();
            color = new(r, g, b);
        }
    }
}
