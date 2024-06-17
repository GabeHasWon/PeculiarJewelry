using MonoMod.RuntimeDetour;
using System;
using System.Linq;
using System.Reflection;
using Terraria.GameContent;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace PeculiarJewelry.Content.JewelryMechanic.UI.JewelStorage;

internal class HijackGridInnerListDrawing : ILoadable
{
    internal static bool DrawAdditionalSlots = false;

    private static Hook innerListDrawHook = null;
    private static Hook recalcChildrenHook = null;

    public void Load(Mod mod)
    {
        var drawChildren = typeof(UIGrid).GetNestedType("UIInnerList", BindingFlags.NonPublic).GetMethod("DrawChildren", BindingFlags.NonPublic | BindingFlags.Instance);
        innerListDrawHook = new Hook(drawChildren, HijackDraw, true);

        var recalcChildren = typeof(UIGrid).GetMethod(nameof(UIGrid.RecalculateChildren), BindingFlags.Public | BindingFlags.Instance);
        recalcChildrenHook = new Hook(recalcChildren, HijackRecalc, true);
    }

    public static void HijackRecalc(Action<UIGrid> orig, UIGrid self)
    {
        orig(self);

        UIElement parent = self;

        while (parent.Parent is not null)
            parent = parent.Parent;

        if (parent is JewelryStorageUI)
        {
            var height = self.GetType().GetField("_innerListHeight", BindingFlags.NonPublic | BindingFlags.Instance);
            height.SetValue(self, (float)height.GetValue(self) + 16);
        }
    }

    public static void HijackDraw(Action<UIElement, SpriteBatch> orig, UIElement self, SpriteBatch spriteBatch)
    {
        orig(self, spriteBatch);

        if (!DrawAdditionalSlots || !self.Children.Any())
            return;

        // Sometimes, we write bad code. It's inevitable, uneviable, and most of all, probably not going to change.

        Vector2 lastPos = Vector2.Zero;
        var bottomItem = self.Children.OrderBy(x => x.GetDimensions().Position().Y).Last();

        foreach (var item in self.Children.Where(x => x.GetDimensions().Position().Y == bottomItem.GetDimensions().Position().Y))
        {
            var dim = item.GetDimensions();

            if (lastPos.X < dim.X && lastPos.Y <= dim.Y)
                lastPos = dim.Position();
        }

        int length = self.Children.Where(x => x is UIJewelSlot).Count();// Main.LocalPlayer.GetModPlayer<JewelStoragePlayer>().storage.Count;
        int xOff = 1;

        for (int i = 0; i < length % 9; ++i)
        {
            var tex = TextureAssets.InventoryBack5.Value;
            var backPos = lastPos + new Vector2(xOff * 53, 0);
            spriteBatch.Draw(tex, backPos - new Vector2(2, 2), null, Color.White * 0.85f, 0f, Vector2.Zero, Main.inventoryScale, SpriteEffects.None, 0);

            length++;
            xOff++;
        }
    }

    public void Unload()
    {
        innerListDrawHook.Undo();
        innerListDrawHook.Dispose();
        innerListDrawHook = null;
    }
}
