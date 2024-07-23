using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class MinorAncientInfo : MajorAncientInfo
{
    public override string Prefix => "Minor";
    public override bool CountsAsMajor => false;
    public override int MaxCuts => base.MaxCuts / 2;

    internal override void InternalSetup() => SubStats = new List<JewelStat>(2);
}
