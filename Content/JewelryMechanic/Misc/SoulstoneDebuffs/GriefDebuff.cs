namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SoulstoneDebuffs;

internal class GriefDebuff : ModBuff
{
    public override void SetStaticDefaults() => Main.debuff[Type] = true;
}

internal class GriefNPC : GlobalNPC
{
    public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
    {
        if (npc.HasBuff<GriefDebuff>())
            player.Heal(damageDone / 5);
    }

    public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
    {
        if (projectile.friendly && projectile.TryGetOwner(out Player owner) && npc.HasBuff<GriefDebuff>() && damageDone / 5 is int dmg and not 0)
            owner.Heal(dmg);
    }
}