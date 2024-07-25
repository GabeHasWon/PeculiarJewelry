using PeculiarJewelry.Content.JewelryMechanic.Syncing;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers.TriggerEffects;

internal class KBAndClearTrigger : TriggerEffect
{
    public override TriggerType Type => TriggerType.InstantOther;

    protected override void InternalInstantEffect(TriggerContext context, Player player, float coefficient, JewelTier tier)
    {
        float radius = TotalTriggerPower(player, coefficient, tier) * 16;

        if (player.whoAmI == Main.myPlayer)
        {
            for (int i = 0; i < Main.maxNPCs; ++i)
            {
                NPC npc = Main.npc[i];

                if (npc.CanBeChasedBy() && npc.DistanceSQ(player.Center) < radius * radius)
                {
                    npc.velocity = player.DirectionTo(npc.Center) * MathHelper.Lerp(npc.knockBackResist, 1f, 0.35f) * 8;
                    new SyncNPCVelocityFromClientModule(i, npc.velocity).Send();
                }
            }
        }

        for (int i = 0; i < Main.maxProjectiles; ++i)
        {
            Projectile projectile = Main.projectile[i];

            if (projectile.active && projectile.hostile && projectile.DistanceSQ(player.Center) < radius * radius)
                projectile.Kill();
        }
    }

    protected override float InternalTriggerPower() => 50;
}
