using PeculiarJewelry.Content.Buffs;
using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Stats;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class IronBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Iron";

    float bonus = 1f;

    public override bool AppliesToStat(Player player, StatType type) => type == StatType.Permenance;
    public override void SingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1.15f;
    public override void ResetSingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1f;

    public override float EffectBonus(Player player, StatType statType)
    {
        float count = player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);
        return count >= 1 ? bonus : 1f;
    }

    public override void StaticBonus(Player player, bool firstSet)
    {
        float count = player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);

        if (count >= 3)
            player.GetModPlayer<IronBonusPlayer>().threeSet = true;
    }

    internal class IronBonusPlayer : ModPlayer
    {
        internal bool threeSet;
        internal bool fiveSet;
        internal int extraDefense;
        internal int maxBuffTime;
        internal int effectiveExtraDefense;

        public override void ResetEffects()
        {
            threeSet = fiveSet = false;

            if (!Player.HasBuff<Iron5SetDefenseBonus>())
            {
                extraDefense = 0;
                maxBuffTime = 0;
            }
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers) => modifiers.ModifyHurtInfo += ModDamage;

        private void ModDamage(ref Player.HurtInfo info)
        {
            if (threeSet && info.Damage <= Player.statDefense * 2)
                info.Damage = (int)(info.Damage * 0.5f);
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (!fiveSet || info.Damage < 5)
                return;

            extraDefense += info.Damage / 5;
            maxBuffTime = 5 * 60; // Set it to 5 seconds as a default
            Player.AddBuff(ModContent.BuffType<Iron5SetDefenseBonus>(), 5 * 60);
        }
    }
}
