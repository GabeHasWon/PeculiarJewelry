using PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader.IO;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats;

internal class MajorJewelInfo : JewelInfo
{
    internal TriggerEffect effect;

    public override string Prefix => "Major";
    public override bool CountsAsMajor => true;

    internal override void InternalSetup()
    {
        SubStats = new System.Collections.Generic.List<JewelStat>(4);
        effect = Activator.CreateInstance(Main.rand.Next(ModContent.GetContent<TriggerEffect>().ToList()).GetType()) as TriggerEffect;
    }

    public void InstantTrigger(TriggerContext context, Player player) => effect.InstantTrigger(context, player, tier);
    public void ConstantTrigger(Player player, float bonus) => effect.ConstantTrigger(player, tier, bonus);
    public string TriggerTooltip(Player player) => effect.Tooltip(tier, player);

    internal override bool PreAddStatTooltips(List<TooltipLine> tooltips, ModItem modItem, bool displayAsJewel)
    {
        tooltips.Add(new TooltipLine(modItem.Mod, "TriggerEffect", TriggerTooltip(Main.LocalPlayer)));
        return false;
    }

    internal override void SaveData(TagCompound tag)
    {
        tag.Add("infoTriggerType", effect.GetType().AssemblyQualifiedName);
        tag.Add("infoTriggerContext", (byte)effect.Context);
    }

    internal override void LoadData(TagCompound tag)
    {
        effect = Activator.CreateInstance(Type.GetType(tag.GetString("infoTriggerType"))) as TriggerEffect;
        byte context = tag.GetByte("infoTriggerContext");
        effect.ForceSetContext((TriggerContext)context);
    }
}
