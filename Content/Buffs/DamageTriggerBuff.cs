﻿using PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers;

namespace PeculiarJewelry.Content.Buffs;

internal class DamageTriggerBuff : ModBuff
{
    public override void Update(Player player, ref int buffIndex) 
        => player.GetDamage(DamageClass.Generic) += player.GetModPlayer<StackableBuffTracker>().StackableStrength("Damage");
}