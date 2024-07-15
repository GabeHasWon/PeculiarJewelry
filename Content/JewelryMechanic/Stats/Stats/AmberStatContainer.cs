using PeculiarJewelry.Content.JewelryMechanic.Stats.Effects;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Stats;

internal class AmberStatContainer() : JewelStat(StatType.Amber)
{
    internal AmberStat stat = null;

    public override JewelStatEffect Get() => stat;
}
