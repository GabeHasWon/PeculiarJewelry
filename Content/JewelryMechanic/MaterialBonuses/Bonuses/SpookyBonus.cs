using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class SpookyBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Spooky";

    float bonus = 1f;

    public override bool AppliesToStat(Player player, StatType type) 
        => type == StatType.Exploitation || type == StatType.Exactitude || type == StatType.Renewal || type == StatType.Vigor;
    public override void SingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1.25f;
    public override void ResetSingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1f;

    public override float EffectBonus(Player player, StatType type)
    {
        bool movement = type == StatType.Exploitation || type == StatType.Exactitude;

        if (CountMaterial(player) >= 1)
            return movement ? bonus : 0.94f;
        return 1f;
    }

    public override void StaticBonus(Player player, bool firstSet)
    {
        float count = CountMaterial(player);

        if (count >= 3)
            player.GetModPlayer<SpookyBonusPlayer>().threeSet = true;

        if (count >= 5)
            player.GetModPlayer<SpookyBonusPlayer>().fiveSet = true;
    }

    // Needs 5-Set

    class SpookyBonusPlayer : ModPlayer 
    {
        internal bool threeSet = false;
        internal bool fiveSet = false;
        internal readonly List<DamageHolder> holders = new();

        private float doT = 0;
        private float visualDoT = 0;

        public override void ResetEffects() => fiveSet = threeSet = false;

        public override void UpdateBadLifeRegen()
        {
            if (threeSet)
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;

                Player.lifeRegenCount += 50 + Player.lifeRegen;
            }
        }

        public override void PostUpdate()
        {
            if (holders.Count > 0)
            {
                for (int i = 0; i < holders.Count; i++)
                {
                    DamageHolder item = holders[i];
                    item.time--;
                    doT += item.damage / (10 * 60f);

                    if (item.time <= 0)
                        holders.RemoveAt(i--);
                }

                if (doT > 1)
                    ManuallyHurtPlayer();
            }
        }

        private void ManuallyHurtPlayer()
        {
            int damage = (int)doT;
            doT -= damage;
            visualDoT += damage;
            Player.statLife -= damage;

            if (Player.statLife <= 0)
                Player.KillMe(PlayerDeathReason.ByCustomReason($"{Player.name} wasted away."), damage, 0);

            if (visualDoT >= 5)
            {
                int vDoT = (int)visualDoT;
                CombatText.NewText(new Rectangle((int)Player.position.X, (int)Player.position.Y, Player.width, 4), Color.Red, vDoT);
                visualDoT -= vDoT;
            }
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (fiveSet)
                modifiers.ModifyHurtInfo += OverrideDefaultDamage;
        }

        private void OverrideDefaultDamage(ref Player.HurtInfo info)
        {
            holders.Add(new DamageHolder(info.Damage));
            info.Damage = 1;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) => holders.Clear();

        internal class DamageHolder
        {
            public int damage;
            public int time;

            public DamageHolder(int damage)
            {
                this.damage = damage;
                time = 10 * 60;
            }
        }
    }

    class SpookyGlobalItem : GlobalItem
    {
        public override bool? UseItem(Item item, Player player)
        {
            if (player.GetModPlayer<SpookyBonusPlayer>().fiveSet && item.healLife > 0)
            {
                player.GetModPlayer<SpookyBonusPlayer>().holders.Clear();
                player.AddBuff(BuffID.PotionSickness, item.buffTime);
                return true;
            }

            return null;
        }

        public override void GetHealLife(Item item, Player player, bool quickHeal, ref int healValue)
        {
            if (player.GetModPlayer<SpookyBonusPlayer>().fiveSet && healValue > 0)
                healValue = 1;
        }
    }
}
