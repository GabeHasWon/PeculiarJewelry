using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.Items.Jewels;
using Steamworks;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace PeculiarJewelry.Content.JewelryMechanic.UI.JewelStorage;

internal class JewelryStorageUI() : UIState
{
    public enum OrderMode
    {
        /// <summary>
        /// By ID only.
        /// </summary>
        Default,

        /// <summary>
        /// By jewelry material.
        /// </summary>
        Material,

        /// <summary>
        /// By jewel tier.
        /// </summary>
        Tier,

        /// <summary>
        /// By jewel cuts remaining.
        /// </summary>
        CutsRemaining,

        /// <summary>
        /// By jewel main stat.
        /// </summary>
        MainStat,

        /// <summary>
        /// By jewel successful cuts.
        /// </summary>
        SuccessfulCuts,

        /// <summary>
        /// By jewelry quality.
        /// </summary>
        JewelryQuality,

        /// <summary>
        /// By jewel or jewelry dust cost.
        /// </summary>
        DustCost,

        ExlJewelMajorStat,
        ExlJewelSubStat,
        ExlJewelryMaterial,

        Max,
    }

    public enum SortMode
    {
        None,
        Jewel,
        Jewelry
    }

    public static OrderMode Order = OrderMode.Default;
    public static bool ReverseGridOrder = false;

    private readonly List<UIElement> gridRemovals = [];

    private UIGrid grid = null;
    private UIPanel panel = null;
    private UIPanel searchPanel = null;
    private int id = 0;
    private bool reverseOrder = true;
    private SortMode sortMode = SortMode.None;
    private string searchTerm = string.Empty;

    internal static string Localize(string postfix) => Language.GetTextValue("Mods.PeculiarJewelry.UI.Misc." + postfix);

    public override void Update(GameTime gameTime)
    {
        ReverseGridOrder = reverseOrder;

        base.Update(gameTime);

        foreach (var item in gridRemovals)
            grid.Remove(item);

        gridRemovals.Clear();

        if (!Main.playerInventory)
            JewelUISystem.SwitchUI(null);

        ReverseGridOrder = false;
    }

    public override void OnInitialize()
    {
        Order = OrderMode.Default;

        panel = new() // Main back panel
        {
            Width = StyleDimension.FromPixels(532),
            Height = StyleDimension.FromPixels(340),
            HAlign = 0.5f,
            VAlign = 0.2f
        };
        Append(panel);

        panel.Append(new UIText("Jewel Storage", 0.7f, true) { VAlign = 0, HAlign = 0.25f, Top = StyleDimension.FromPixels(4) });

        CreateSortButton(panel);

        grid = new()
        {
            Width = StyleDimension.FromPixelsAndPercent(-24, 1),
            Height = StyleDimension.FromPixelsAndPercent(-40, 1),
            VAlign = 1f,
        };

        grid.OnLeftClick += AddItemToStorage;

        ResetGridContents(false);

        panel.Append(grid);

        var bar = new UIScrollbar()
        {
            HAlign = 1f,
            VAlign = 1f,
            Width = StyleDimension.FromPixels(20),
            Height = StyleDimension.FromPixelsAndPercent(-40, 1)
        };

        grid.SetScrollbar(bar);
        panel.Append(bar);

        UIButton<string> exitButton = new($"x")
        {
            Width = StyleDimension.FromPixels(30),
            Height = StyleDimension.FromPixels(30),
        };
        exitButton.OnLeftClick += (sender, e) => JewelUISystem.SwitchUI(null);
        panel.Append(exitButton);

        UIButton<string> ascButton = new($"^")
        {
            Width = StyleDimension.FromPixels(30),
            Height = StyleDimension.FromPixels(30),
            HAlign = 1f
        };
        
        ascButton.OnLeftClick += (sender, e) =>
        {
            reverseOrder = !reverseOrder;
            (e as UIButton<string>).SetText(reverseOrder ? "^" : "v");
            grid._items.Reverse();
        };

        panel.Append(ascButton);
    }

    private void CreateSortButton(UIPanel panel)
    {
        var button = new UIButton<string>("Sort by: Default", 1f, false)
        {
            VAlign = 0,
            HAlign = 1f,
            Left = StyleDimension.FromPixels(-34),
            Width = StyleDimension.FromPixels(180),
            Height = StyleDimension.FromPixels(30)
        };

        button.OnLeftClick += (_, ele) => ChangeSortMode(ele, 1);
        button.OnRightClick += (_, ele) => ChangeSortMode(ele, -1);

        panel.Append(button);
    }

    private void ChangeSortMode(UIElement listeningElement, int inc)
    {
        Order += inc;

        if (Order >= OrderMode.Max)
            Order = OrderMode.Default;

        if (Order < OrderMode.Default)
            Order = OrderMode.Max - 1;

        SortMode oldSortMode = sortMode;

        sortMode = Order switch
        {
            OrderMode.ExlJewelryMaterial => SortMode.Jewelry,
            OrderMode.ExlJewelMajorStat or OrderMode.ExlJewelSubStat => SortMode.Jewel,
            _ => SortMode.None
        };

        string text = $"Sort by: {Order}";
        
        if (Order.ToString().StartsWith("Exl"))
            text = $"Search by: {Order}";

        (listeningElement as UIButton<string>).SetText(text);

        if (oldSortMode != sortMode)
            ResetGridContents(true);

        grid.UpdateOrder();

        if (reverseOrder)
            grid._items.Reverse();
    }

    private void ResetGridContents(bool resetSearch)
    {
        if (resetSearch)
        {
            if (searchPanel is not null && HasChild(searchPanel))
                RemoveChild(searchPanel);

            if (sortMode != SortMode.None)
            {
                searchPanel = new UIPanel()
                {
                    HAlign = 0.5f,
                    VAlign = 0.2f,
                    Top = StyleDimension.FromPixels(-100),
                    Left = StyleDimension.FromPixels(156),
                    Width = StyleDimension.FromPixels(200),
                    Height = StyleDimension.FromPixels(40),
                };

                var searchBar = new UISearchBar(Language.GetText("Mods.PeculiarJewelry.UI.Misc.Search"), 1f)
                {
                    Width = StyleDimension.Fill,
                    Height = StyleDimension.Fill,
                    IgnoresMouseInteraction = true
                };
                searchBar.SetContents(null, true);
                searchPanel.Append(searchBar);
                searchPanel.OnLeftClick += (_, self) => searchBar.ToggleTakingText();
                searchBar.OnContentsChanged += SearchBar_OnContentsChanged;

                Append(searchPanel);
            }
        }

        grid.Clear();

        for (int i = 0; i < Main.LocalPlayer.GetModPlayer<JewelStoragePlayer>().storage.Count; i++)
        {
            Item item = Main.LocalPlayer.GetModPlayer<JewelStoragePlayer>().storage[i];

            if (sortMode == SortMode.None || sortMode == SortMode.Jewelry && item.ModItem is BasicJewelry || sortMode == SortMode.Jewel && item.ModItem is Jewel)
            {
                if (sortMode == SortMode.None)
                    grid.Add(GenerateJewelSlot(i));
                else if (searchTerm == string.Empty || ItemFitsSearch(item))
                    grid.Add(GenerateJewelSlot(i));
            }
        }
    }

    private bool ItemFitsSearch(Item item)
    {
        if (Order == OrderMode.ExlJewelryMaterial)
        {
            var jewelry = item.ModItem as BasicJewelry;

            if (jewelry.MaterialCategory.Contains(searchTerm))
                return true;
        }

        var jewel = item.ModItem as Jewel;

        switch (Order)
        {
            case OrderMode.ExlJewelMajorStat:
                return jewel.info.Major.GetName().Value.Contains(searchTerm);

            case OrderMode.ExlJewelSubStat:
                foreach (var sub in jewel.info.SubStats)
                {
                    if (sub.GetName().Value.Contains(searchTerm))
                        return true;
                }

                break;
        }

        return false;
    }

    private void SearchBar_OnContentsChanged(string obj)
    {
        if (obj is null || obj == string.Empty)
            searchTerm = string.Empty;
        else
            searchTerm = obj;
        
        ResetGridContents(false);
    }

    private UIJewelSlot GenerateJewelSlot(int i)
    {
        Item item = Main.LocalPlayer.GetModPlayer<JewelStoragePlayer>().storage[i];
        var slot = new UIJewelSlot(item, id++, (self, _) => ClearSelf(self, item)) 
        { 
            HAlign = 0.02f, 
            VAlign = 0.01f 
        };

        slot.OnUpdate += HoverJewelSlot;

        return slot;
    }

    private void HoverJewelSlot(UIElement affectedElement)
    {
        var slot = affectedElement as UIJewelSlot;

        if (affectedElement.IsMouseHovering)
        {
            Main.hoverItemName = slot.item.Name;

            if (slot.item.stack > 1)
                Main.hoverItemName = Main.hoverItemName + " (" + slot.item.stack + ")";
        }
    }

    private void ClearSelf(UIElement element, Item item)
    {
        Main.LocalPlayer.GetModPlayer<JewelStoragePlayer>().storage.Remove(item);
        gridRemovals.Add(element);
    }

    private void AddItemToStorage(UIMouseEvent evt, UIElement listeningElement)
    {
        Item heldItem = Main.LocalPlayer.HeldItem;

        if (UIJewelSlot.HoveringSlot || IsInvalidItemForStorage(heldItem))
        {
            UIJewelSlot.HoveringSlot = false;
            return;
        }

        AddItem(heldItem);

        UIJewelSlot.HoveringSlot = false;
    }

    public static bool IsInvalidItemForStorage(Item item) => item.IsAir || item.ModItem is null || item.ModItem.Mod is not PeculiarJewelry;

    public void AddItem(Item item)
    {
        var clone = item.Clone();
        Main.LocalPlayer.GetModPlayer<JewelStoragePlayer>().storage.Add(clone);
        grid.Add(GenerateJewelSlot(Main.LocalPlayer.GetModPlayer<JewelStoragePlayer>().storage.Count - 1));
        item.TurnToAir();
        Main.mouseItem.TurnToAir();
        grid.UpdateOrder();

        if (reverseOrder)
            grid._items.Reverse();
    }

    protected override void DrawChildren(SpriteBatch spriteBatch)
    {
        float oldScale = Main.inventoryScale;
        Main.inventoryScale = 1f;
        base.DrawChildren(spriteBatch);
        Main.inventoryScale = oldScale;
    }
}