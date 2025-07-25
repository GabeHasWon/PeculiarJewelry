using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SoulstoneStats;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Stats;
using System;
using System.Collections.Generic;
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
        Major = new SoulstoneContainer((StatType)Main.rand.Next((int)StatType.SoulAgony, (int)StatType.SoulMax));
        ClassType = (ClassEnum)Main.rand.Next(5);
        ColorAdjustment = Main.rand.NextFloat(0.6f, 1);
    }

    internal override bool OverrideDisplayColor(out Color color)
    {
        color = GetClassColor(ClassType, ColorAdjustment);
        return true;
    }

    public static Color GetClassColor(ClassEnum classType, float adjustment) => classType switch
    {
        ClassEnum.Melee => Color.Lerp(Color.Red, Color.IndianRed, adjustment),
        ClassEnum.Ranged => Color.Lerp(Color.CadetBlue, Color.DarkSlateBlue, adjustment),
        ClassEnum.Magic => Color.Lerp(Color.Yellow, Color.DarkOrange, adjustment),
        ClassEnum.Summoner => Color.Lerp(Color.Green, Color.DarkOliveGreen, adjustment),
        ClassEnum.Generic => Color.Lerp(Color.White, Color.DarkSlateGray, adjustment),
        _ => throw new ArgumentException("ClassType exceeds the options for some reason. (MajorSoulstoneInfo OverrideDisplayColor)")
    };

    protected override void PreApplyTo(Player player, float add, ref float mult)
    {
        // Set per-jewel info, no need to unset
        player.GetModPlayer<SoulstonePlayer>().InfoByInfo[Major.Type].MaxTier = tier;
        player.GetModPlayer<SoulstonePlayer>().InfoByInfo[Major.Type].Class = ClassType;
    }

    internal override void PostAddStatTooltips(List<TooltipLine> tooltips, ModItem modItem, bool displayAsJewel)
    {
        string text = $"Ghosts count as [c/{GetClassColor(ClassType, ColorAdjustment).Hex3()}:{ClassType.ToString().ToLower()} damage]";
        tooltips.Add(new TooltipLine(ModContent.GetInstance<PeculiarJewelry>(), "ClassType", text) { OverrideColor = new(93, 93, 93) });
    }

    internal override void SaveData(TagCompound tag)
    {
        tag.Add("class", (byte)ClassType);
        tag.Add("color", ColorAdjustment);
        tag.Add("soul", (byte)Major.Get().Type);
    }

    internal override void LoadData(TagCompound tag)
    {
        ClassType = (ClassEnum)tag.GetByte("class");
        ColorAdjustment = tag.GetFloat("color");

        byte soul = tag.GetByte("soul");

        if (soul != 0)
            Major = new SoulstoneContainer((StatType)soul);
    }
}
