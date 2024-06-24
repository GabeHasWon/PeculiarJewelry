using PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers;
using System;
using System.Linq;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats;

internal class MajorJewelInfo : JewelInfo
{
    internal TriggerEffect effect;

    public override string Prefix => "Major";
    public override bool PriorityDisplay => true;

    internal override void InternalSetup()
    {
        SubStats = new System.Collections.Generic.List<JewelStat>(4);
        effect = Activator.CreateInstance(Main.rand.Next(ModContent.GetContent<TriggerEffect>().ToList()).GetType()) as TriggerEffect;
    }

    public void InstantTrigger(TriggerContext context, Player player) => effect.InstantTrigger(context, player, tier);
    public void ConstantTrigger(Player player, float bonus) => effect.ConstantTrigger(player, tier, bonus);

    public string TriggerTooltip(Player player) => effect.Tooltip(tier, player);
}
