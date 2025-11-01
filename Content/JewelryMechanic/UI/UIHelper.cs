using System;

namespace PeculiarJewelry.Content.JewelryMechanic.UI;

internal static class UIHelper
{
    public static void CheckNPCInRange(Player player, NPC npc, Action<Player, NPC> distanceAction)
    {
        int tileRangeX = Player.tileRangeX;
        int tileRangeY = Player.tileRangeY;
        Rectangle rectangle = new((int)(player.Center.X - tileRangeX * 16f), (int)(player.Center.Y - tileRangeY * 16f), tileRangeX * 16 * 2, tileRangeY * 16 * 2);
        Rectangle value2 = new((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);

        if (!rectangle.Intersects(value2) || player.chest != -1 || !npc.active)
        {
            distanceAction(player, npc);
        }
    }

    public static void HoldTownNPC(int whoAmI, Player player = null)
    {
        player ??= Main.LocalPlayer;

        NPC npc = Main.npc[whoAmI];
        npc.ai[0] = 0f;
        npc.ai[1] = 300f;
        npc.localAI[3] = 100f;

        if (player.Center.X < npc.Center.X)
            npc.direction = -1;
        else
            npc.direction = 1;
    }
}
