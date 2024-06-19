using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class MinorImpureJewelInfo : JewelInfo
{
    public override string Prefix => "Minor";
    public override string JewelTitle => "Impure";
    public override int MaxCuts => 10 + (int)((int)tier / 2f);
    public override bool HasExclusivity => false;

    internal override void InternalSetup() => SubStats = new List<JewelStat>(2);
}
