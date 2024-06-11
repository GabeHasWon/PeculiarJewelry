using PeculiarJewelry.Content.JewelryMechanic.Globals;
using System;
using System.Linq;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace PeculiarJewelry.Content.JewelryMechanic.UI;

internal class JewelryCraftingUI() : UIState
{
    static UIList jewelryList = null;
    static UIColoredImageButton lastButton = null;
    static UIItemIcon lastIcon = null;
    static UIItemIcon showIcon = null;
    static UIText showText = null;
    static Recipe storedRecipe = null;
    static int craftType = -1;
    static bool craftSuccess = false;
    
    int _timer = 0;

    internal static string Localize(string postfix) => Language.GetTextValue("Mods.PeculiarJewelry.UI.Misc." + postfix);

    public override void OnInitialize()
    {
        UIPanel panel = new() // Main back panel
        {
            Width = StyleDimension.FromPixels(300),
            Height = StyleDimension.FromPixels(220),
            HAlign = 0.5f,
            VAlign = 0.25f
        };
        panel.OnMouseOver += BlockMouse;
        Append(panel);

        MaterialList(panel);
        JewelryList(panel);

        showIcon = new(new Item(ItemID.None), new Vector2(64f), true) { HAlign = 1f, VAlign = 0.56f };
        panel.Append(showIcon);
        showText = new UIText("") { HAlign = 1f, VAlign = 0.6f, Top = new StyleDimension(36, 0) };
        panel.Append(showText);

        UIButton<string> exitButton = new($"x")
        {
            Width = StyleDimension.FromPixels(30),
            Height = StyleDimension.FromPixels(30),
            VAlign = -0.02f,
            HAlign = 1f
        };
        exitButton.OnLeftClick += (sender, e) => JewelUISystem.SwitchUI(null);
        panel.Append(exitButton);

        UIButton<string> craftButton = new(Localize("Craft"))
        {
            Width = StyleDimension.FromPixels(80),
            Height = StyleDimension.FromPixels(36),
            VAlign = 0.19f,
            HAlign = 1f
        };
        craftButton.OnLeftClick += (_, _) => ClickCraft();
        craftButton.OnUpdate += UpdateCraftButton;
        panel.Append(craftButton);
    }

    private void UpdateCraftButton(UIElement affectedElement)
    {
        _timer--;

        var button = affectedElement as UIButton<string>;
        var color = Color.Lerp(Color.White, craftSuccess ? Color.Green : Color.Red, MathHelper.Clamp(_timer / 60f, 0, 1));
        button.SetText($"[c/{color.Hex3()}:{Localize("Craft")}]");
    }

    private void ClickCraft()
    {
        if (_timer > 0)
            return;

        craftSuccess = false;
        _timer = 60;

        if (storedRecipe is null || craftType == -1)
            return;

        foreach (var item in storedRecipe.requiredItem)
        {
            if (Main.LocalPlayer.CountItem(item.type, item.stack) < item.stack)
                return;
        }

        foreach (var ingredient in storedRecipe.requiredItem)
        {
            int stack = ingredient.stack;

            for (int i = 0; i < Main.LocalPlayer.inventory.Length; ++i)
            {
                Item item = Main.LocalPlayer.inventory[i];

                if (item.type != ingredient.type)
                    continue;

                if (item.stack >= stack)
                {
                    item.stack -= stack;
                    stack = 0;
                }
                else
                {
                    stack -= item.stack;
                    item.stack = 0;
                }

                if (item.stack <= 0)
                    item.TurnToAir();

                if (stack < 0)
                    break;
            }
        }

        craftSuccess = true;
        Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Loot("JewelCrafting"), craftType);
    }

    private static void BlockMouse(UIMouseEvent evt, UIElement listeningElement)
    {
        Main.isMouseLeftConsumedByUI = true;
        Main.LocalPlayer.mouseInterface = true;
        Main.LocalPlayer.lastMouseInterface = true;
    }

    private static void JewelryList(UIPanel panel)
    {
        UIText text = new(Localize("Type"))
        {
            Left = StyleDimension.FromPixels(104)
        };
        panel.Append(text);

        UIPanel subPanel = new()
        {
            Width = StyleDimension.FromPixels(86),
            Height = StyleDimension.FromPixelsAndPercent(-30, 1f),
            VAlign = 1f,
            Left = StyleDimension.FromPixels(104)
        };
        panel.Append(subPanel);

        jewelryList = new()
        {
            Width = StyleDimension.FromPixelsAndPercent(-20, 1),
            Height = StyleDimension.Fill,
        };
        subPanel.Append(jewelryList);

        UIScrollbar bar = new()
        {
            Width = StyleDimension.FromPixels(20),
            Height = StyleDimension.Fill,
            HAlign = 1f
        };
        jewelryList.SetScrollbar(bar);
        subPanel.Append(bar);
    }

    private static void MaterialList(UIPanel panel)
    {
        UIText text = new(Localize("Material"));
        panel.Append(text);

        UIPanel subPanel = new()
        {
            Width = StyleDimension.FromPixels(90),
            Height = StyleDimension.FromPixelsAndPercent(-30, 1f),
            VAlign = 1f
        };
        panel.Append(subPanel);

        UIList list = new()
        {
            Width = StyleDimension.FromPixelsAndPercent(-20, 1),
            Height = StyleDimension.Fill,
        };
        subPanel.Append(list);

        UIScrollbar bar = new()
        {
            Width = StyleDimension.FromPixels(20),
            Height = StyleDimension.Fill,
            HAlign = 1.1f
        };
        list.SetScrollbar(bar);
        subPanel.Append(bar);

        foreach (int item in JewelrySets.MaterialToName.Keys)
        {
            Main.instance.LoadItem(item);
            UIColoredImageButton icon = new(TextureAssets.Item[item]);
            
            icon.OnLeftClick += (_, self) =>
            {
                UpdateJewelryList(item);

                lastButton?.SetColor(Color.White);
                (self as UIColoredImageButton).SetColor(Color.Gray);
                lastButton = self as UIColoredImageButton;
            };
            
            icon.OnMouseOver += BlockMouse;
            list.Add(icon);
        }
    }

    private static void UpdateJewelryList(int id)
    {
        jewelryList.Clear();

        foreach (int obj in JewelrySets.MatIdToItemSet[id])
        {
            UIItemIcon icon = new(new(obj));
            icon.OnMouseOver += BlockMouse;

            icon.OnLeftClick += (_, _) =>
            {
                lastIcon?.SetColor(Color.White);
                icon.SetColor(Color.Gray);
                lastIcon = icon;

                craftType = obj;
                showIcon.SetItem(new Item(obj));
                Recipe.FindRecipes(false);
                storedRecipe = Main.recipe.FirstOrDefault(x => x.createItem.type == obj && x.ContainsIngredient(id));

                if (storedRecipe is not null)
                {
                    string ingredients = "";
                    foreach (var item in storedRecipe.requiredItem)
                    {
                        ingredients += $"x{item.stack}[i:{item.type}]\n";
                    }

                    showText.SetText(ingredients);
                }
            };

            jewelryList.Add(icon);
        }
    }

    public class UIItemIcon : UIElement
    {
        private readonly float _maxSize;

        private Item _item;
        private Color _color;

        public UIItemIcon(Item item, Vector2? overrideSize = null, bool pointClamp = false)
        {
            _item = item;
            Width.Set(overrideSize is not null ? overrideSize.Value.X : 32, 0f);
            Height.Set(overrideSize is not null ? overrideSize.Value.Y : 32, 0f);
            _color = Color.White;
            _maxSize = overrideSize is not null ? MathF.Max(overrideSize.Value.X, overrideSize.Value.Y) : 32;

            if (pointClamp)
                OverrideSamplerState = SamplerState.PointClamp;
        }

        public void SetColor(Color color) => _color = color;
        public void SetItem(Item item) => _item = item;

        protected override void DrawSelf(SpriteBatch spriteBatch) => ItemSlot.DrawItemIcon(_item, 31, spriteBatch, GetDimensions().Center(), _item.scale * (_maxSize / 32f), 32, _color);
    }
}