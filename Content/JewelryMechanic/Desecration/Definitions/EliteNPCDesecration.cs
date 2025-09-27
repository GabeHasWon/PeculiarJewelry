using System;
using Terraria.DataStructures;

namespace PeculiarJewelry.Content.JewelryMechanic.Desecration.Definitions;

internal class EliteNPCDesecration : DesecrationModifier
{
    public override float StrengthCap => 2f;
    public override float Profanity => 2f;

    public class EliteNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private bool _isElite = false;
        private short _eliteTimer = 0;

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            float str = Desecrations[nameof(EliteNPCDesecration)].strength;

            if (str <= 0 || npc.friendly || npc.lifeMax < 5 || npc.damage < 0)
                return;

            _isElite = Main.rand.NextFloat() < 0.125f * str;

            if (_isElite)
            {
                BuildStats(npc, str);

                if (Main.netMode != NetmodeID.SinglePlayer)
                    npc.netUpdate = true;
            }
        }

        private static void BuildStats(NPC npc, float str)
        {
            float factor = 1 + (0.33f * str);
            npc.damage = (int)(npc.damage * factor);
            npc.defense = (int)(npc.defense * factor);
            npc.lifeMax = (int)(npc.lifeMax * factor);
            npc.life = npc.lifeMax;

            npc.GetGlobalNPC<NPCBehaviourBoostGlobal>().modifiedAISpeed += factor - 1;
        }

        public override bool PreAI(NPC npc)
        {
            if (_isElite)
            {
                _eliteTimer++;

                if (_eliteTimer >= 60 * 8 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int damage = npc.damage;

                    if (Main.masterMode)
                        damage /= 6;
                    else if (Main.expertMode)
                        damage /= 4;
                    else
                        damage /= 2;

                    Vector2 vel = new Vector2(0, -6).RotatedByRandom(MathF.Tau);
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, vel, ModContent.ProjectileType<SoulProjectile>(), damage, 3f, Main.myPlayer);
                    _eliteTimer = 0;
                }
            }

            return true;
        }

        public override Color? GetAlpha(NPC npc, Color drawColor)
        {
            if (_isElite)
                return Color.Lerp(drawColor, Color.MediumPurple, 0.5f);

            return null;
        }
    }

    class SoulProjectile : ModProjectile
    {
        public override string Texture => "PeculiarJewelry/Assets/Textures/SoulProjectile";

        private ref float Timer => ref Projectile.ai[0];

        public override void SetDefaults()
        {
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.aiStyle = -1;
            Projectile.width = Projectile.height = 30;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60 * 12;
        }

        public override void AI()
        {
            Projectile.rotation += 0.4f;

            Timer++;

            if (Timer <= 60)
            {
                Projectile.velocity *= 0.98f;
                Projectile.Opacity = Timer / 60f;
            }
            else if (Timer <= 86)
            {
                var plr = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];

                if (!plr.active || plr.dead)
                {
                    Projectile.Kill();
                    return;
                }

                Projectile.velocity += Projectile.DirectionTo(plr.Center) * 0.75f;
            }

            if (Projectile.timeLeft < 60)
                Projectile.Opacity = Projectile.timeLeft / 60f;

            Lighting.AddLight(Projectile.Center, new Vector3(0.1f, 0.4f, 0.4f));
        }

        public override bool CanHitPlayer(Player target) => Timer > 60;
    }
}
