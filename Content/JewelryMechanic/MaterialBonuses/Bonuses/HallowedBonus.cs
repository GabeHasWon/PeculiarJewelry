﻿using PeculiarJewelry.Content.JewelryMechanic.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Stats;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class HallowedBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Hallowed";

    float bonus = 1f;

    public override bool AppliesToStat(Player player, StatType type) => true;
    public override void SingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1.05f;
    public override void ResetSingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1f;

    public override float EffectBonus(Player player, StatType statType)
    {
        int count = player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);

        if (count >= 1)
            return bonus;
        return 1f;
    }

    public override void StaticBonus(Player player)
    {
        
    }

    // Needs 3-Set, 5-Set
}