using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SoulstoneStats;

internal class AgonyStat(ClassEnum classType = ClassEnum.Invalid) : SoulstoneStat(classType)
{
    public override StatType Type => StatType.SoulAgony;
}
