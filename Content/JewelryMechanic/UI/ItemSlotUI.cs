using System;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace PeculiarJewelry.Content.JewelryMechanic.UI;

internal class ItemSlotUI(Item[] itemArray, int itemIndex, int itemSlotContext, Func<Item, ItemSlotUI, bool> handleItemFunc) : UIItemSlot(itemArray, itemIndex, itemSlotContext)
{
    private static readonly FieldInfo _UIItemSlotItemArray = null;

    public Item Item => ItemSlots[0];
    public bool HasItem => !Item.IsAir && Item.type > ItemID.None;

    private Item[] ItemSlots => (Item[])_UIItemSlotItemArray.GetValue(this);

    private readonly int _context = itemSlotContext;
    private readonly Func<Item, ItemSlotUI, bool> _handleItem = handleItemFunc;

    static ItemSlotUI()
    {
        _UIItemSlotItemArray ??= typeof(UIItemSlot).GetField("_itemArray", BindingFlags.Instance | BindingFlags.NonPublic);
    }

    public ItemSlotUI(Item[] itemArray, int itemIndex, Func<Item, ItemSlotUI, bool> handleItemFunc) : this(itemArray, itemIndex, ItemSlot.Context.BankItem, handleItemFunc)
    {
    }

    public void ForceItem(Item newItem) => ItemSlots[0] = newItem;

    private void HandleItemSlotLogic()
    {
        if (IsMouseHovering)
        {
            Main.NewText("eg");
            Main.LocalPlayer.mouseInterface = true;
            Item inv = ItemSlots[0];

            if (_handleItem is null || _handleItem.Invoke(Main.LocalPlayer.HeldItem, this))
            {
                ItemSlot.OverrideHover(ref inv, _context);
                ItemSlot.LeftClick(ref inv, _context);
                ItemSlot.RightClick(ref inv, _context);
                ItemSlot.MouseHover(ref inv, _context);
                ItemSlots[0] = inv;
            }
        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        HandleItemSlotLogic();
        Item inv = Item;
        Vector2 position = GetDimensions().Center() + new Vector2(52f, 52f) * -0.5f * Main.inventoryScale;
        ItemSlot.Draw(spriteBatch, ref inv, _context, position);
    }
}
