using System;
using System.Collections.Generic;
using Terraria.GameContent;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers.TriggerEffects;

internal class DebuffWaveTrigger : TriggerEffect
{
    public override TriggerType Type => TriggerType.InstantOther;

    protected override void InternalInstantEffect(TriggerContext context, Player player, float coefficient, JewelTier tier)
    {
        if (Main.myPlayer == player.whoAmI) // Run only on local client
        {
            int proj = Projectile.NewProjectile(player.GetSource_Misc("JewelryTrigger"), player.Center, Vector2.Zero, ModContent.ProjectileType<AoEProjectile>(), 0, 0,
                player.whoAmI, TotalTriggerPower(player, 0.1f, tier));

            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
        }
    }

    protected override float InternalTriggerPower() => 100;

    class AoEProjectile : ModProjectile
    {
        private const int MaxTimeLeft = 120;

        public override string Texture => "PeculiarJewelry/Assets/Textures/Ring";

        private float EffectStrength => Projectile.ai[0];
        private ref float Radius => ref Projectile.ai[1];

        private float MaxRadius => EffectStrength * 16 / 2f;

        private HashSet<int> _blockedWhoAmI = new();

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = MaxTimeLeft;
        }

        public override bool? CanCutTiles() => false;

        public override void AI()
        {
            Radius = MathHelper.Lerp(Radius, MaxRadius, 0.1f);
            Projectile.scale = Radius / 109f * 2;
            Projectile.Opacity = 1 - (Radius / MaxRadius);

            if (Projectile.Opacity < 0.005f)
            {
                Projectile.Kill();
                return;
            }

            if (Projectile.timeLeft % 2 == 0)
                return;

            for (int i = 0; i < Main.maxNPCs; ++i)
            {
                if (_blockedWhoAmI.Contains(i))
                    continue;

                NPC npc = Main.npc[i];

                if (npc.CanBeChasedBy() && npc.DistanceSQ(Projectile.Center) < Radius * Radius)
                {
                    npc.AddBuff(Main.rand.Next(new int[] { BuffID.OnFire, BuffID.Poisoned, BuffID.Venom, BuffID.Frostburn }), (int)(EffectStrength * 60));
                    _blockedWhoAmI.Add(i);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            var pos = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(tex, pos, null, lightColor * Projectile.Opacity, 0f, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}