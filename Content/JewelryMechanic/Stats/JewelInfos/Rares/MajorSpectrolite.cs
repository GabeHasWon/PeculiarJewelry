namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

internal class MajorSpectroliteInfo : JewelInfo
{
    public static JewelryStatConfig Config => ModContent.GetInstance<JewelryStatConfig>();

    public override string Prefix => "Major";
    public override string JewelTitle => "Spectrolite";
    public override bool HasExclusivity => false;
    public override bool CountsAsMajor => true;

    internal override void InternalSetup() => SubStats = [];

    public override JewelStat GenStat() => Major is not null ? Major : new JewelStat((StatType)Main.rand.Next((int)StatType.SoulMax + 1, (int)StatType.SpectroliteMax));
}
