using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;
using System;

namespace PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;

public class MinorSpectrolite : Jewel
{
    public override LocalizedText ExaminationLocalization => Language.GetText("Mods.PeculiarJewelry.UI.Exam.Help.MinorSpectrolite");
    protected override Type InfoType => typeof(MinorSpectroliteInfo);
    protected override byte MaxVariations => 5;

    public override void Defaults()
    {
        Item.width = 24;
        Item.height = 22;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 6, 0, 0);
    }
}
