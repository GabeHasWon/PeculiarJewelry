using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class LeadBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Lead";

    float bonus = 1f;

    public override bool AppliesToStat(Player player, StatType type) => type == StatType.Tenacity;
    public override void SingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1f + player.GetModPlayer<CatEyePlayer>().GetBonus(MaterialKey, 0.15f);
    public override void ResetSingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1f;

    public override float EffectBonus(Player player, StatType statType)
    {
        float count = CountMaterial(player);
        return count >= 1 ? bonus : 1f;
    }

    public override void StaticBonus(Player player, bool firstSet)
    {
        float count = CountMaterial(player);

        if (count >= 3)
            player.noKnockback = true;

        if (count >= 5)
        {
            player.GetModPlayer<LeadBonusPlayer>().fiveSet = true;
            player.GetModPlayer<LeadBonusPlayer>().UpdateShields();
        }
    }

    private class LeadBonusPlayer : ModPlayer
    {
        internal bool fiveSet = false;
        internal int shieldCount = 0;
        internal int drawTimer = 0;

        private int _shieldTimer = 0;
        private int _shieldImmune = 0;

        public override void ResetEffects() => fiveSet = false;

        internal void UpdateShields()
        {
            _shieldImmune--;

            if (fiveSet && shieldCount < 3)
            {
                if (_shieldTimer++ > 30 * 60)
                {
                    shieldCount++;
                    _shieldTimer = 0;
                }
            }
            else
                _shieldTimer = 0;
        }

        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (_shieldImmune > 0)
                return true;

            if (shieldCount > 0)
            {
                shieldCount--;
                _shieldImmune = 60;
                Player.AddImmuneTime(info.CooldownCounter, 60);
                Player.immune = true;

                BreakShield();
                return true;
            }

            return false;
        }

        private void BreakShield()
        {
            float rotOff = Player.GetModPlayer<LeadBonusPlayer>().drawTimer * 0.02f;
            float rot = Player.GetModPlayer<LeadBonusPlayer>().shieldCount / 3f * MathHelper.TwoPi + rotOff;
            Vector2 pos = new Vector2(0, -40).RotatedBy(rot) + Player.Center;

            for (int i = 0; i < 10; ++i)
                Dust.NewDust(pos, 1, 1, DustID.Lead);

            SoundEngine.PlaySound(SoundID.Shatter);
        }
    }

    private class LeadShieldPlayerLayer : PlayerDrawLayer
    {
        private static Asset<Texture2D> ShieldTex;

        public override void Load() => ShieldTex = ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/MaterialBonuses/Bonuses/LeadShield");
        public override void Unload() => ShieldTex = null;

        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            var leadPlayer = drawInfo.drawPlayer.GetModPlayer<LeadBonusPlayer>();

            if (!leadPlayer.fiveSet)
                return;

            float rotOff = leadPlayer.drawTimer++ * 0.02f;

            for (int i = 0; i < leadPlayer.shieldCount; ++i)
            {
                float rot = i / 3f * MathHelper.TwoPi + rotOff;
                Vector2 pos = new Vector2(0, -40).RotatedBy(rot) + drawInfo.drawPlayer.Center - Main.screenPosition;
                var data = new DrawData(ShieldTex.Value, pos, null, Color.White, rot, new Vector2(14, 8), 1f, SpriteEffects.None);
                drawInfo.DrawDataCache.Add(data);
            }
        }
    }
}
