using PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Stats;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class FullAmberInfo : JewelInfo
{
    public static JewelryStatConfig Config => ModContent.GetInstance<JewelryStatConfig>();

    public override string Prefix => "Full";
    public override string JewelTitle => "Amber";
    public override bool HasExclusivity => false;
    public override bool CountsAsMajor => true;
    public override int MaxCuts => 0;

    private Item accessory = new Item(ItemID.HermesBoots);

    public override void RollSubstats() { }

    internal override void InternalSetup()
    {
        SubStats = new List<JewelStat>(0);
        Major = new AmberStatContainer()
        {
            stat = new AmberStat()
            {
                accessory = new Item(AmberAccessoryPool.Get()),
            }
        };
    }

    protected override void PreApplyTo(Player player, float add, float multiplier) => (Major.Get() as AmberStat).accessory = accessory;
    protected override void PostApplyTo(Player player, float add, float multiplier) => (Major.Get() as AmberStat).accessory = null;

    internal override bool OverrideDisplayColor(out Color color)
    {
        color = new Color(217, 110, 4);
        return true;
    }
}
