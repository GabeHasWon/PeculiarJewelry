using PeculiarJewelry.Content.Items.Jewels;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Stats;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers;
using System;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class MinorPearlInfo : JewelInfo
{
    public static JewelryStatConfig Config => ModContent.GetInstance<JewelryStatConfig>();

    public override string Prefix => "Minor";
    public override string JewelTitle => "Pearl";
    public override bool HasExclusivity => false;
    public override bool CountsAsMajor => true;
    public override int MaxCuts => 0;

    public TriggerEffect[] effects = new TriggerEffect[1];

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
    }

    internal override bool OverrideDisplayColor(out Color color)
    {
        color = new Color(240, 240, 240);
        return true;
    }

    internal override void SaveData(TagCompound tag)
    {
        for (int i = 0; i < effects.Length; ++i)
        {
            tag.Add("infoTriggerType" + i, effects[i].GetType().AssemblyQualifiedName);
            tag.Add("infoTriggerContext" + i, (byte)effects[i].Context);
        }
    }

    internal override void LoadData(TagCompound tag)
    {
        for (int i = 0; i < effects.Length; ++i)
        {
            effects[i] = Activator.CreateInstance(Type.GetType(tag.GetString("infoTriggerType" + i))) as TriggerEffect;
            byte context = tag.GetByte("infoTriggerContext" + i);
            effects[i].ForceSetContext((TriggerContext)context);
        }
    }
}
