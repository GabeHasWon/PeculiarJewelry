namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SoulstoneDebuffs;

internal class TortureDebuff : ModBuff
{
    public override void SetStaticDefaults() => Main.debuff[Type] = true;
}

internal class TortureNPC : GlobalNPC
{
    public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
    {
        if (npc.HasBuff<TortureDebuff>())
            modifiers.FinalDamage *= 0.1f;
    }
}