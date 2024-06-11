using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace PeculiarJewelry.Content.JewelryMechanic.UI.JewelStorage;

internal class JewelStoragePlayer : ModPlayer
{
    public static Hook hookOrder = null;

    public List<Item> storage = [];

    public override void Load()
    {
        var method = typeof(UIGrid).GetMethod(nameof(UIGrid.UpdateOrder));
        hookOrder = new Hook(method, new Action<Action<UIGrid>, UIGrid>((orig, self) =>
        {
            orig(self);

            if (JewelryStorageUI.ReverseGridOrder)
                self._items.Reverse();
        }));
    }

    public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
    {
        if (context is not ItemSlot.Context.InventoryItem)
            return false;

        if (JewelUISystem.Instance.JewelInterface.CurrentState is JewelryStorageUI storage && !JewelryStorageUI.IsInvalidItemForStorage(inventory[slot]))
        {
            storage.AddItem(inventory[slot]);
            return true;
        }

        return false;
    }

    public override void Unload() => hookOrder.Undo();

    public override void SaveData(TagCompound tag)
    {
        tag.Add("count", storage.Count);

        for (int i = 0; i < storage.Count; i++)
        {
            Item item = storage[i];
            tag.Add("item" + i, ItemIO.Save(item));
        }
    }

    public override void LoadData(TagCompound tag)
    {
        int count = tag.GetInt("count");

        for (int i = 0; i < count; ++i)
        {
            Item item = ItemIO.Load(tag.GetCompound("item" + i));

            if (item.IsAir || item.stack == 0 || item.type == ItemID.None)
                continue;

            storage.Add(item);
        }
    }
}
