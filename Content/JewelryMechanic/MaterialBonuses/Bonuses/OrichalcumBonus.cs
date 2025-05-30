using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;
using Terraria.DataStructures;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class OrichalcumBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Orichalcum";

    float bonus = 1f;

    public override bool AppliesToStat(Player player, StatType type) => type is StatType.Arcane or StatType.Resurgence or StatType.Willpower;
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
            player.GetModPlayer<OrichalcumBonusPlayer>().threeSet = true;

        if (count >= 5)
            player.GetModPlayer<OrichalcumBonusPlayer>().fiveSet = true;
    }

    class OrichalcumBonusPlayer : ModPlayer
    {
        internal bool threeSet = false;
        internal bool fiveSet = false;
        internal int projCount = 0;
        internal float damageBonusForDisplay = 0;

        public override void ResetEffects() => threeSet = fiveSet = false;

        public override bool OnPickup(Item item)
        {
            if (fiveSet && item.type == ItemID.Star)
                Player.AddBuff(ModContent.BuffType<OrichalcumDamageBuff>(), 4 * 60);

            return true;
        }
    }

    class OrichalcumDamageBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            float bonus = player.buffTime[buffIndex] / 3000f;
            player.GetDamage(DamageClass.Generic) += bonus;
            player.GetModPlayer<OrichalcumBonusPlayer>().damageBonusForDisplay = bonus;
        }

        public override bool ReApply(Player player, int time, int buffIndex)
        {
            player.buffTime[buffIndex] += time;
            return true;
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) 
            => tip = tip.Replace("{0}", Main.LocalPlayer.GetModPlayer<OrichalcumBonusPlayer>().damageBonusForDisplay.ToString());
    }

    class OrichalcumBonusProjectile : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.damage < 0 || !projectile.DamageType.CountsAsClass(DamageClass.Magic))
                return;

            if (source is EntitySource_ItemUse_WithAmmo ammo)
            {
                var player = ammo.Player;
                var plr = player.GetModPlayer<OrichalcumBonusPlayer>();

                if (plr.threeSet)
                {
                    if (plr.projCount == 2)
                    {
                        if (!player.CheckMana(ammo.Item.mana, true))
                            return;

                        plr.projCount = 0;

                        projectile.position = projectile.Center;
                        projectile.scale *= 1.5f;
                        projectile.Size *= 1.5f;
                        projectile.position -= projectile.Size / 2;
                        projectile.damage = (int)(projectile.damage * 2f);
                    }
                    else
                        plr.projCount++;
                }
            }
        }
    }
}
