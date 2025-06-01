using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Stats;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class AdamantiteBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Adamantite";

    float bonus = 1f;

    public override bool AppliesToStat(Player player, StatType type) => type == StatType.Exactitude || type == StatType.Exploitation;
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
            player.GetModPlayer<AdamantiteBonusPlayer>().threeSet = true;

        if (count >= 5)
            player.GetModPlayer<AdamantiteBonusPlayer>().fiveSet = true;
    }

    class AdamantiteBonusPlayer : ModPlayer
    {
        internal bool threeSet = false;
        internal bool fiveSet = false;

        private float _chance;
        private StatModifier _mods;
        private int _baseDamage;

        public override void ResetEffects() => threeSet = fiveSet = false;

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!threeSet)
                return;

            _chance = (item.crit + 4) / 100f;
            _mods = modifiers.CritDamage;
            _baseDamage = item.damage;

            modifiers.ModifyHitInfo += ModifyCritStack;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!threeSet)
                return;

            _chance = proj.CritChance / 100f;
            _mods = modifiers.CritDamage;
            _baseDamage = proj.damage;

            modifiers.ModifyHitInfo += ModifyCritStack;
        }

        private void ModifyCritStack(ref NPC.HitInfo info)
        {
            if (!info.Crit && fiveSet && Main.rand.NextFloat() < _chance)
            {
                info.Crit = true;
                info.Damage = (int)_mods.ApplyTo(info.Damage);
            }

            if (info.Crit)
            {
                float critDamage = _mods.ApplyTo(_baseDamage);
                bool safetyConsumed = false;

                while (true)
                {
                    _chance /= 2;
                    bool success = Main.rand.NextFloat() < _chance;

                    if (fiveSet && !success && !safetyConsumed)
                    {
                        success = Main.rand.NextFloat() < _chance;
                        safetyConsumed = true;
                    }

                    if (success)
                        info.Damage += (int)critDamage;
                    else
                        break;
                }
            }
        }
    }
}