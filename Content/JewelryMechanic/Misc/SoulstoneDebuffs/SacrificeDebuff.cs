namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SoulstoneDebuffs;

internal class SacrificeDebuff : ModBuff
{
    public override void SetStaticDefaults() => Main.debuff[Type] = true;
}

internal class SacrificeNPC : GlobalNPC
{
    public override bool InstancePerEntity => true;

    float elapsedDoT = 0f;

    public override bool PreAI(NPC npc)
    {
        if (!npc.HasBuff<SacrificeDebuff>())
        {
            if (elapsedDoT > 1)
            {
                ApplyDoT(npc, (int)elapsedDoT, ref elapsedDoT, Color.Gray, Color.Black);
            }

            elapsedDoT = 0;
            return true;
        }

        elapsedDoT += npc.life / 12f / 60f;

        if (elapsedDoT > 30)
            ApplyDoT(npc, 30, ref elapsedDoT, Color.Gray, Color.Black);

        return true;
    }

    public static void ApplyDoT(NPC npc, int damage, ref float elapsedDoT, Color? lightColor = null, Color? darkColor = null)
    {
        lightColor ??= Color.OrangeRed;
        darkColor ??= Color.Orange;

        if (!npc.dontTakeDamage && !npc.immortal)
        {
            if (npc.realLife == -1)
            {
                npc.life -= damage;
            }
            else
            {
                Main.npc[npc.realLife].life -= damage;
            }
        }

        elapsedDoT -= damage;

        // Vanilla is dumb, this is the easiest way to properly kill an NPC while showing gore & doing death effects,
        // that is, WITHOUT calling StrikeNPC for every hit, which causes a hit sound and forces a specific combat text
        if (npc.life <= 0)
        {
            npc.life = 1;
            NPC.HitInfo info = default;
            info.HideCombatText = true;
            info.Damage = 1;
            npc.StrikeNPC(info);
        }

        CombatText.NewText(npc.Hitbox, Color.Lerp(lightColor.Value, darkColor.Value, Main.rand.NextFloat()), damage, false, true);
    }
}