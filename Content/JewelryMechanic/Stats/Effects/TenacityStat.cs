namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects;

/// <summary>
/// Endurance. MP safe.
/// </summary>
internal class TenacityStat : JewelStatEffect
{
    public override StatType Type => StatType.Tenacity;
    public override Color Color => new(100, 100, 100);

    public override void Apply(Player player, float strength) => player.endurance += GetEffectBonus(player, strength) / 100f;
    protected override float InternalEffectBonus(float multiplier, Player player) => PeculiarJewelry.StatConfig.TenacityStrength * multiplier;

    public class TenacityPlayer : ModPlayer
    {
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Player.endurance < 0) // Add damage to extra un-resistant players
            {
                modifiers.FinalDamage -= Player.endurance;
            }
        }
    }
}
