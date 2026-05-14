using PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;
using Terraria.ModLoader.IO;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

internal class MinorSpectroliteInfo : JewelInfo
{
    public override string Prefix => "Minor";
    public override string JewelTitle => "Spectrolite";
    public override bool HasExclusivity => false;
    public override bool CountsAsMajor => false;

    internal override void InternalSetup() => SubStats = [];

    public override JewelStat GenStat() => Major is not null ? Major : new SpectroliteContainer((StatType)Main.rand.Next((int)StatType.SoulMax + 1, (int)StatType.SpectroliteMax), false)
    {
        Strength = 0.5f
    };

    internal override void LoadData(TagCompound tag)
    {
        float str = Major.Strength;
        var type = Major.Type;

        Major = new SpectroliteContainer(type, false)
        {
            Strength = str
        };
    }
}
