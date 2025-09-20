using PeculiarJewelry.Content.JewelryMechanic.Misc;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SoulstoneStats;

internal class GriefStat(ClassEnum classType = ClassEnum.Invalid) : SoulstoneStat(classType)
{
    public override StatType Type => StatType.SoulGrief;
}
