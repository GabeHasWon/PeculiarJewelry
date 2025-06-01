using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using Terraria.DataStructures;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class CrimtaneBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Crimtane";

    private float damageBonus = 1f;
    private float bonusStrength = 1.25f;

    public override bool AppliesToStat(Player player, StatType type) =>
        type == StatType.Potency || type == StatType.Might || type == StatType.Order || type == StatType.Precision || type == StatType.Willpower || // Benefits
        type == StatType.Vigor || type == StatType.Renewal; // Reduces

    public override void SingleJewelBonus(Player player, BasicJewelry jewel) => damageBonus = player.GetModPlayer<CatEyePlayer>().GetBonus(MaterialKey, bonusStrength);
    public override void ResetSingleJewelBonus(Player player, BasicJewelry jewel) => damageBonus = 1f;

    public override float EffectBonus(Player player, StatType statType)
    {
        float count = player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);
        
        if (count >= 1)
            return statType is StatType.Vigor or StatType.Renewal ? 1f - player.GetModPlayer<CatEyePlayer>().GetBonus(MaterialKey, 0.06f) : damageBonus;

        return 1f;
    }

    public override void StaticBonus(Player player, bool firstSet)
    {
        float count = player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);
        bonusStrength = 1.25f;

        if (count >= 3)
        {
            player.GetModPlayer<CrimtaneBonusPlayer>().threeSet = true;
            bonusStrength = 1.4f;
        }

        if (count >= 5)
            player.GetModPlayer<CrimtaneBonusPlayer>().fiveSet = true;
    }

    public class CrimtaneBonusPlayer : ModPlayer
    {
        internal bool threeSet = false;
        internal bool fiveSet = false;

        public override void ResetEffects() => threeSet = fiveSet = false;

        public override void UpdateLifeRegen()
        {
            if (threeSet)
            {
                Player.lifeRegen = 0;
                Player.lifeSteal = 0;
            }
        }

        internal void DoMissEffect()
        {
            if (fiveSet && !Player.immune)
            {
                Player.Hurt(PlayerDeathReason.ByCustomReason($"{Player.name} forgot to aim."), Player.statLifeMax2 / 10, 0, false, false, -1, false, 
                    Player.statLifeMax2 / 10);
                Player.AddBuff(ModContent.BuffType<CrimtaneAggressionDebuff>(), 4 * 60);
                Player.SetImmuneTimeForAllTypes(20);
                Player.immune = true;
            }
        }
    }

    internal class CrimtaneAggressionDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) => player.GetDamage(DamageClass.Generic) += 3;
    }
}
