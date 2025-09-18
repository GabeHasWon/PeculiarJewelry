using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SoulstoneStats;

internal class PlagueStat(ClassEnum classType = ClassEnum.Invalid) : SoulstoneStat(classType)
{
    public override StatType Type => StatType.SoulPlague;
}
