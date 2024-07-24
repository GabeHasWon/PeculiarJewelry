using PeculiarJewelry.Content.Items.Pliers;
using System;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class MajorLuckyJewelInfo : JewelInfo
{
    public static JewelryStatConfig Config => ModContent.GetInstance<JewelryStatConfig>();

    public override string Prefix => "Major";
    public override string JewelTitle => "Lucky";
    public override bool HasExclusivity => false;
    public override bool CountsAsMajor => true;

    internal override void InternalSetup() => SubStats = new List<JewelStat>(4);

    protected override void PostApplyTo(Player player, float add, float multiplier) => player.luck += 0.2f;
    internal override void PostAddStatTooltips(List<TooltipLine> tooltips, ModItem modItem, bool displayAsJewel) 
        => tooltips.Add(new TooltipLine(ModContent.GetInstance<PeculiarJewelry>(), "LuckBonus", Language.GetTextValue("Mods.PeculiarJewelry.Jewels.Misc.MajorLuckBonus")));
    internal override bool OverridePlierAttempt(Plier plier) => true;
    internal override int ModifyCoinPrice(int price) => (int)MathF.Ceiling(price * 0.8f);
    internal override int ModifyDustPrice(int price) => (int)MathF.Ceiling(price * 0.8f);
    internal override float BaseJewelCutChance() => 1f - successfulCuts * 0.035f;

    internal override bool OverrideDisplayColor(out Color color)
    {
        color = new Color(57, 143, 41);
        return true;
    }

    public override bool PreBuffStat(out float result)
    {
        result = MathF.Max(JewelryCommon.DefaultStatRangeFunction(), JewelryCommon.DefaultStatRangeFunction());
        return true;
    }
}
