using PeculiarJewelry.Content.JewelryMechanic.Stats.Stats;
using System;
using Terraria.ModLoader.IO;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

internal class MajorSoulstoneInfo : JewelInfo
{
    public enum ClassEnum : byte
    {
        Melee,
        Ranged,
        Magic,
        Summoner,
        Generic
    }

    public static JewelryStatConfig Config => ModContent.GetInstance<JewelryStatConfig>();

    public override string Prefix => "Major";
    public override string JewelTitle => "Soulstone";
    public override bool HasExclusivity => false;
    public override bool CountsAsMajor => true;
    public override int MaxCuts => 0;

    public ClassEnum ClassType = ClassEnum.Generic;
    public float ColorAdjustment = 1;

    public override void RollSubstats() { }

    internal override void InternalSetup()
    {
        SubStats = [];
        Major = new JewelStat((StatType)Main.rand.Next((int)StatType.SoulAgony, (int)StatType.SoulMax));
        ClassType = (ClassEnum)Main.rand.Next(5);
        ColorAdjustment = Main.rand.NextFloat(0.6f, 1);
    }

    internal override bool OverrideDisplayColor(out Color color)
    {
        color = ClassType switch
        {
            ClassEnum.Melee => Color.Red,//Main.hslToRgb(206 / 255f, 1, ),
            ClassEnum.Ranged => Color.Blue,//(new Color(79, 109, 255) * ColorAdjustment) with { A = 255 },
            ClassEnum.Magic => Color.Yellow,// (new Color(255, 236, 48) * ColorAdjustment) with { A = 255 },
            ClassEnum.Summoner => Color.Green,
            ClassEnum.Generic => Color.White,
            _ => throw new ArgumentException("ClassType exceeds the options for some reason. (MajorSoulstoneInfo OverrideDisplayColor)")
        };

        return true;
    }

    internal override void SaveData(TagCompound tag) => tag.Add("class", (byte)ClassType);
    internal override void LoadData(TagCompound tag) => ClassType = (ClassEnum)tag.GetByte("class");
}
