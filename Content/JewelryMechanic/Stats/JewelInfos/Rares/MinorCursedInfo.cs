using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

internal class MinorCursedInfo : JewelInfo
{
    public override string Prefix => "Minor";
    public override string JewelTitle => "Cursed";
    public override bool CountsAsMajor => false;

    internal override void InternalSetup() => SubStats = new List<JewelStat>(4);

    protected override void AddSubStat(List<StatType> takenTypes, int index)
    {
        if (index < SubStats.Capacity) // Fill slots
        {
            SubStats.Add(JewelStat.Random);

            if (HasExclusivity)
            {
                while (SubStats[index].Get().Exclusivity != exclusivity && SubStats[index].Get().Exclusivity != StatExclusivity.None || takenTypes.Contains(SubStats[index].Get().Type))
                    SubStats[index] = JewelStat.Random;
            }

            takenTypes.Add(SubStats[index].Get().Type);

            if (HasExclusivity && exclusivity == StatExclusivity.None)
                exclusivity = SubStats[index].Get().Exclusivity;

            SubStats[index].Negative = Main.rand.NextBool(4 - index);
        }
        else
        {
            int adjI = index - SubStats.Capacity;
            SubStats[adjI].Strength += JewelryCommon.StatStrengthRange(this);
        }
    }

    protected override void PreApplyTo(Player player, float add, ref float multiplier)
    {
        float positive = Major.Strength;
        float negative = 0;

        foreach (var item in SubStats)
        {
            if (item.Negative)
                negative += item.Strength;
            else
                positive += item.Strength;
        }

        multiplier *= negative * -2f / (positive + 1);
    }
}
