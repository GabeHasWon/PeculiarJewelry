using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class MajorMoonstoneInfo : JewelInfo
{
    public static JewelryStatConfig Config => ModContent.GetInstance<JewelryStatConfig>();

    public override string Prefix => "Major";
    public override string JewelTitle => "Moonstone";
    public override bool HasExclusivity => false;
    public override bool CountsAsMajor => true;

    internal override void InternalSetup() => SubStats = new List<JewelStat>(1) { new(Major.Type) };

    public override JewelStat GenStat() => Major is not null ? Major : new JewelStat((StatType)Main.rand.Next((int)StatType.BasicMax + 1, (int)StatType.MaxMoonstone));

    internal override bool OverrideDisplayColor(out Color color)
    {
        color = new Color(153, 153, 191);
        return true;
    }
}
