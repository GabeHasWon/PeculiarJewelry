using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;
using System;

namespace PeculiarJewelry.Content.Items.Jewels.Rares.Impure;

public class ImpureMinor : ImpureMajor
{
    public override LocalizedText ExaminationLocalization => Language.GetText("Mods.PeculiarJewelry.UI.Exam.Help.ImpureMinor");
    protected override Type InfoType => typeof(MinorImpureJewelInfo);
    protected override byte MaxVariations => 5;

    public sealed override void Defaults()
    {
        Item.width = 24;
        Item.height = 22;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 15, 0, 0);
    }
}
