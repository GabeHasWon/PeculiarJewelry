using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using Terraria;
using static Humanizer.In;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class MythrilBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Mythril";

    float bonus = 1f;

    public override bool AppliesToStat(Player player, StatType type) => type is StatType.Precision or StatType.Preservation or StatType.Tension;
    public override void SingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1f + player.GetModPlayer<CatEyePlayer>().GetBonus(MaterialKey, 0.15f);
    public override void ResetSingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1f;

    public override float EffectBonus(Player player, StatType statType)
    {
        float count = player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);
        return count >= 1 ? bonus : 1f;
    }

    public override void StaticBonus(Player player, bool firstSet)
    {
        float count = player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);

        if (count >= 3)
            player.GetModPlayer<MythrilBonusPlayer>().threeSet = true;

        if (count >= 5)
            player.GetModPlayer<MythrilBonusPlayer>().fiveSet = true;
    }

    class MythrilBonusPlayer : ModPlayer
    {
        internal bool threeSet = false;
        internal bool fiveSet = false;
        internal int unmissedHitsInARow = 0;

        public override void ResetEffects() => threeSet = fiveSet = false;

        public override float UseSpeedMultiplier(Item item)
            => item.DamageType.CountsAsClass(DamageClass.Ranged) && threeSet && Player.velocity.LengthSquared() <= 0.0001f ? 2f : 1;

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            unmissedHitsInARow++;
            proj.GetGlobalProjectile<MythrilBonusProjectile>().hasMissed = false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (fiveSet)
                modifiers.FinalDamage += unmissedHitsInARow / 80f;
        }
    }

    class MythrilBonusProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        internal bool hasMissed = true;

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            if (!projectile.TryGetOwner(out var owner))
                return;

            bool classSpecification = !projectile.DamageType.CountsAsClass(DamageClass.Summon) && !projectile.DamageType.CountsAsClass(DamageClass.SummonMeleeSpeed);

            if (owner.GetModPlayer<MythrilBonusPlayer>().fiveSet) // Forced ranged check for Mythril set bonus
                classSpecification = projectile.DamageType.CountsAsClass(DamageClass.Ranged);

            if (projectile.damage > 0 && projectile.friendly && hasMissed && classSpecification)
            {
                owner.GetModPlayer<MythrilBonusPlayer>().unmissedHitsInARow = 0;
                owner.GetModPlayer<DemoniteBonus.DemoniteBonusPlayer>().DoMissEffect();
                owner.GetModPlayer<CrimtaneBonus.CrimtaneBonusPlayer>().DoMissEffect();
            }
        }
    }
}
