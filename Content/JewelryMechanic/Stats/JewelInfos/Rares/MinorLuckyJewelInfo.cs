using PeculiarJewelry.Content.Items.Pliers;
using System;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class MinorLuckyJewelInfo : JewelInfo
{
    public static JewelryStatConfig Config => ModContent.GetInstance<JewelryStatConfig>();

    public override string Prefix => "Minor";
    public override string JewelTitle => "Impure";
    public override int MaxCuts => 10 + (int)((int)tier / 2f);
    public override bool HasExclusivity => false;

    internal override void InternalSetup() => SubStats = new List<JewelStat>(2);

    protected override void PostApplyTo(Player player, float add, float multiplier) => player.luck += 0.1f;
    internal override void PostAddStatTooltips(List<TooltipLine> tooltips, JewelInfo info, ModItem modItem)
        => tooltips.Add(new TooltipLine(ModContent.GetInstance<PeculiarJewelry>(), "LuckBonus", Language.GetTextValue("Mods.PeculiarJewelry.Jewels.Misc.MinorLuckBonus")));
    internal override bool OverridePlierAttempt(Plier plier) => true;
    internal override int ModifyCoinPrice(int price) => (int)MathF.Ceiling(price * 0.8f);
    internal override int ModifyDustPrice(int price) => (int)MathF.Ceiling(price * 0.8f);

    internal override bool OverrideDisplayColor(out Color color)
    {
        color = new Color(57, 143, 41);
        return true;
    }

    public override bool PreBuffStat(out float result)
    {
        result = GetStatRange(Main.rand.Next(1, Config.PowerScaleStepCount + 1));
        return true;
    }

    private static float GetStatRange(int step)
    {
        float result = JewelryCommon.StatRangeFunction(step / Config.PowerScaleStepCount - 1);
        return result;
    }

    public override (float, float) BuffStatRange() => (GetStatRange(1), GetStatRange(Config.PowerScaleStepCount));
}
