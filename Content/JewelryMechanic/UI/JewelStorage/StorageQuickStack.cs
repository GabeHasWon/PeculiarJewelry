using Mono.Cecil.Cil;
using MonoMod.Cil;
using PeculiarJewelry.Content.Items.Tiles;
using System.Collections.Generic;
using Terraria.Audio;

namespace PeculiarJewelry.Content.JewelryMechanic.UI.JewelStorage;
internal class StorageQuickStack : ILoadable
{
    public void Load(Mod mod) => IL_Player.QuickStackAllChests += AddStorageToQuickStack;

    private void AddStorageToQuickStack(ILContext il)
    {
        ILCursor c = new(il);

        if (!c.TryGotoNext(MoveType.After, x => x.MatchStloc(11)))
            return;

        c.Emit(OpCodes.Ldarg_0);
        c.Emit(OpCodes.Ldloc_S, (byte)9);
        c.Emit(OpCodes.Ldloc_S, (byte)10);
        c.EmitDelegate(AddStorage);
    }

    private static void AddStorage(Player self, int x, int y)
    {
        if (Main.tile[x, y].HasTile && Main.tile[x, y].TileType == ModContent.TileType<KeepsakeChestTile>() && self.Distance(new Vector2(x * 16 + 8, y * 16 + 8)) < 600f)
            ApplyQuickStack(new Vector2(x, y).ToWorldCoordinates() + new Vector2(20), self);
    }

    public void Unload() { }

    /// <summary>
    /// Copied from vanilla's ChestUI.QuickStack.
    /// </summary>
    public static void ApplyQuickStack(Vector2 containerWorldPosition, Player player)
    {
        Item[] inventory = player.inventory;
        Vector2 center = player.Center;
        List<Item> storage = player.GetModPlayer<JewelStoragePlayer>().storage;

        List<int> netIds = [];
        List<int> validIndices = [];
        List<int> emptyList = [];
        Dictionary<int, int> inventoryLookup = [];
        List<int> list4 = [];

        bool[] transfers = new bool[storage.Count];

        for (int i = 0; i < storage.Count; i++)
        {
            if (storage[i].type > ItemID.None && storage[i].stack > 0 && (storage[i].type < ItemID.CopperCoin || storage[i].type > ItemID.PlatinumCoin))
            {
                validIndices.Add(i);
                netIds.Add(storage[i].netID);
            }
            else if (storage[i].type == ItemID.None || storage[i].stack <= 0)
                emptyList.Add(i);
        }

        const int MaxInInventoryIndex = 50;
        const int MinOutOfHotbarIndex = 10;

        for (int j = MinOutOfHotbarIndex; j < MaxInInventoryIndex; j++)
        {
            if (!inventory[j].favorited && !JewelryStorageUI.IsInvalidItemForStorage(inventory[j]))
                inventoryLookup.Add(j, inventory[j].netID);
        }

        for (int k = 0; k < validIndices.Count; k++)
        {
            int storageIndex = validIndices[k];
            int netID = storage[storageIndex].netID;

            foreach (KeyValuePair<int, int> itemPair in inventoryLookup)
            {
                if (itemPair.Value == netID && inventory[itemPair.Key].netID == netID)
                {
                    int num4 = inventory[itemPair.Key].stack;

                    SoundEngine.PlaySound(SoundID.Grab);

                    Item clone = inventory[itemPair.Key];
                    storage.Add(clone);
                    inventory[itemPair.Key].TurnToAir();
                    
                    if (num4 > 0)
                        Chest.VisualizeChestTransfer(center, containerWorldPosition, storage[storageIndex], num4);
                    
                    transfers[storageIndex] = true;
                }
            }
        }

        foreach (KeyValuePair<int, int> item3 in inventoryLookup)
        {
            if (inventory[item3.Key].stack == 0)
                list4.Add(item3.Key);
        }

        foreach (int item4 in list4)
        {
            inventoryLookup.Remove(item4);
        }

        for (int l = 0; l < emptyList.Count; l++)
        {
            int num6 = emptyList[l];
            bool flag = true;
            int itemType = storage[num6].netID;

            if (itemType is >= 71 and <= 74)
                continue;

            foreach (KeyValuePair<int, int> item5 in inventoryLookup)
            {
                if ((item5.Value != itemType || inventory[item5.Key].netID != itemType) && (!flag || inventory[item5.Key].stack <= 0))
                    continue;

                SoundEngine.PlaySound(SoundID.Grab);

                if (flag)
                {
                    itemType = item5.Value;
                    storage[num6] = inventory[item5.Key];
                    inventory[item5.Key] = new Item();
                    Chest.VisualizeChestTransfer(center, containerWorldPosition, storage[num6], storage[num6].stack);
                }
                else
                {
                    int num8 = inventory[item5.Key].stack;
                    int num9 = storage[num6].maxStack - storage[num6].stack;

                    if (num9 == 0)
                        break;

                    if (num8 > num9)
                        num8 = num9;

                    ItemLoader.TryStackItems(storage[num6], inventory[item5.Key], out num8);

                    if (num8 > 0)
                        Chest.VisualizeChestTransfer(center, containerWorldPosition, storage[num6], num8);

                    if (inventory[item5.Key].stack == 0)
                        inventory[item5.Key] = new Item();
                }

                transfers[num6] = true;
                flag = false;
            }
        }

        if (Main.netMode == NetmodeID.MultiplayerClient && player.chest >= 0)
        {
            for (int m = 0; m < transfers.Length; m++)
            {
                NetMessage.SendData(MessageID.SyncChestItem, -1, -1, null, player.chest, m);
            }
        }

        netIds.Clear();
        validIndices.Clear();
        emptyList.Clear();
        inventoryLookup.Clear();
        list4.Clear();
    }
}
