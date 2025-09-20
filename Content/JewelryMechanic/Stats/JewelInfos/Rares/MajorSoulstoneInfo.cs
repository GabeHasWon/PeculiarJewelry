using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SoulstoneStats;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Stats;
using System;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

internal class MajorSoulstoneInfo : JewelInfo
{
    public static JewelryStatConfig Config => ModContent.GetInstance<JewelryStatConfig>();

    public override string Prefix => "Major";
    public override string JewelTitle => "Soulstone";
    public override bool HasExclusivity => false;
    public override bool CountsAsMajor => true;
    public override int MaxCuts => 0;

    public ClassEnum ClassType => (Major as SoulstoneContainer).stat.Class;

    public float ColorAdjustment = 1;

    public override void RollSubstats() { }

    internal override void InternalSetup()
    {
        SubStats = [];
        Major = new SoulstoneContainer((StatType)Main.rand.Next((int)StatType.SoulAgony, (int)StatType.SoulMax));
        ColorAdjustment = Main.rand.NextFloat(0.6f, 1);
    }

    internal override bool OverrideDisplayColor(out Color color)
    {
        color = ClassType.GetColor(ColorAdjustment);
        return true;
    }

    protected override void PreApplyTo(Player player, float add, ref float mult)
    {
        // Set per-jewel info, no need to unset
        player.GetModPlayer<SoulstonePlayer>().InfoByInfo[Major.Type].MaxTier = tier;
    }

    internal override void PostAddStatTooltips(List<TooltipLine> tooltips, ModItem modItem, bool displayAsJewel)
    {
        string text = $"Ghosts count as [c/{ClassType.GetColor(ColorAdjustment).Hex3()}:{ClassType.ToString().ToLower()} damage]";
        tooltips.Add(new TooltipLine(ModContent.GetInstance<PeculiarJewelry>(), "ClassType", text) { OverrideColor = new(93, 93, 93) });
    }

    internal override void SaveData(TagCompound tag)
    {
        tag.Add("class", (short)ClassType);
        tag.Add("color", ColorAdjustment);
        tag.Add("soul", (byte)Major.Get().Type);
    }

    internal override void LoadData(TagCompound tag)
    {
        ColorAdjustment = tag.GetFloat("color");
        byte soul = tag.GetByte("soul");

        if (soul != 0)
            Major = new SoulstoneContainer((StatType)soul, (ClassEnum)tag.GetShort("class"));
    }
}
