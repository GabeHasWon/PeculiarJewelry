﻿using PeculiarJewelry.Content.Items.Jewels;
using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses;
using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

internal class MinorCatEyeInfo : JewelInfo
{
    public override string Prefix => "Minor";
    public override string JewelTitle => "CatEye";

    public override string Name
    {
        get
        {
            string of = Language.GetTextValue("Mods.PeculiarJewelry.Jewels.Of");
            string text = $"{Jewel.Localize("Jewels.Prefixes." + Prefix)} {tier.Localize()} {Jewel.Localize("Jewels.Titles." + JewelTitle)}{of}{MaterialName()} {Title}";

            if (Major.Strength > 1)
                text += $" +{successfulCuts}";

            return text;
        }
    }

    public string MaterialBonus = "Copper";

    private string MaterialName() => Language.GetTextValue("Mods.PeculiarJewelry.Material.Bonuses." + MaterialBonus + ".Name");

    public override void RollSubstats() { }

    internal override void InternalSetup()
    {
        SubStats = [];
        MaterialBonus = Main.rand.Next(JewelryCommon.GetAllUnlockedMaterials());
    }

    protected override void PreApplyTo(Player player, float add, ref float multiplier) => player.GetModPlayer<MaterialPlayer>().AddMaterial(MaterialBonus, 0.5f);

    internal override bool PreAddStatTooltips(List<TooltipLine> tooltips, ModItem modItem, bool displayAsJewel, ref float modStrength)
    {
        tooltips.Add(new TooltipLine(modItem.Mod, "CatMaterialBonus", Language.GetTextValue("Mods.PeculiarJewelry.Jewelry.CatEyeHalf", MaterialBonus)));
        return false;
    }
}
