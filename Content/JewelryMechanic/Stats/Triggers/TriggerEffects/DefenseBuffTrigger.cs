using PeculiarJewelry.Content.Buffs;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers.TriggerEffects;

internal class DefenseTriggerConditional : TriggerEffect
{
    public override TriggerType Type => TriggerType.Conditional;

    protected override void InternalConditionalEffect(TriggerContext context, Player player, float coefficient, JewelTier tier)
    {
        player.GetModPlayer<StackableBuffTracker>().StackableBuff<DefenseTriggerBuff>("Defense", new(2, TotalConditionalStrength(coefficient, tier)));
    }

    protected override float InternalTriggerPower() => 2;
}


internal class DefenseTriggerInstant : TriggerEffect
{
    public override TriggerType Type => TriggerType.InstantStatus;

    protected override void InternalInstantEffect(TriggerContext context, Player player, float coefficient, JewelTier tier)
    {
        player.GetModPlayer<StackableBuffTracker>().StackableBuff<DefenseTriggerBuff>("Defense", new((int)(coefficient * 5 * 60), TotalTriggerPower(player, coefficient, tier) / 10f));
    }

    protected override float InternalTriggerPower() => 3f;
}
