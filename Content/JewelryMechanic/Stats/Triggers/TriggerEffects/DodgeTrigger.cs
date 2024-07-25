using PeculiarJewelry.Content.Buffs;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers.TriggerEffects;

internal class DodgeTrigger : TriggerEffect
{
    public override TriggerType Type => TriggerType.Conditional;

    protected override void InternalConditionalEffect(TriggerContext context, Player player, float coefficient, JewelTier tier)
        => player.GetModPlayer<StackableBuffTracker>().StackableBuff<DodgeTriggerBuff>("Dodge", new(2, TotalConditionalStrength(coefficient, tier)));

    protected override float InternalTriggerPower() => 650;
}
