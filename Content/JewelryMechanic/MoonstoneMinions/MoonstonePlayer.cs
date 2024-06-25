using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.MoonstoneMinions;

internal class MoonstonePlayer : ModPlayer
{
    public Dictionary<StatType, int> DesiredProjectileCountByStatType = [];

    public override void ResetEffects()
    {
        foreach (var item in DesiredProjectileCountByStatType)
            DesiredProjectileCountByStatType[item.Key] = 0;
    }

    public override void PostUpdateEquips()
    {

    }
}
