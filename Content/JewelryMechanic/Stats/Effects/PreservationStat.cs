namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects;

internal class PreservationStat : JewelStatEffect
{
    public override StatType Type => StatType.Preservation;
    public override Color Color => Color.LightGreen;
    public override StatExclusivity Exclusivity => StatExclusivity.Ranged;

    public override void Apply(Player player, float strength) => player.GetModPlayer<PreservationPlayer>().bonus += GetEffectBonus(player, strength);
    protected override float InternalEffectBonus(float multiplier, Player player) => PeculiarJewelry.StatConfig.PreservationStrength * multiplier;

    class PreservationPlayer : ModPlayer
    {
        private static bool Recursing = false;

        public float bonus = 0f;

        public override void ResetEffects() => bonus = 0f;
        public override bool CanConsumeAmmo(Item weapon, Item ammo) => Main.rand.NextFloat() > bonus / 100f;

        public override void OnConsumeAmmo(Item weapon, Item ammo)
        {
            if (bonus < 0 && !Recursing)
            {
                while (Main.rand.NextFloat() < -bonus / 100f)
                {
                    Recursing = true;
                    Player.ConsumeItem(ammo.type);
                    Recursing = false;

                    bonus++;
                }
            }
        }
    }
}
