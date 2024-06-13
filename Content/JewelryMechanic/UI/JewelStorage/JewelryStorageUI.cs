using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.Items.Jewels;
using Steamworks;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
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

        //ExlJewelMajorStat,
        //ExlJewelSubStat,
        //ExlJewelryMaterial,

        Max,
    }

    public enum SortMode
    {
        All,
        Jewel,
        Jewelry
    }

    public static OrderMode Order = OrderMode.Default;
    public static bool ReverseGridOrder = false;

    private readonly List<UIElement> gridRemovals = [];

    private UIGrid grid = null;
    private UIPanel panel = null;
    private UIPanel searchPanel = null;
    private UIPanel helpPanel = null;
    private int id = 0;
    private bool reverseOrder = true;
    private SortMode sortMode = SortMode.All;
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

        panel.Append(new UIText(Localize("StorageTitle"), 0.7f, true) { VAlign = 0, HAlign = 0.25f, Top = StyleDimension.FromPixels(4) });

        CreateSortButton(panel);

        grid = new()
        {
            Width = StyleDimension.FromPixelsAndPercent(-24, 1),
            Height = StyleDimension.FromPixelsAndPercent(-40, 1),
            VAlign = 1f,
        };

        grid.OnLeftClick += AddItemToStorage;

        ResetGridContents();

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

        UIButton<string> questionButton = new($"?")
        {
            Width = StyleDimension.FromPixels(30),
            Height = StyleDimension.FromPixels(30),
            Left = StyleDimension.FromPixels(34)
        };
        questionButton.OnLeftClick += ToggleQuestionPanel;
        panel.Append(questionButton);

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

        AppendSearch();
    }

    private void ToggleQuestionPanel(UIMouseEvent evt, UIElement listeningElement)
    {
        if (helpPanel is null)
        {
            helpPanel = new()
            {
                Width = StyleDimension.FromPixels(592),
                Height = StyleDimension.FromPixels(300),
                Top = StyleDimension.FromPixels(332),
                HAlign = 0.5f,
                VAlign = 0.2f
            };
            Append(helpPanel);

            helpPanel.Append(new UIText("You can search in normal text, or with any of the following tags:\n" +
                $"[c/AAFFAA:For jewelry]:\n{AsTag("material")} - matches material, {AsTag("jewelrytier")} - matches jewelry tier\n[c/AAFFAA:For jewels:]" +
                $"\n{AsTag("tier")} - matches jewel tier, {AsTag("cut")} - matches cut count, {AsTag("type")} - matches jewelry type, major or minor, " +
                $"{AsTag("sub")} - matches any sub stat on the jewel, {AsTag("successfulcuts")} - matches successful cut count\n" +
                $"[c/AAFFAA:For example:]\n\"material:iron jewelrytier:extravagant sub:vigor\"\nwould get any extravagant iron jewelry with at least 1 sub of vigor in it.\n" +
                $"All jewels in jewelry will count for the search.", 0.9f)
            {
                IsWrapped = true,
                Width = StyleDimension.Fill,
                Height = StyleDimension.Fill,
            });
        }
        else
        {
            RemoveChild(helpPanel);
            helpPanel = null;
        }
    }

    public static string AsTag(string tag) => $"\"{tag}\"";

    private void CreateSortButton(UIPanel panel)
    {
        var button = new UIButton<string>(Localize("OrderBy") + Localize("OrderType." + Order), 1f, false)
        {
            VAlign = 0,
            HAlign = 1f,
            Left = StyleDimension.FromPixels(-34),
            Width = StyleDimension.FromPixels(170),
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

        string text = Localize("OrderBy") + Localize("OrderType." + Order);
        
        (listeningElement as UIButton<string>).SetText(text);

        grid.UpdateOrder();

        if (reverseOrder)
            grid._items.Reverse();
    }

    private void ResetGridContents()
    {
        grid.Clear();
        int count = Main.LocalPlayer.GetModPlayer<JewelStoragePlayer>().storage.Count;

        for (int i = 0; i < count; i++)
        {
            Item item = Main.LocalPlayer.GetModPlayer<JewelStoragePlayer>().storage[i];

            if (sortMode == SortMode.All || sortMode == SortMode.Jewelry && item.ModItem is BasicJewelry || sortMode == SortMode.Jewel && item.ModItem is Jewel)
            {
                if (sortMode == SortMode.All)
                    grid.Add(GenerateJewelSlot(i));
                else if (searchTerm == string.Empty || ItemFitsSearch(item))
                    grid.Add(GenerateJewelSlot(i));
            }
        }

        if (count % 9 != 0)
        {
            int remainder = 9 - count % 9;
            var air = new Item();
            air.TurnToAir();

            for (int i = 0; i < remainder; ++i)
            {
                grid.Add(new UIJewelSlot(air, id++, (self, _) => self.id = 200)
                {
                    HAlign = 0.02f,
                    VAlign = 0.01f
                });
            }    
        }

        grid.Add(new UIElement() { Height = StyleDimension.FromPixels(10), Width = StyleDimension.FromPixels(20) });
    }

    private void AppendSearch()
    {
        searchPanel = new UIPanel()
        {
            HAlign = 0.5f,
            VAlign = 0.2f,
            Top = StyleDimension.FromPixels(-100),
            Left = StyleDimension.FromPixels(66),
            Width = StyleDimension.FromPixels(400),
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

        var sortToggle = new UIButton<string>(Localize("SortType.All"))
        {
            Width = StyleDimension.FromPixels(120),
            Height = StyleDimension.FromPixels(40),
            HAlign = 0.5f,
            VAlign = 0.2f,
            Left = StyleDimension.FromPixels(-206),
            Top = StyleDimension.FromPixels(-100)
        };

        sortToggle.OnLeftClick += (_, self) => ClickSortToggle(self, 1);
        sortToggle.OnRightClick += (_, self) => ClickSortToggle(self, -1);
        Append(sortToggle);
    }

    private void ClickSortToggle(UIElement listeningElement, int inc)
    {
        var sortToggle = listeningElement as UIButton<string>;

        sortMode += inc;

        if (sortMode > SortMode.Jewelry)
            sortMode = SortMode.All;

        if (sortMode < SortMode.All)
            sortMode = SortMode.Jewelry;

        sortToggle.SetText(Localize("SortType." + sortMode.ToString()));
        ResetGridContents();
    }

    private bool ItemFitsSearch(Item item)
    {
        if (sortMode == SortMode.Jewelry)
            return StorageSearching.JewelryMatches(searchTerm, item.ModItem as BasicJewelry);
        else if (sortMode == SortMode.Jewel)
        {
            var jewel = item.ModItem as Jewel;
            return StorageSearching.JewelMatches(searchTerm, jewel.info, jewel);
        }

        return false;
    }

    private void SearchBar_OnContentsChanged(string obj)
    {
        if (obj is null || obj == string.Empty)
            searchTerm = string.Empty;
        else
            searchTerm = obj;
        
        ResetGridContents();
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
        ResetGridContents();

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