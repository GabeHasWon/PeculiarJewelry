using PeculiarJewelry.Content.Items.Jewels;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Stats;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class PureAmberInfo : JewelInfo
{
    public static JewelryStatConfig Config => ModContent.GetInstance<JewelryStatConfig>();

    public override string Prefix => "Pure";
    public override string JewelTitle => "Amber";
    public override bool HasExclusivity => false;
    public override bool CountsAsMajor => true;
    public override int MaxCuts => 0;

    public override string Name
    {
        get
        {
            string of = Language.GetTextValue("Mods.PeculiarJewelry.Jewels.Of");
            string text = $"{Jewel.Localize("Jewels.Prefixes." + Prefix)} {tier.Localize()} {Jewel.Localize("Jewels.Titles." + JewelTitle)}{of}{GetAmberStatName()}";

            if (Major.Strength > 1)
                text += $" +{successfulCuts}";

            return text;
        }
    }

    private string GetAmberStatName() => Lang.GetItemNameValue((Major as AmberStatContainer).stat.accessory.type);

    public override void RollSubstats() { }

    internal override void InternalSetup()
    {
        SubStats = new List<JewelStat>(0);
        Major = GetContainerForItem(ItemID.None);
    }

    public static AmberStatContainer GetContainerForItem(int id) => new AmberStatContainer()
    {
        stat = new AmberStat()
        {
            accessory = new Item(id),
        }
    };

    internal override bool OverrideDisplayColor(out Color color)
    {
        color = new Color(217, 110, 4);
        return true;
    }

    internal override void SaveData(TagCompound tag) => tag.Add("amberItemId", (Major as AmberStatContainer).stat.accessory.type);

    internal override void LoadData(TagCompound tag)
    {
        int id = tag.GetInt("amberItemId");
        Major = GetContainerForItem(id);
    }
}
