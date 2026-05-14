namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

internal class MinorSpectroliteInfo : JewelInfo
{
    public override string Prefix => "Minor";
    public override string JewelTitle => "Spectrolite";
    public override bool HasExclusivity => false;
    public override bool CountsAsMajor => false;

    internal override void InternalSetup() => SubStats = [];

    public override JewelStat GenStat() => Major is not null ? Major : new JewelStat((StatType)Main.rand.Next((int)StatType.SoulMax + 1, (int)StatType.SpectroliteMax))
    {
        Strength = 0.5f
    };
}
