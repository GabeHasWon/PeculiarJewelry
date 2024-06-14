using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.Items.Jewels;
using System;
using Terraria.UI;

namespace PeculiarJewelry.Content.JewelryMechanic.UI.JewelStorage;

public class UIJewelSlot : UIElement
{
    public static bool HoveringSlot = false;

    public bool Empty => item.IsAir;

    internal Action<UIJewelSlot, Item> onEmptySlot;
    internal Action<UIJewelSlot, Item> onFullSlot;

    public Item item;
    
    private int _itemSlotContext;

    public UIJewelSlot(Item item, Action<UIJewelSlot, Item> onEmpty = null, Action<UIJewelSlot, Item> onFull = null, int itemSlotContext = ItemSlot.Context.ChestItem)
    {
        this.item = item;
        _itemSlotContext = itemSlotContext;
        onEmptySlot = onEmpty;
        onFullSlot = onFull;
        Width = new StyleDimension(48f, 0f);
        Height = new StyleDimension(48f, 0f);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        if (Width.Pixels <= 0 && Height.Pixels <= 0 && Width.Percent <= 0 && Height.Percent <= 0)
            return;

        HandleItemSlotLogic();

        Vector2 position = GetDimensions().Center() + new Vector2(52f, 52f) * -0.5f * Main.inventoryScale;
        ItemSlot.Draw(spriteBatch, ref item, _itemSlotContext, position);
    }

    private void HandleItemSlotLogic()
    {
        if (IsMouseHovering && ValidItem(item, true))
        {
            HoveringSlot = true;
            Main.LocalPlayer.mouseInterface = true;
            Main.isMouseLeftConsumedByUI = true;

            ItemSlot.OverrideHover(ref item, _itemSlotContext);
            ItemSlot.LeftClick(ref item, _itemSlotContext);
            ItemSlot.MouseHover(ref item, _itemSlotContext);
        }

        if (item.IsAir)
            onEmptySlot?.Invoke(this, item);
        else
            onFullSlot?.Invoke(this, item);
    }

    private static bool ValidItem(Item item, bool checkHeld)
    {
        if (checkHeld && Main.LocalPlayer.selectedItem == 58 && !Main.LocalPlayer.HeldItem.IsAir && !ValidItem(Main.LocalPlayer.HeldItem, false))
            return false;

        return item.ModItem is not null && item.ModItem.Mod == ModContent.GetInstance<PeculiarJewelry>();
    }

    public override int CompareTo(object obj)
    {
        if (obj is not UIJewelSlot)
            return JewelryStorageUI.ReverseGridOrder ? -1 : 1;

        switch (JewelryStorageUI.Order)
        {
            case JewelryStorageUI.OrderMode.Default:
                break;

            case JewelryStorageUI.OrderMode.Material:
                return JewelryCompare(obj, (x, y) => x.MaterialCategory.CompareTo(y.MaterialCategory));

            case JewelryStorageUI.OrderMode.Tier:
                return JewelCompare(obj, (x, y) => x.info.tier.CompareTo(y.info.tier));

            case JewelryStorageUI.OrderMode.CutsRemaining:
                return JewelCompare(obj, (x, y) => x.info.RemainingCuts.CompareTo(y.info.RemainingCuts));

            case JewelryStorageUI.OrderMode.MainStat:
                return JewelCompare(obj, (x, y) => x.info.Major.GetName().Value.CompareTo(y.info.Major.GetName().Value));

            case JewelryStorageUI.OrderMode.SuccessfulCuts:
                return JewelCompare(obj, (x, y) => x.info.successfulCuts.CompareTo(y.info.successfulCuts));

            case JewelryStorageUI.OrderMode.JewelryQuality:
                return JewelryCompare(obj, (x, y) => x.tier.CompareTo(y.tier));

            case JewelryStorageUI.OrderMode.DustCost:
                return DustCostCompare(obj);
        }

        return obj is UIJewelSlot slot ? UniqueId.CompareTo(slot.UniqueId) : base.CompareTo(obj);
    }

    private int DustCostCompare(object obj)
    {
        int myCost = 0;
        int otherCost = 0;

        if (item.ModItem is Jewel jewel)
            myCost = jewel.info.TotalDustCost();
        else if (item.ModItem is BasicJewelry jewelry)
        {
            foreach (var item in jewelry.Info)
                myCost += item.TotalDustCost();
        }

        if (obj is not UIJewelSlot otherSlot)
            return myCost > 0 ? 1 : 0;
        else
        {
            if (otherSlot.item.ModItem is Jewel otherJewel)
                otherCost = otherJewel.info.TotalDustCost();
            else if (otherSlot.item.ModItem is BasicJewelry otherJewelry)
            {
                foreach (var item in otherJewelry.Info)
                    otherCost += item.TotalDustCost();
            }
        }

        return otherCost.CompareTo(myCost);
    }

    private int JewelCompare(object obj, Func<Jewel, Jewel, int> valueFunc)
    {
        if (item.ModItem is Jewel jewel)
        {
            if (obj is not UIJewelSlot otherSlot || otherSlot.item.ModItem is not Jewel otherJewel)
                return 1;
            else
            {
                int comp = valueFunc.Invoke(otherJewel, jewel);

                if (comp == 0)
                    comp = obj is UIJewelSlot slot ? UniqueId.CompareTo(slot.UniqueId) : base.CompareTo(obj);

                return comp;
            }
        }

        return obj is not UIJewelSlot otherSl || otherSl.item.ModItem is not Jewel ? 1 : -1;
    }

    private int JewelryCompare(object obj, Func<BasicJewelry, BasicJewelry, int> valueFunc)
    {
        if (item.ModItem is BasicJewelry jewelry)
        {
            return obj is not UIJewelSlot otherSlot || otherSlot.item.ModItem is not BasicJewelry otherJewelry
                ? 1
                : valueFunc(otherJewelry, jewelry);
        }

        return obj is not UIJewelSlot otherSl || otherSl.item.ModItem is not BasicJewelry ? 1 : -1;
    }
}
