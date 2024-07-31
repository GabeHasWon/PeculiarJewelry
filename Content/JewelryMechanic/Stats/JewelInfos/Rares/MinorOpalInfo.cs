using PeculiarJewelry.Content.JewelryMechanic.UI;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class MinorOpalInfo : JewelInfo
{
    public override string Prefix => "Minor";
    public override bool CountsAsMajor => false;

    internal override void InternalSetup()
    {
        Major = new JewelStat(StatType.None);
        SubStats = new List<JewelStat>(4);
        AddSubStat([], 0);
    }
}
