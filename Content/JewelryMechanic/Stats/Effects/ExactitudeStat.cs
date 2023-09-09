﻿namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects;

internal class ExactitudeStat : JewelStatEffect
{
    public override StatType Type => StatType.Exactitude;
    public override Color Color => Color.Yellow;

    public override void Apply(Player player, float strength, Item item)
    {
    }

    public override float GetEffectValue(float multiplier) => PeculiarJewelry.StatConfig.ExactitudeStrength * multiplier;
}
