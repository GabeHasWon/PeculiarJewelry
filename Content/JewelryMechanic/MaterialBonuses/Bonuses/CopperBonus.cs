using PeculiarJewelry.Content.Buffs;
using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Stats;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class CopperBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Copper";

    float bonus = 1f;

    public override bool AppliesToStat(Player player, StatType type) => type == StatType.Vigor;
    public override void SingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1.15f;
    public override void ResetSingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1f;

    public override float EffectBonus(Player player, StatType statType)
    {
        float copperCount = player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);
        return copperCount >= 1 ? bonus : 1f;
    }

    public override void StaticBonus(Player player, bool firstSet)
    {
        float copperCount = CountMaterial(player);

        if (copperCount >= 3 && firstSet)
            player.statLifeMax2 += player.statDefense;

        if (copperCount >= 5)
            player.GetModPlayer<CopperBonusPlayer>().fiveSet = true;
    }

    public class CopperBonusPlayer : ModPlayer
    {
        public bool fiveSet = false;

        public override void ResetEffects() => fiveSet = false;
    }

    public class CopperGlobalItem : GlobalItem
    {
        public override bool? UseItem(Item item, Player player)
        {
            if (player.GetModPlayer<CopperBonusPlayer>().fiveSet) // Force use of life items to gain health
            {
                if (item.type == ItemID.LifeCrystal && !player.HasBuff<LifeCrystalCopper5SetCooldown>())
                {
                    player.Heal(player.statLifeMax2 / 2);
                    player.AddBuff(ModContent.BuffType<LifeCrystalCopper5SetCooldown>(), 60 * 60 * 2);
                    return true;
                }

                if (item.type == ItemID.LifeFruit && !player.HasBuff<LifeFruitCopper5SetCooldown>())
                {
                    player.Heal(player.statLifeMax2 / 10);
                    player.AddBuff(ModContent.BuffType<LifeFruitCopper5SetCooldown>(), 60 * 60 * 2);
                    return true;
                }
            }

            return null;
        }
    }
}
