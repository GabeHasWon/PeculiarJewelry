using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Stats;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class TungstenBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Tungsten";

    float bonus = 1f;

    public override bool AppliesToStat(Player player, StatType type) => type == StatType.Dexterity;
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
            player.frogLegJumpBoost = true;

        if (count >= 5)
        {
            if (player.controlDown)
                player.maxFallSpeed *= 2;

            if (player.controlUp)
                player.maxFallSpeed = 0.1f;
        }
    }
}
