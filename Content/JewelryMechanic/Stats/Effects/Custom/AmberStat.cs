namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom;

internal class AmberStat : JewelStatEffect
{
    public override StatType Type => StatType.Exploitation;
    public override Color Color => new Color(217, 110, 4);

    Item _acc = null;

    public override void Apply(Player player, float strength)
    {
        if (_acc.expertOnly && !Main.expertMode)
            return;

        if (_acc.accessory)
            player.GrantPrefixBenefits(_acc);

        player.GrantArmorBenefits(_acc);
    }

    protected override float InternalEffectBonus(float multiplier, Player player) => 0;
}
