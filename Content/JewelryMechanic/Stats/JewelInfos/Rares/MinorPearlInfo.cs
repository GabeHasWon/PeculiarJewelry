using PeculiarJewelry.Content.Items.Jewels;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class MinorPearlInfo : MajorPearlInfo
{
    public override string Prefix => "Minor";
    public override string JewelTitle => "Pearl";
    public override bool HasExclusivity => false;
    public override bool CountsAsMajor => false;

    public override string Name
    {
        get
        {
            string of = Language.GetTextValue("Mods.PeculiarJewelry.Jewels.Of");
            string text = $"{Jewel.Localize("Jewels.Prefixes." + Prefix)} {tier.Localize()} {Jewel.Localize("Jewels.Titles." + JewelTitle)}{of}{effects[0].DisplayName}";

            if (Major.Strength > 1)
                text += $" +{successfulCuts}";

            return text;
        }
    }

    public override void RollSubstats() { }

    internal override void InternalSetup()
    {
        SubStats = new List<JewelStat>(0);
        Major = new(StatType.None);

        effects = new TriggerEffect[1];
        var triggers = ModContent.GetContent<TriggerEffect>().ToList();

        for (int i = 0; i < effects.Length; i++)
            effects[i] = Activator.CreateInstance(Main.rand.Next(triggers).GetType()) as TriggerEffect;
    }
}
