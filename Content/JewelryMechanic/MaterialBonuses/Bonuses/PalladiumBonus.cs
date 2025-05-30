using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Stats;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class PalladiumBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Palladium";

    float bonus = 1f;

    public override bool AppliesToStat(Player player, StatType type) => type is StatType.Frenzy or StatType.Gigantism or StatType.Might;
    public override void SingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1.15f;
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
            player.GetModPlayer<PalladiumBonusPlayer>().threeSet = true;

        if (count >= 5)
            player.GetModPlayer<PalladiumBonusPlayer>().fiveSet = true;
    }

    class PalladiumBonusPlayer : ModPlayer
    {
        internal bool threeSet = false;
        internal bool fiveSet = false;

        private bool _speedUpNextSwing = false;

        public override void ResetEffects() => threeSet = fiveSet = false;

        public override float UseSpeedMultiplier(Item item)
        {
            if (_speedUpNextSwing)
            {
                _speedUpNextSwing = false;
                return 2.6f;
            }

            return 1f;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (threeSet)
                _speedUpNextSwing = true;
        }

        public override void MeleeEffects(Item item, Rectangle hitbox)
        {
            if (fiveSet && !item.noMelee)
            {
                for (int i = 0; i < Main.maxProjectiles; ++i)
                {
                    Projectile p = Main.projectile[i];

                    if (p.active && p.hostile && p.Hitbox.Intersects(hitbox))
                        p.Kill();
                }
            }
        }
    }
}
