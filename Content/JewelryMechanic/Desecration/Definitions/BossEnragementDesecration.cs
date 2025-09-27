namespace PeculiarJewelry.Content.JewelryMechanic.Desecration.Definitions;

internal class BossEnragementDesecration : DesecrationModifier
{
    public override float StrengthCap => 2f;
    public override float Profanity => 2f;

    private class EnragedNPC : GlobalNPC
    {
        public const int EnrangeMaxTime = 60 * 60; // 1 minute

        public override bool InstancePerEntity => true;

        private int _enrageTimer = 0;
        private bool _enraged = false;

        public override bool AppliesToEntity(NPC npc, bool lateInstantiation) => npc.boss;

        public override bool PreAI(NPC npc)
        {
            float str = Desecrations[nameof(BossEnragementDesecration)].strength;

            if (str <= 0)
                return true;

            if (_enrageTimer >= EnrangeMaxTime)
                _enraged = true;
            else
                _enrageTimer++;

            if (_enraged)
            {
                npc.GetGlobalNPC<NPCBehaviourBoostGlobal>().modifiedAISpeed += 0.5f + (0.5f * str);
                npc.GetGlobalNPC<DamageResistDesecration.ResistNPC>().damageResist *= 1 + (str * 0.1f);
                npc.damage = (int)(npc.damage * (str + 1));
                npc.defense = (int)(npc.defense * (str + 1) * 0.75f);
            }

            return true;
        }

        public override Color? GetAlpha(NPC npc, Color drawColor)
        {
            if (_enraged)
                return Color.Lerp(drawColor, Color.Red, 0.5f);

            return null;
        }
    }
}
