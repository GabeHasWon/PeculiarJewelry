using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class TitaniumBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Titanium";

    float bonus = 1f;

    public override bool AppliesToStat(Player player, StatType type) => type is StatType.Potency or StatType.Absolution;
    public override void SingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1f + player.GetModPlayer<CatEyePlayer>().GetBonus(MaterialKey, 0.15f);
    public override void ResetSingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1f;

    public override float EffectBonus(Player player, StatType statType)
    {
        float count = player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);
        return count >= 1 ? bonus : 1f;
    }

    public override void StaticBonus(Player player, bool firstSet)
    {
        float count = CountMaterial(player);

        if (count >= 3)
            player.GetModPlayer<TitaniumBonusPlayer>().threeSet = true;

        if (count >= 5)
            player.GetModPlayer<TitaniumBonusPlayer>().fiveSet = true;
    }

    private class TitaniumBonusPlayer : ModPlayer
    {
        internal bool threeSet = false;
        internal bool fiveSet = false;

        private int _lastNPCHit = -1;

        public override void ResetEffects() => threeSet = fiveSet = false;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (threeSet)
            {
                UpdateHitNPC(target);

                target.AddBuff(ModContent.BuffType<TitaniumDefenseDebuff>(), 2);
                target.GetGlobalNPC<TitaniumGlobalNPC>().debuffStacks++;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (fiveSet && proj.type != ModContent.ProjectileType<EchoProjectile>())
                SpawnEcho(target, damageDone);
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (fiveSet)
                SpawnEcho(target, damageDone);
        }

        private void SpawnEcho(NPC target, int damageDone)
        {
            Projectile.NewProjectileDirect(target.GetSource_OnHurt(Player), Player.Center,
                new Vector2(0, Main.rand.NextFloat(5, 9)).RotatedByRandom(MathHelper.TwoPi), ModContent.ProjectileType<EchoProjectile>(),
                damageDone, 1f, Player.whoAmI, 0, target.whoAmI);
        }

        private void UpdateHitNPC(NPC target)
        {
            if (_lastNPCHit != target.whoAmI && _lastNPCHit >= 0)
                Main.npc[_lastNPCHit].GetGlobalNPC<TitaniumGlobalNPC>().debuffStacks = 0;

            _lastNPCHit = target.whoAmI;
        }
    }

    private class TitaniumDefenseDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true; // Not for players but yeah
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.GetGlobalNPC<TitaniumGlobalNPC>().debuffStacks <= 0)
            {
                npc.buffTime[buffIndex] = 0;
                buffIndex--;
            }
        }
    }

    private class TitaniumGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        internal int debuffStacks = 0;

        public override Color? GetAlpha(NPC npc, Color drawColor)
        {
            if (debuffStacks <= 0)
                return null;

            return GetTint(npc, drawColor);
        }

        private Color GetTint(NPC npc, Color drawColor)
        {
            const float Max = 0.6f;

            float factor = debuffStacks / (npc.defense * 0.5f);
            return Color.Lerp(drawColor, Color.Red, Math.Min(Max, factor * Max));
        }

        private int EffectiveStacks(NPC npc) => Math.Min(npc.defense / 2, debuffStacks);

        public override void ModifyHitByItem(NPC npc, Player p, Item i, ref NPC.HitModifiers modifiers) => modifiers.FlatBonusDamage += EffectiveStacks(npc);
        public override void ModifyHitByProjectile(NPC npc, Projectile p, ref NPC.HitModifiers modifiers) => modifiers.FlatBonusDamage += EffectiveStacks(npc);

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (debuffStacks > 0)
            {
                var val = debuffStacks.ToString();
                Vector2 origin = FontAssets.DeathText.Value.MeasureString(val) / 2f;
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.DeathText.Value, val, 
                    new Vector2(npc.Center.X, npc.position.Y - 22) - Main.screenPosition, GetTint(npc, drawColor), 0f, origin, new(0.75f));
            }
        }
    }

    internal class EchoProjectile : ModProjectile
    {
        private ref float Timer => ref Projectile.ai[0];
        private ref float TargetWhoAmI => ref Projectile.ai[1];
        
        private bool DeathThroes
        {
            get => Projectile.ai[2] == 1;
            set => Projectile.ai[2] = value ? 1 : 0;
        }

        private NPC Target => Main.npc[(int)TargetWhoAmI];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 6;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            Projectile.DamageType = DamageClass.Generic;
            Projectile.width = Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.Opacity = 0.6f;
            Projectile.tileCollide = false;
            Projectile.penetrate = 3;
        }

        public override void AI()
        {
            Timer++;
            Projectile.timeLeft = 2;

            if (DeathThroes)
            {
                Projectile.velocity *= 0.7f;
                Projectile.scale *= 1.05f;
                Projectile.Opacity *= 0.9f;

                if (Projectile.Opacity < 0.05f)
                    Projectile.Kill();
                return;
            }

            if (Timer < 30)
                Projectile.velocity *= 0.99f;
            else
            {
                if (!Target.active || Target.life < 0)
                {
                    DeathThroes = true;
                    return;
                }

                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Target.Center) * 12, 0.08f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 6; ++i)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.FireworkFountain_Blue, Alpha: 80);

            DeathThroes = true;
        }

        public override bool? CanHitNPC(NPC target) => DeathThroes ? false : null;

        public override bool PreDraw(ref Color lightColor)
        {
            float alpha = Projectile.Opacity;

            foreach (var item in Projectile.oldPos)
            {
                Vector2 pos = item - Main.screenPosition;
                alpha = MathHelper.Lerp(alpha, 0f, 0.25f);
                Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, pos, null, lightColor * alpha, 0f, Projectile.Size / 2f, Projectile.scale, SpriteEffects.None, 0);
            }

            return false;
        }
    }
}
