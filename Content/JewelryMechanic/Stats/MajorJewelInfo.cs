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
    public override bool IsRare => false;

    internal override void InternalSetup()
    {
        SubStats = new List<JewelStat>(4);
        effect = Activator.CreateInstance(Main.rand.Next(ModContent.GetContent<TriggerEffect>().ToList()).GetType()) as TriggerEffect;
    }

    public virtual void InstantTrigger(TriggerContext context, Player player) => effect.InstantTrigger(context, player, tier);
    public virtual void ConstantTrigger(Player player, float bonus) => effect.ConstantTrigger(player, tier, bonus);
    public virtual string TriggerTooltip(Player player) => effect.Tooltip(tier, player);

    internal override bool PreAddStatTooltips(List<TooltipLine> tooltips, ModItem modItem, bool displayAsJewel, ref float modStrength)
    {
        tooltips.Add(new TooltipLine(modItem.Mod, "TriggerEffect", TriggerTooltip(Main.LocalPlayer)));
        return false;
    }

    internal override void SaveData(TagCompound tag) => tag.Add("effect", effect.Save());

    internal override void LoadData(TagCompound tag)
    {
        if (tag.TryGet("effect", out TagCompound effectTag))
            effect = TriggerEffect.Load(effectTag);
        else
        {
            effect = Activator.CreateInstance(Type.GetType(tag.GetString("infoTriggerType"))) as TriggerEffect;
            byte context = tag.GetByte("infoTriggerContext");
            effect.ForceSetContext((TriggerContext)context);
            effect.multiplier = 1f;
        }
    }
}
