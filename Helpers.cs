global using Terraria;
global using Terraria.ID;
global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using Terraria.Localization;
global using Terraria.ModLoader;

using PeculiarJewelry.Content.JewelryMechanic.Stats;
using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;
using PeculiarJewelry.Content.Items.JewelryItems;
using Terraria.DataStructures;

namespace PeculiarJewelry;

public static class Extensions
{
    public static float MaterialBonus(this Player player, string material, StatType type) => BaseMaterialBonus.BonusesByKey[material].EffectBonus(player, type);
    public static void SingleBonus(this Player player, string mat, BasicJewelry jewel) => BaseMaterialBonus.BonusesByKey[mat].SingleJewelBonus(player, jewel);
    public static void UndoSingle(this Player player, string mat, BasicJewelry jewel) => BaseMaterialBonus.BonusesByKey[mat].ResetSingleJewelBonus(player, jewel);
}

public static class NewItem
{
    public static void SpawnSynced(IEntitySource source, Vector2 position, int type, int stack, bool noGrabDelay)
    {
        int item = Item.NewItem(source, position, type, stack, noGrabDelay: noGrabDelay);

        if (Main.netMode == NetmodeID.MultiplayerClient)
            NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item);
    }

    public static void SpawnSynced(IEntitySource source, Vector2 position, Item item, bool noGrabDelay)
    {
        int whoAmI = Item.NewItem(source, position, item, noGrabDelay: noGrabDelay);

        if (Main.netMode == NetmodeID.MultiplayerClient)
            NetMessage.SendData(MessageID.SyncItem, -1, -1, null, whoAmI);
    }
}