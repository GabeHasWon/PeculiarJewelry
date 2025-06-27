using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

internal class MajorCursedInfo : MinorCursedInfo
{
    public override string Prefix => "Major";
    public override bool CountsAsMajor => true;

    protected override int NegativeDenominator => 4;

    internal override void InternalSetup() => SubStats = new List<JewelStat>(4);

    public override JewelStat GenStat()
    {
        JewelStat stat = new JewelStat(StatType.Allure); //base.GenStat();
        stat.Negative = Main.rand.NextBool(5);
        return stat;
    }
}
