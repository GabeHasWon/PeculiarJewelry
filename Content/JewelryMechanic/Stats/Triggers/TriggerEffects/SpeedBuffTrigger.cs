﻿using PeculiarJewelry.Content.Buffs;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers.TriggerEffects;

internal class SpeedTriggerConditional : TriggerEffect
{
    public override TriggerType Type => TriggerType.Conditional;

    protected override void InternalConditionalEffect(TriggerContext context, Player player, float coefficient, JewelTier tier)
    {
        player.GetModPlayer<StackableBuffTracker>().StackableBuff<SpeedTriggerBuff>("Regen", new(2, TotalConditionalStrength(coefficient, tier)));
    }

    public override float TriggerPower() => 1;
}


internal class SpeedTriggerInstant : TriggerEffect
{
    public override TriggerType Type => TriggerType.InstantStatus;

    protected override void InternalInstantEffect(TriggerContext context, Player player, float coefficient, JewelTier tier)
    {
        player.GetModPlayer<StackableBuffTracker>().StackableBuff<SpeedTriggerBuff>("Regen", new((int)(coefficient * 5 * 60), TriggerPower() / 10f));
    }

    public override float TriggerPower() => 1.5f;
}
