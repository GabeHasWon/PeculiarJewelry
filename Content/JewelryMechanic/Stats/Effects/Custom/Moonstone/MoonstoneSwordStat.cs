using PeculiarJewelry.Content.JewelryMechanic.MoonstoneMinions;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.Moonstone;

/// <summary>
/// Defense piercing. MP safe.
/// </summary>
internal class MoonstoneSwordStat : JewelStatEffect
{
    public override StatType Type => StatType.MoonstoneSword;
    public override Color Color => new(135, 135, 135);

    public override void Apply(Player player, float strength) => player.GetModPlayer<MoonstonePlayer>().DesiredProjectileCountByStatType[Type]++;
    protected override float InternalEffectBonus(float multiplier, Player player) => 1;
    protected override bool SkipStatMods(Player player, float strength) => true;
}
