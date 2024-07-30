using PeculiarJewelry.Content.Items.Jewels;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader.IO;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class MajorPearlInfo : MajorJewelInfo
{
    public static JewelryStatConfig Config => ModContent.GetInstance<JewelryStatConfig>();

    public override string Prefix => "Major";
    public override string JewelTitle => "Pearl";
    public override bool HasExclusivity => false;
    public override bool CountsAsMajor => true;
    public override string Title => effects[0].DisplayName.Value;

    public TriggerEffect[] effects = new TriggerEffect[3];

    public override void InstantTrigger(TriggerContext context, Player player)
    {
        foreach (var effect in effects)
            effect.InstantTrigger(context, player, tier);
    }

    public override void ConstantTrigger(Player player, float bonus)
    {
        foreach (var effect in effects)
            effect.ConstantTrigger(player, tier, bonus);
    }

    public override string TriggerTooltip(Player player)
    {
        string tooltip = "";

        for (int i = 0; i < effects.Length; i++)
        {
            TriggerEffect effect = effects[i];
            tooltip += effect.Tooltip(tier, player) + (i < effects.Length - 1 ? "\n" : "");
        }

        return tooltip;
    }

    internal override bool PreAddStatTooltips(List<TooltipLine> tooltips, ModItem modItem, bool displayAsJewel)
    {
        string[] tips = TriggerTooltip(Main.LocalPlayer).Split('\n');

        foreach (string tip in tips)
            tooltips.Add(new TooltipLine(modItem.Mod, "PearlTriggerEffects", tip));

        return true;
    }

    public override void RollSubstats() { }

    internal override void InternalSetup()
    {
        SubStats = new List<JewelStat>(0);
        Major = new(StatType.None);

        var triggers = ModContent.GetContent<TriggerEffect>().ToList();

        for (int i = 0; i < effects.Length; i++)
            effects[i] = Activator.CreateInstance(Main.rand.Next(triggers).GetType()) as TriggerEffect;
    }

    internal override bool OverrideDisplayColor(out Color color)
    {
        color = new Color(240, 240, 240);
        return true;
    }

    internal override void SuccessfulCut()
    {
        successfulCuts++;

        for (int i = 0; i < effects.Length; ++i)
            effects[i].multiplier += 0.1f * JewelryCommon.DefaultStatRangeFunction();
    }

    internal override void SaveData(TagCompound tag)
    {
        for (int i = 0; i < effects.Length; ++i)
            tag.Add("effect" + i, effects[i].Save());
    }

    internal override void LoadData(TagCompound tag)
    {
        for (int i = 0; i < effects.Length; ++i)
            effects[i] = TriggerEffect.Load(tag.GetCompound("effect" + i));
    }
}
