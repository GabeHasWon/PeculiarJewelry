using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

internal class FullAmberInfo : PureAmberInfo
{
    public override string Prefix => "Full";

    internal override void InternalSetup()
    {
        SubStats = new List<JewelStat>(0);
        Major = GetContainerForItem(AmberAccessoryPool.Get());
    }
}
