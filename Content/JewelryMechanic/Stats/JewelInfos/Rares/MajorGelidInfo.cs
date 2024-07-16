using PeculiarJewelry.Content.Items.Pliers;
using System;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class MajorGelidInfo : JewelInfo
{
    public static JewelryStatConfig Config => ModContent.GetInstance<JewelryStatConfig>();

    public override string Prefix => "Major";
    public override string JewelTitle => "Gelid";
    public override bool CountsAsMajor => true;
    public override int CutDustType => ItemID.Gel;

    internal override void InternalSetup() => SubStats = new List<JewelStat>(4);

    internal override bool OverridePlierAttempt(Plier plier) => true;
    internal override int ModifyCoinPrice(int price) => (int)MathF.Ceiling(price * 0.5f);
    internal override int ModifyDustPrice(int price) => (int)MathF.Ceiling(price * 0.5f);
    internal override float BaseJewelCutChance() => 1f - successfulCuts * 0.02f;

    public override (float, float) BuffStatRange()
    {
        (float min, float max) = base.BuffStatRange();
        return (min / 2, max / 2);
    }

    internal override bool OverrideDisplayColor(out Color color)
    {
        color = new Color(218, 91, 151);
        return true;
    }
}
