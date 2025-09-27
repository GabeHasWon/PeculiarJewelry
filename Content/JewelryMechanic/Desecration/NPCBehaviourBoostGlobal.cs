namespace PeculiarJewelry.Content.JewelryMechanic.Desecration;

public class NPCBehaviourBoostGlobal : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public float modifiedAISpeed = 0f;

    private float _extraAITimer = 0;
    private bool _boosting = false;

    public override void ResetEffects(NPC npc)
    {
        _boosting = false;
        modifiedAISpeed = 0;
    }

    public override bool PreAI(NPC npc)
    {
        if (_extraAITimer <= -1)
        {
            _extraAITimer++;
            npc.position -= npc.velocity;
            return false;
        }
        
        return true;
    }

    public override void PostAI(NPC npc)
    {
        if (_boosting)
            return;

        if (modifiedAISpeed == 0)
        {
            _extraAITimer = 0;
            return;
        }

        _extraAITimer += modifiedAISpeed;
        var oldPosition = npc.position;

        while (_extraAITimer >= 1f)
        {
            _boosting = true;
            npc.AI();
            _boosting = false;
            _extraAITimer--;
        }

        npc.position = oldPosition;
    }
}
