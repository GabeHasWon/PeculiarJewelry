using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

internal class MinorCursedInfo : JewelInfo
{
    public override string Prefix => "Minor";
    public override string JewelTitle => "Cursed";
    public override bool CountsAsMajor => false;

    protected virtual int NegativeDenominator => 2;

    internal override void InternalSetup() => SubStats = new List<JewelStat>(2);

    public override JewelStat GenStat()
    {
        JewelStat stat = new JewelStat(StatType.Legion);// base.GenStat();
        stat.Negative = Main.rand.NextBool(3);
        return stat;
    }

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

            bool canBeNegative = true;

            if (index == SubStats.Count - 1)
                canBeNegative = SubStats.Any(x => !x.Negative);

            if (canBeNegative)
                SubStats[index].Negative = Main.rand.NextBool(NegativeDenominator - index);
        }
        else
        {
            int adjI = index - SubStats.Capacity;
            SubStats[adjI].Strength += JewelryCommon.StatStrengthRange(this);
        }
    }

    protected override void PreApplyTo(Player player, float add, ref float multiplier) => GetModifier(ref multiplier);

    private void GetModifier(ref float multiplier)
    {
        float positive = 0;
        float negative = 0;

        foreach (var item in SubStats)
        {
            if (item.Negative)
                negative -= item.Strength;
            else
                positive += item.Strength;
        }

        if (Major.Negative)
            negative -= Major.Strength;
        else
            positive += Major.Strength;

        multiplier *= negative * -2f / positive + 1;
    }

    internal override bool PreAddStatTooltips(List<TooltipLine> tooltips, ModItem modItem, bool displayAsJewel, ref float modStrength)
    {
        GetModifier(ref modStrength);
        return false;
    }
}
