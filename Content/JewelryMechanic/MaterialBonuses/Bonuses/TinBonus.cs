﻿using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Stats;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class TinBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Tin";

    float bonus = 1f;

    public override bool AppliesToStat(Player player, StatType type) => type == StatType.Renewal;
    public override void SingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1.15f;
    public override void ResetSingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1f;

    public override float EffectBonus(Player player, StatType statType)
    {
        int count = player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);

        if (count >= 1)
            return bonus;
        return 1f;
    }

    public override void StaticBonus(Player player, bool firstSet)
    {
        int count = player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);

        if (count >= 5)
            player.GetModPlayer<TinMaterialPlayer>().state = TinMaterialPlayer.State.Set5;
        else if (count >= 3)
            player.GetModPlayer<TinMaterialPlayer>().state = TinMaterialPlayer.State.Set3;
    }

    internal class TinMaterialPlayer : ModPlayer
    {
        public enum State
        {
            None,
            Set3,
            Set5
        }

        public State state;

        private int _lastHealthHit = 0;

        public override void ResetEffects() => state = State.None;

        internal void TinBonuses()
        {
            if (state != State.None && Player.statLife < _lastHealthHit && Player.lifeRegen > 0)
                Player.lifeRegen *= 3;

            if (state == State.Set5)
                Player.GetDamage(DamageClass.Generic) += Player.lifeRegen / 200f;
        }

        public override void OnHurt(Player.HurtInfo info) => _lastHealthHit = Player.statLife;
    }
}
