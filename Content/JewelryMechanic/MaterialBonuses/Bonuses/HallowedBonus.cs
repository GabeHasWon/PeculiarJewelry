using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using Terraria.DataStructures;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class HallowedBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Hallowed";

    float bonus = 1f;

    public override bool AppliesToStat(Player player, StatType type) => true;
    public override void SingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1.05f;
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
            player.GetModPlayer<HallowedBonusPlayer>().threeSet = true;

        if (count >= 5)
            player.GetModPlayer<HallowedBonusPlayer>().fiveSet = true;
    }

    public class HallowedBonusPlayer : ModPlayer
    {
        internal bool IsImmune => threeSet && !Main.CurrentFrameFlags.AnyActiveBossNPC && Main.invasionType == InvasionID.None;
        internal bool threeSet = false;
        internal bool fiveSet = false;
        internal float fiveSetPower = 0;

        public override void ResetEffects() => threeSet = fiveSet = false;

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (IsImmune)
            {
                playSound = genGore = false;
                Player.statLife = 1;
                return false;
            }

            return true;
        }
    }
}
