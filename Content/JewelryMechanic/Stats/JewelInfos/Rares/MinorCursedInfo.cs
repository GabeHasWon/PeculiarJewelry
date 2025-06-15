using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

internal class MinorCursedInfo : JewelInfo
{
    public override string Prefix => "Minor";
    public override string JewelTitle => "Cursed";
    public override bool CountsAsMajor => false;

    internal override void InternalSetup() => SubStats = new List<JewelStat>(4);
}
