using PeculiarJewelry.Content.JewelryMechanic.Desecration;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SoulstoneDebuffs;

internal class PlagueDebuff : ModBuff
{
    public override void SetStaticDefaults() => Main.debuff[Type] = true;
}

internal class PlagueNPC : GlobalNPC
{
    public override bool PreAI(NPC npc)
    {
        if (npc.HasBuff<PlagueDebuff>())
            npc.GetGlobalNPC<NPCBehaviourBoostGlobal>().modifiedAISpeed -= 0.5f;
        
        return true;
    }
}