using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Stats;
using PeculiarJewelry.Content.JewelryMechanic.UI;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Terraria.ModLoader.IO;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

internal class MajorSoulstoneInfo : JewelInfo
{
    public static JewelryStatConfig Con => ModContent.GetInstance<JewelryStatConfig>();

    public override string Prefix => "Major";
    public override string JewelTitle => "Soulstone";
    public override bool HasExclusivity => false;
    public override bool CountsAsMajor => true;

    public ClassEnum ClassType => (Major as SoulstoneContainer).stat.Class;
    public ref float Strength => ref (Major as SoulstoneContainer).Strength;
    public float Cooldown => (Major as SoulstoneContainer).Cooldown;

    public float ColorAdjustment = 1;

    public override void RollSubstats() { }

    internal override void InternalSetup()
    {
        SubStats = [];
        Major = new SoulstoneContainer((StatType)Main.rand.Next((int)StatType.SoulAgony, (int)StatType.SoulMax), 30 * 60) { Strength = 0.3f };
        ColorAdjustment = Main.rand.NextFloat(0.6f, 1);
    }

    internal override void SuccessfulCut()
    {
        successfulCuts++;
        (float min, float max) = BuffStatRange();
        Strength += MathHelper.Lerp(min, max, Main.rand.Next(Con.PowerScaleStepCount) / (Con.PowerScaleStepCount - 1f));
    }

    public override (float, float) BuffStatRange() => (0.01f, 0.02f);

    internal override bool OverrideDisplayColor(out Color color)
    {
        color = ClassType.GetColor(ColorAdjustment);
        return true;
    }

    public override bool ModifyCutLine(TooltipLine line, [NotNullWhen(false)] ref Color? newColor, [NotNullWhen(false)] ref string newText)
    {
        newColor = null;

        // We don't want to modify this stat at all, but it's modified by default as normal Jewels have mods for it
        if (line.Name == "MajorStat")
        {
            newText = line.Text;
            return false;
        }

        if (line.Name != "SoulDebuff")
            return true;

        newColor = CutJewelUIState.MajorStatUpgradeColor;

        float baseStrength = SoulstoneContainer.ConfigModifier(Major.Type) * Strength;
        (float min, float max) = BuffStatRange();
        string displayStrength = "+[c/ffa500:" + ((baseStrength + min) * 100).ToString("#0.#") + " - " + ((baseStrength + max) * 100).ToString("#0.#") + "]";
        newText = Language.GetTextValue("Mods.PeculiarJewelry.Jewelry.SoulstoneDebuff", displayStrength, Major.Type.Localize());
        return false;
    }

    // Set per-jewel info, no need to unset - it's reset in SoulstonePlayer
    protected override void PreApplyTo(Player player, float add, ref float mult) => player.GetModPlayer<SoulstonePlayer>().InfoByStatType[Major.Type].MaxTier = tier;

    internal override void PostAddStatTooltips(List<TooltipLine> tooltips, ModItem modItem, bool displayAsJewel)
    {
        string text = Language.GetTextValue("Mods.PeculiarJewelry.Jewelry.SoulstoneClass", ClassType.GetColor(ColorAdjustment).Hex3(), ClassType.ToString().ToLower());
        tooltips.Insert(3, new TooltipLine(ModContent.GetInstance<PeculiarJewelry>(), "ClassType", text) { OverrideColor = new(100, 100, 100) });

        string displayStrength = (SoulstoneContainer.ConfigModifier(Major.Type) * Strength * 100f).ToString("#0.#");
        string debuff = Language.GetTextValue("Mods.PeculiarJewelry.Jewelry.SoulstoneDebuff", displayStrength, Major.Type.Localize());
        tooltips.Insert(4, new TooltipLine(ModContent.GetInstance<PeculiarJewelry>(), "SoulDebuff", debuff) { OverrideColor = new(100, 100, 100) });
    }

    internal override void SaveData(TagCompound tag)
    {
        tag.Add("class", (short)ClassType);
        tag.Add("color", ColorAdjustment);
        tag.Add("strength", Strength);
        tag.Add("cooldown", Cooldown);
        tag.Add("soul", (byte)Major.Get().Type);
    }

    internal override void LoadData(TagCompound tag)
    {
        ColorAdjustment = tag.GetFloat("color");
        Strength = tag.GetFloat("strength");
        byte soul = tag.GetByte("soul");

        if (soul != 0)
            Major = new SoulstoneContainer((StatType)soul, tag.GetFloat("cooldown"), (ClassEnum)tag.GetShort("class")) { Strength = Strength };
    }
}
