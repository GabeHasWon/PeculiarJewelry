using NetEasy;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;

namespace PeculiarJewelry.Content.JewelryMechanic.Syncing;

[Serializable]
public class SyncLandModule(int playerWhoLanded) : Module
{
    private readonly int playerWhoLanded = playerWhoLanded;

    protected override void Receive()
    {
        Player self = Main.player[playerWhoLanded];

        foreach (var item in self.GetModPlayer<JewelPlayer>().TriggerJewelInfos)
            item.InstantTrigger(TriggerContext.OnLand, self);

        self.GetModPlayer<JewelPlayer>().landCooldown = 60;

        if (Main.netMode == NetmodeID.Server)
            Send(-1, playerWhoLanded, false);
    }
}
