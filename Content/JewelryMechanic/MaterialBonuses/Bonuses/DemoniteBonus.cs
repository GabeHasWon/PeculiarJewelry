using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class DemoniteBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Demonite";

    private float damageBonus = 1f;
    private float bonusStrength = 1.25f;

    public override bool AppliesToStat(Player player, StatType type) => 
        type == StatType.Potency || type == StatType.Might || type == StatType.Order || type == StatType.Precision || type == StatType.Willpower || // Benefits
        type == StatType.Permenance || type == StatType.Tenacity; // Reduces

    public override void SingleJewelBonus(Player player, BasicJewelry jewel) => damageBonus = bonusStrength;
    public override void ResetSingleJewelBonus(Player player, BasicJewelry jewel) => damageBonus = 1f;

    public override float EffectBonus(Player player, StatType statType)
    {
        float count = player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);
        
        if (count >= 1)
            return statType is StatType.Permenance or StatType.Tenacity ? 0.94f : damageBonus;

        return 1f;
    }

    public override void StaticBonus(Player player, bool firstSet)
    {
        float count = player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);
        bonusStrength = 1.25f;

        if (count >= 3)
        {
            player.GetModPlayer<DemoniteBonusPlayer>().threeSet = true;
            bonusStrength = 1.4f;
        }

        if (count >= 5)
            player.GetModPlayer<DemoniteBonusPlayer>().fiveSet = true;
    }

    internal class DemoniteBonusPlayer : ModPlayer
    {
        internal bool threeSet = false;
        internal bool fiveSet = false;
        internal bool missedMeleeSwing = false;

        public override void ResetEffects() => threeSet = fiveSet = false;

        public override bool CanUseItem(Item item)
        {
            if (!item.noMelee && item.damage > 0)
                missedMeleeSwing = true;

            return true;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) => missedMeleeSwing = false;

        public override void PostUpdate()
        {
            if (Player.itemAnimation == 1 & missedMeleeSwing)
            {
                DoMissEffect();
                Player.GetModPlayer<CrimtaneBonus.CrimtaneBonusPlayer>().DoMissEffect();
            }
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (threeSet)
                modifiers.FinalDamage *= 1.4f;
        }

        internal void DoMissEffect()
        {
            if (fiveSet)
                Player.AddBuff(ModContent.BuffType<DemoniteAggressionDebuff>(), 4 * 60);
        }
    }

    internal class DemoniteAggressionDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense *= 0;
            player.endurance = 0;
            player.GetDamage(DamageClass.Generic) += 2f;
        }
    }
}
