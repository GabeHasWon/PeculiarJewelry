using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;
using System;

namespace PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;

public class MajorSpectrolite : Jewel
{
    public override LocalizedText ExaminationLocalization => Language.GetText("Mods.PeculiarJewelry.UI.Exam.Help.MajorSpectrolite");
    protected override Type InfoType => typeof(MajorSpectroliteInfo);
    protected override byte MaxVariations => 3;

    public override void Defaults()
    {
        Item.width = 42;
        Item.height = 40;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 60, 0, 0);
    }
}
