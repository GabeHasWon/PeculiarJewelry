using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class CobaltBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Cobalt";

    float bonus = 1f;

    public override bool AppliesToStat(Player player, StatType type) => type == StatType.Abundance || type == StatType.Legion || type == StatType.Order;
    public override void SingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1.15f;
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
            player.GetModPlayer<CobaltBonusPlayer>().threeSet = true;

        if (count >= 5)
            player.GetModPlayer<CobaltBonusPlayer>().fiveSet = true;
    }

    private class CobaltBonusPlayer : ModPlayer
    {
        internal bool threeSet = false;
        internal bool oldThree = false;
        internal bool fiveSet = false;

        public override void ResetEffects()
        {
            oldThree = threeSet;
            threeSet = false;
            fiveSet = false;
        }
    }

    internal class CobaltBonusProjectile : GlobalProjectile
    {
        private static readonly int[] ExceptionIDs = new int[] { ProjectileID.StardustDragon1, ProjectileID.StardustDragon2, ProjectileID.StardustDragon3, 
            ProjectileID.StardustDragon4, ProjectileID.StormTigerTier1, ProjectileID.StormTigerTier2, ProjectileID.StormTigerTier3, ProjectileID.StormTigerGem, 
            ProjectileID.AbigailMinion, ProjectileID.AbigailCounter };

        public override bool InstancePerEntity => true;

        private bool _shadow = false;
        private int _shadowOwner = -1;
        private int _ownsShadow = -1;
        private bool _shouldOwnShadow = false;
        private int _speedUpTimer = 0;

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.minion || entity.sentry || ExceptionIDs.Contains(entity.type);

        private static bool IsRealShadow(Projectile projectile)
        {
            var cobalt = projectile.GetGlobalProjectile<CobaltBonusProjectile>();
            return cobalt._shadow && !ExceptionIDs.Contains(projectile.type);
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            bool ownerThreeSet = Main.player[projectile.owner].GetModPlayer<CobaltBonusPlayer>().oldThree;
            bool switching = source is EntitySource_Misc { Context: "StormTigerTierSwap" or "AbigailTierSwap" } && ownerThreeSet;
            bool normal = source is EntitySource_ItemUse_WithAmmo itemUse && itemUse.Player.GetModPlayer<CobaltBonusPlayer>().threeSet;

            if (normal || switching) // Normal is most projectiles, switching is the projectiles that spawn from a spawner proj
            {
                if (!projectile.minion && !projectile.sentry)
                    return;

                if (projectile.damage <= 0)
                    return;

                if (projectile.GetGlobalProjectile<CobaltBonusProjectile>()._shadow)
                    return;

                if (ExceptionIDs.Contains(projectile.type))
                {
                    projectile.GetGlobalProjectile<CobaltBonusProjectile>()._shadow = true;
                    projectile.damage += projectile.damage / 2;
                    return;
                }

                int shadowCount = 0;
                int maxCount = Main.player[projectile.owner].maxMinions;
                
                if (projectile.sentry)
                    maxCount = Main.player[projectile.owner].maxTurretsOld;

                for (int i = 0; i < Main.maxProjectiles; i++) // Cap shadow minions
                {
                    Projectile p = Main.projectile[i];
                    bool check = projectile.sentry ? p.sentry : p.minion;

                    if (p.active && check && p.owner == projectile.owner && p.GetGlobalProjectile<CobaltBonusProjectile>()._shadow)
                    {
                        shadowCount++;

                        if (shadowCount > maxCount)
                            return;
                    }
                }

                projectile.GetGlobalProjectile<CobaltBonusProjectile>()._shouldOwnShadow = true;
            }
        }

        private static int SpawnShadow(Projectile parent)
        {
            int newProj = Projectile.NewProjectile(new EntitySource_Parent(parent, "ShadowClone"), parent.Center, parent.velocity * 0.5f,
                parent.type, parent.damage / 2, parent.knockBack, parent.owner);

            if (parent.sentry)
                Main.projectile[newProj].position -= new Vector2(0, 16);

            Main.projectile[newProj].GetGlobalProjectile<CobaltBonusProjectile>()._shadow = true;
            Main.projectile[newProj].GetGlobalProjectile<CobaltBonusProjectile>()._shadowOwner = parent.whoAmI;
            Main.projectile[newProj].minionSlots = 0; // Don't take up extra slots
            Main.projectile[newProj].Opacity = 0.5f;
            Main.projectile[newProj].minion = Main.projectile[newProj].sentry = false;

            parent.GetGlobalProjectile<CobaltBonusProjectile>()._ownsShadow = newProj;
            parent.GetGlobalProjectile<CobaltBonusProjectile>()._shouldOwnShadow = false;
            return newProj;
        }

        public override void DrawBehind(Projectile projectile, int index, List<int> bT, List<int> b, List<int> behindProjectiles, List<int> oP, List<int> oW)
        {
            if (projectile.GetGlobalProjectile<CobaltBonusProjectile>()._shadow)
                behindProjectiles.Add(index);
        }

        public override bool PreAI(Projectile projectile)
        {
            var selfCobalt = projectile.GetGlobalProjectile<CobaltBonusProjectile>();

            SpeedUpBehaviour(projectile);

            if (selfCobalt._shouldOwnShadow)
                SpawnShadow(projectile);

            if (!IsRealShadow(projectile))
                return true;

            int shadowOwner = selfCobalt._shadowOwner;
            if (Main.projectile[shadowOwner].type != projectile.type)
            {
                projectile.Kill();
                return false;
            }

            if (projectile.TryGetOwner(out Player owner) && !owner.GetModPlayer<CobaltBonusPlayer>().threeSet)
            {
                projectile.Kill();
                return false;
            }

            return true;
        }

        private static void SpeedUpBehaviour(Projectile projectile)
        {
            if (projectile.TryGetOwner(out Player player) && player.GetModPlayer<CobaltBonusPlayer>().fiveSet && 
                projectile.GetGlobalProjectile<CobaltBonusProjectile>()._speedUpTimer++ % 2 == 0)
                RepeatAI(projectile, 1);
        }

        public static void RepeatAI(Projectile projectile, int repeats)
        {
            int type = projectile.type;
            bool actType = projectile.ModProjectile != null && projectile.ModProjectile.AIType > 0;

            for (int i = 0; i < repeats; ++i)
            {
                if (actType)
                    projectile.type = projectile.ModProjectile.AIType;

                projectile.VanillaAI();

                if (actType)
                    projectile.type = type;
            }

            ProjectileLoader.AI(projectile);
        }

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            int owns = projectile.GetGlobalProjectile<CobaltBonusProjectile>()._ownsShadow;

            if (owns >= 0)
                Main.projectile[owns].Kill();

            int owner = projectile.GetGlobalProjectile<CobaltBonusProjectile>()._shadowOwner;

            if (owner >= 0)
            {
                var ownerCobaltProj = Main.projectile[owner].GetGlobalProjectile<CobaltBonusProjectile>();
                ownerCobaltProj._ownsShadow = -1;
                ownerCobaltProj._shouldOwnShadow = false;
            }
        }

        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (projectile.GetGlobalProjectile<CobaltBonusProjectile>()._shadow)
                Main.instance.PrepareDrawnEntityDrawing(projectile, GameShaders.Armor.GetShaderIdFromItemId(ItemID.TwilightDye), null);

            return true;
        }
    }
}
