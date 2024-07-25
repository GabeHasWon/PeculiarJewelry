namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers.TriggerEffects;

internal class ImmuneFrameTrigger : TriggerEffect
{
    public override TriggerType Type => TriggerType.InstantOther;
    public override bool NeedsCooldown => true;

    protected override void InternalInstantEffect(TriggerContext context, Player player, float coefficient, JewelTier tier)
    {
        int time = (int)TotalTriggerPower(player, coefficient, tier);
        player.AddImmuneTime(ImmunityCooldownID.General, time);
        player.AddImmuneTime(ImmunityCooldownID.Bosses, time);
        player.immuneNoBlink = false;
        player.immune = true;
        player.immuneTime = time;
        player.AddBuff(CooldownBuffType, CooldownTime(tier));
    }

    protected override float InternalTriggerPower() => 60;
}
