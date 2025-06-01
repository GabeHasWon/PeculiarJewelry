using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using ReLogic.Content;
using System;
using Terraria.DataStructures;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class SpectreBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Spectre";

    float bonus = 1f;

    public override bool AppliesToStat(Player player, StatType type) => type is StatType.Celerity or StatType.Dexterity or StatType.Permenance or StatType.Tenacity;
    public override void SingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1 + player.GetModPlayer<CatEyePlayer>().GetBonus(MaterialKey, 0.25f);
    public override void ResetSingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1f;

    public override float EffectBonus(Player player, StatType type)
    {
        float count = CountMaterial(player);
        bool defensive = type == StatType.Permenance || type == StatType.Tenacity;
        return count >= 1 ? defensive ? bonus : 1f - player.GetModPlayer<CatEyePlayer>().GetBonus(MaterialKey, 0.06f) : 1f;
    }

    public override void StaticBonus(Player player, bool firstSet)
    {
        float count = CountMaterial(player);

        if (count >= 3)
        {
            player.waterWalk = player.waterWalk2 = true;

            if (player.wings <= 0) // This is the 3 set bonus - mimic wings
            {
                player.wings = ArmorIDs.Wing.JimsWings;
                player.wingTimeMax = player.GetWingStats(ArmorIDs.Wing.FishronWings).FlyTime;
                player.equippedWings = new Item(ItemID.FishronWings);
                player.wingsLogic = ArmorIDs.Wing.FishronWings;
            }
        }

        if (count >= 5)
            player.GetModPlayer<SpectreBonusPlayer>().fiveSet = true;
    }

    class SpectreBonusPlayer : ModPlayer
    {
        const int MaxTeleportDistanceInTiles = 20;

        internal bool fiveSet;

        private Vector2 _ghostVelocity;
        private Vector2 _ghostPosition;
        private bool _ghosting;
        private int _ghostTime;
        private float _ghostAlpha;

        private int _holdDownTime = 0;

        public override void ResetEffects() => fiveSet = false;

        public override void PostUpdateRunSpeeds()
        {
            if (!fiveSet)
                return;

            if (Player.controlDown && !Player.pulley)
            {
                _holdDownTime++;

                if (_holdDownTime > 15)
                {
                    _ghosting = true;
                    _holdDownTime = 0;
                }
            }
            else if (!_ghosting)
                _holdDownTime = 0;

            if (!_ghosting)
            {
                _ghostPosition = Player.position;
                return;
            }
                
            _ghostTime++;

            if (Player.controlDown)
                _ghostVelocity.Y += 0.2f;

            if (Player.controlUp)
                _ghostVelocity.Y -= 0.2f;

            if (Player.controlLeft)
                _ghostVelocity.X -= 0.2f;

            if (Player.controlRight)
                _ghostVelocity.X += 0.2f;

            if (_ghostVelocity.LengthSquared() > 12 * 12)
                _ghostVelocity = Vector2.Normalize(_ghostVelocity) * 12;

            if (Vector2.DistanceSquared(Player.position, _ghostPosition + _ghostVelocity) <= Math.Pow(MaxTeleportDistanceInTiles * 16, 2))
                _ghostPosition += _ghostVelocity;
            else
                _ghostVelocity = Vector2.Zero;

            if (Main.rand.NextBool(2))
                Dust.NewDust(_ghostPosition + Player.Size / 2f, 1, 1, DustID.SpectreStaff, Scale: Main.rand.NextFloat(1.5f, 2f));

            if (!Player.controlUp && !Player.controlDown && !Player.controlLeft && !Player.controlRight)
                _holdDownTime++;
            else
                _holdDownTime = 0;

            if (!Collision.SolidCollision(_ghostPosition, Player.width, Player.height) && (_holdDownTime > 5 || Player.controlJump))
                Teleport();
        }

        private void Teleport()
        {
            for (float i = 0; i < 1; i += 0.025f)
                Dust.NewDust(Vector2.Lerp(Player.Center, _ghostPosition + Player.Size / 2f, i), 1, 1, DustID.SpectreStaff);

            Player.Center = _ghostPosition;

            _ghostVelocity = Vector2.Zero;
            _ghosting = false;
        }

        public override void PreUpdateMovement()
        {
            if (_ghosting)
            {
                Player.velocity.Y *= 0.8f;
                Player.velocity.X *= 0.5f;
            }
        }

        class CameraControl : ModSystem
        {
            public override void ModifyScreenPosition()
            {
                var spectre = Main.LocalPlayer.GetModPlayer<SpectreBonusPlayer>();

                if (spectre.fiveSet && spectre._ghosting)
                    Main.screenPosition = spectre._ghostPosition - (Main.ScreenSize.ToVector2() / 2f) + Main.LocalPlayer.Size / 2f;
            }
        }

        class SpectreLayer : PlayerDrawLayer
        {
            private Asset<Texture2D> _wisp;

            public override void Load() => _wisp = ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/MaterialBonuses/Bonuses/SpectreShadow");
            public override void Unload() => _wisp = null;

            public override Position GetDefaultPosition() => PlayerDrawLayers.BeforeFirstVanillaLayer;

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                if (drawInfo.shadow > 0)
                    return;

                var spectre = drawInfo.drawPlayer.GetModPlayer<SpectreBonusPlayer>();

                if (!spectre.fiveSet)
                    return;

                spectre._ghostAlpha = MathHelper.Lerp(spectre._ghostAlpha, spectre._ghosting ? 0.6f : 0.1f, 0.05f);

                Player plr = drawInfo.drawPlayer;
                Color color = (!Collision.SolidCollision(spectre._ghostPosition, plr.width, plr.height) ? Color.White : Color.Red) * spectre._ghostAlpha;
                Vector2 pos = spectre._ghostPosition - Main.screenPosition + plr.Size / 2f;
                float rot = spectre._ghostVelocity.X * 0.02f;
                Vector2 scale = new Vector2(1f, MathF.Sin(spectre._ghostTime * 0.02f) * 0.25f + 1f);
                drawInfo.DrawDataCache.Add(new DrawData(_wisp.Value, pos, null, color, rot, _wisp.Value.Size() / 2f, scale, SpriteEffects.None, 0));
            }
        }
    }
}
