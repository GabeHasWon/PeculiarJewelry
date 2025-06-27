using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects;

/// <summary>
/// Projectile behaviour speed. MP safe.
/// </summary>
internal class LegionStat : JewelStatEffect
{
    public override StatType Type => StatType.Legion;
    public override Color Color => Color.CornflowerBlue;
    public override StatExclusivity Exclusivity => StatExclusivity.Summon;

    public override void Apply(Player player, float strength) => player.GetModPlayer<LegionPlayer>().bonus += GetEffectBonus(player, strength) / 100f;
    protected override float InternalEffectBonus(float multiplier, Player player) => PeculiarJewelry.StatConfig.LegionStrength * multiplier * 2;

    private class LegionPlayer : ModPlayer
    {
        public float bonus = 0f;

        public override void ResetEffects() => bonus = 0f;
    }

    private class LegionProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        float totalSpeed;

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.minion || entity.sentry;

        public override bool PreAI(Projectile projectile)
        {
            if (!projectile.TryGetOwner(out var owner) || owner is null)
                return true;

            float speed = owner.GetModPlayer<LegionPlayer>().bonus;
            totalSpeed += speed;

            if (totalSpeed < 0)
            {
                totalSpeed++;
                projectile.timeLeft++;
                return false;
            }

            while (totalSpeed > 1f)
            {
                CobaltBonus.CobaltBonusProjectile.RepeatAI(projectile, (int)totalSpeed);
                totalSpeed -= (int)totalSpeed;
            }

            return true;
        }
    }
}
