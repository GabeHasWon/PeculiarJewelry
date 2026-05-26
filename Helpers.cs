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
using Terraria.UI;

namespace PeculiarJewelry;

public static class Extensions
{
    public static float MaterialBonus(this Player player, string material, StatType type) => BaseMaterialBonus.BonusesByKey[material].EffectBonus(player, type);
    public static void SingleBonus(this Player player, string mat, BasicJewelry jewel) => BaseMaterialBonus.BonusesByKey[mat].SingleJewelBonus(player, jewel);
    public static void UndoSingle(this Player player, string mat, BasicJewelry jewel) => BaseMaterialBonus.BonusesByKey[mat].ResetSingleJewelBonus(player, jewel);

    public static Vector2 SafeNormalize(this Vector2 vec, Vector2? fallback = null)
    {
        var norm = Vector2.Normalize(vec);

        if (vec.HasNaNs())
            return fallback ?? Vector2.Zero;

        return norm;
    }

    public static Vector2 SafeDirectionTo(this Entity entity, Vector2 target, Vector2? fallback = null) => SafeNormalize(entity.DirectionTo(target), fallback);

    public static void ApplyMouseInterfaceBlock(this UIElement element) => element.OnUpdate += AddMouseInterfaceBlock;

    private static void AddMouseInterfaceBlock(UIElement affectedElement)
    {
        if (affectedElement.ContainsPoint(Main.MouseScreen))
            Main.LocalPlayer.mouseInterface = true;
    }
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