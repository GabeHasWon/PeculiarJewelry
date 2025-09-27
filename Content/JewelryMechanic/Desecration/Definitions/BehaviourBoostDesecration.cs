namespace PeculiarJewelry.Content.JewelryMechanic.Desecration.Definitions;

internal class BehaviourBoostDesecration : DesecrationModifier
{
    public override float StrengthCap => 3f;
    public override float Profanity => 3f;

    public override void PreAI(NPC npc) => npc.GetGlobalNPC<NPCBehaviourBoostGlobal>().modifiedAISpeed += 0.33f * strength;
}
