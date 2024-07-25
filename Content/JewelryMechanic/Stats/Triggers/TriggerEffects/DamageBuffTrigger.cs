using PeculiarJewelry.Content.Buffs;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers.TriggerEffects;

internal class DamageTriggerConditional : TriggerEffect
{
    public override TriggerType Type => TriggerType.Conditional;

    protected override void InternalConditionalEffect(TriggerContext context, Player player, float coefficient, JewelTier tier)
    {
        player.GetModPlayer<StackableBuffTracker>().StackableBuff<DamageTriggerBuff>("Damage", new(2, TotalTriggerPower(player, coefficient, tier)));
    }

    protected override float InternalTriggerPower() => 100;
}


internal class DamageTriggerInstant : TriggerEffect
{
    public override TriggerType Type => TriggerType.InstantStatus;

    protected override void InternalInstantEffect(TriggerContext context, Player player, float coefficient, JewelTier tier)
    {
        player.GetModPlayer<StackableBuffTracker>().StackableBuff<DamageTriggerBuff>("Damage", new((int)(coefficient * 5 * 60), TotalTriggerPower(player, coefficient, tier) / 10f));
    }

    protected override float InternalTriggerPower() => 150f;
}
