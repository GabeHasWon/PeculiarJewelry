using PeculiarJewelry.Content.Items.Pliers;
using System;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class MajorAncientInfo : JewelInfo
{
    public override string Prefix => "Major";
    public override string JewelTitle => "Ancient";
    public override bool CountsAsMajor => true;
    public override int MaxCuts => base.MaxCuts / 2;

    internal override void InternalSetup() => SubStats = new List<JewelStat>(4);
    internal override float BaseJewelCutChance() => 0.5f - successfulCuts * 0.05f;

    internal override void SuccessfulCut()
    {
        successfulCuts++;

        for (int i = 0; i < 2; ++i)
            Major.Strength += JewelryCommon.StatStrengthRange(this);

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

    internal override bool OverrideDisplayColor(out Color color)
    {
        color = new Color(218, 91, 151);
        return true;
    }
}
