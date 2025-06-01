using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using Terraria.GameContent.Drawing;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class SilverBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Silver";

    float bonus = 1f;

    public override bool AppliesToStat(Player player, StatType type) => type == StatType.Celerity;
    public override void SingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1f + +player.GetModPlayer<CatEyePlayer>().GetBonus(MaterialKey, 0.15f);
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
            player.GetModPlayer<SilverBonusPlayer>().threeSet = true;

        if (count >= 5)
            player.GetModPlayer<SilverBonusPlayer>().fiveSet = true;
    }

    class SilverBonusPlayer : ModPlayer
    {
        internal bool threeSet = false;
        internal bool fiveSet = false;

        public override void ResetEffects() => threeSet = fiveSet = false;

        public override void PostUpdateRunSpeeds()
        {
            if (fiveSet)
            {
                Player.dash = 1;
                Player.dashType = 5;
            }

            if (!threeSet)
                return;

            Player.maxRunSpeed *= 2f;
            Player.runAcceleration *= 1.2f;
            Player.runSlowdown *= 2f;
            Player.DoBootsEffect(SilverRunEffect);
        }

        public bool SilverRunEffect(int X, int Y)
        {
            Tile tile = Main.tile[X, Y + 1];

            if (tile == null || !tile.HasTile || tile.LiquidAmount > 0 || !WorldGen.SolidTileAllowBottomSlope(X, Y + 1))
                return false;

            ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings
            {
                PositionInWorld = new Vector2(Player.position.X + Main.rand.NextFloat(Player.width), Player.position.Y + Main.rand.NextFloat(Player.height)),
            });
            return true;
        }
    }
}
