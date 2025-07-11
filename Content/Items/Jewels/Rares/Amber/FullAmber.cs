using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;
using System;

namespace PeculiarJewelry.Content.Items.Jewels.Rares.Amber;

public class FullAmber : PureAmber
{
    public override LocalizedText ExaminationLocalization => Language.GetText("Mods.PeculiarJewelry.UI.Exam.Help.FullAmber");
    protected override Type InfoType => typeof(FullAmberInfo);
    public override string Texture => base.Texture.Replace("Full", "Pure");
}
