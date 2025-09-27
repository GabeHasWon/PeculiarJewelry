using PeculiarJewelry.Content.JewelryMechanic.Stats.Stats;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

internal class MinorSoulstoneInfo : MajorSoulstoneInfo
{
    public override string Prefix => "Minor";
    public override string JewelTitle => "Soulstone";
    public override bool CountsAsMajor => false;

    public override void RollSubstats() { }

    internal override void InternalSetup()
    {
        SubStats = [];
        Major = new SoulstoneContainer((StatType)Main.rand.Next((int)StatType.SoulAgony, (int)StatType.SoulMax), 60 * 60) { Strength = 0.1f };
        ColorAdjustment = Main.rand.NextFloat(0.6f, 1);
    }

    public override (float, float) BuffStatRange() => (0.004f, 0.008f);
}
