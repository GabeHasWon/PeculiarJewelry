using PeculiarJewelry.Content.JewelryMechanic.UI;
using rail;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class MajorOpalInfo : JewelInfo
{
    public override string Prefix => "Major";
    public override string JewelTitle => "Opal";
    public override bool CountsAsMajor => true;
    public override string Title => SubStats[0].GetName().Value;
    public override bool IgnoreSubstatUpgrade => true;

    internal override void InternalSetup()
    {
        Major = new JewelStat(StatType.None);
        SubStats = new List<JewelStat>(8);
        AddSubStat([], 0);
    }

    internal override void SuccessfulCut()
    {
        successfulCuts++;

        for (int i = 0; i < 2; ++i)
        {
            if (SubStats.Count == SubStats.Capacity)
                Main.rand.Next(SubStats).Strength += JewelryCommon.StatStrengthRange(this);
            else
            {
                List<StatType> takenTypes = [Major.Type];

                foreach (var item in SubStats)
                    takenTypes.Add(item.Type);

                AddSubStat(takenTypes, SubStats.Count);
            }
        }
    }

    public override void AddCutLines(List<TooltipLine> lines, bool hoveringAnvil)
    {
        if (hoveringAnvil)
        {
            string upgradeOrAdd = (SubStats.Count == SubStats.Capacity ? CutJewelUIState.Localize("Upgraded") : CutJewelUIState.Localize("Added"));
            string text = Language.GetTextValue("Mods.PeculiarJewelry.Items.MajorOpal.SubstatOverrideLine") + upgradeOrAdd;
            lines.Add(new TooltipLine(ModContent.GetInstance<PeculiarJewelry>(), "SubstatUpgrade", text));
        }
    }

    internal override bool OverrideDisplayColor(out Color color)
    {
        color = new Color(42, 136, 255);
        return true;
    }
}
