using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Stats;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class BeetleBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Beetle";

    float bonus = 1f;

    public override bool AppliesToStat(Player player, StatType type) => type is StatType.Tenacity or StatType.Permenance or StatType.Celerity or StatType.Dexterity;
    public override void SingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1.25f;
    public override void ResetSingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1f;

    public override float EffectBonus(Player player, StatType type)
    {
        bool movement = type is StatType.Celerity or StatType.Dexterity;

        if (CountMaterial(player) >= 1)
            return movement ? bonus : 0.94f;

        return 1f;
    }

    public override void StaticBonus(Player player, bool firstSet)
    {
        float count = CountMaterial(player);

        if (count >= 3 && player.velocity.LengthSquared() < 0.01f)
            player.GetDamage(DamageClass.Generic) += 1;

        if (count >= 5)
            player.GetModPlayer<BeetleBonusPlayer>().fiveSet = true;
    }

    class BeetleBonusPlayer : ModPlayer
    {
        public bool fiveSet = false;

        public override void ResetEffects() => fiveSet = false;

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (fiveSet && Player.velocity.LengthSquared() < 0.01f)
                modifiers.FinalDamage /= 2;
        }
    }
}
