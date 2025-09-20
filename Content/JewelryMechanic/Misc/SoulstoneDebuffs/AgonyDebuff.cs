namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SoulstoneDebuffs;

internal class AgonyDebuff : ModBuff
{
    public override void SetStaticDefaults() => Main.debuff[Type] = true;
}

internal class AgonyNPC : GlobalNPC
{
    public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        if (npc.HasBuff<AgonyDebuff>())
            modifiers.FinalDamage += 1;
    }

    public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        if (npc.HasBuff<AgonyDebuff>())
            modifiers.FinalDamage += 1;
    }
}