using PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;
using System;
using Terraria.ModLoader.IO;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

internal class MajorSpectroliteInfo : JewelInfo
{
    public override string Prefix => "Major";
    public override string JewelTitle => "Spectrolite";
    public override bool HasExclusivity => false;
    public override bool CountsAsMajor => true;

    internal override void InternalSetup() => SubStats = [];

    public override JewelStat GenStat() 
        => Major is not null ? Major : new SpectroliteContainer((StatType)Main.rand.Next((int)StatType.SoulMax + 1, (int)StatType.SpectroliteMax), true, Guid.NewGuid())
    {
        Strength = 1,
    };

    internal override void LoadData(TagCompound tag)
    {
        float str = Major.Strength;
        var type = Major.Type;
        Guid guid = Guid.NewGuid();
        
        Major = new SpectroliteContainer(type, true, guid) 
        {
            Strength = str 
        };
    }
}
