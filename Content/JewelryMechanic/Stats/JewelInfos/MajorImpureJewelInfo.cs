using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class MajorImpureJewelInfo : JewelInfo
{
    public override string Prefix => "Major";
    public override string JewelTitle => "Impure";
    public override bool HasExclusivity => false;

    internal override void InternalSetup() => SubStats = new List<JewelStat>(4);
}
