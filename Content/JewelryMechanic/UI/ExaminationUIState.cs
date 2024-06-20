using PeculiarJewelry.Content.Items.Jewels;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace PeculiarJewelry.Content.JewelryMechanic.UI;

internal class ExaminationUIState : UIState, IClosableUIState
{
    private ItemSlotUI _slot = null;
    private UINPCDialoguePanel _dialogue = null;
    private int _idleChatId = 0;
    private bool _lastHasItem = false;

    private static string Localize(string postfix) => Language.GetTextValue("Mods.PeculiarJewelry.UI.Exam." + postfix);

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!Main.playerInventory)
        {
            JewelUISystem.SwitchUI(null);
            return;
        }

        if (_slot.HasItem)
        {
            _idleChatId = Main.rand.Next(3);

            if (!_lastHasItem)
                _dialogue.SetText((_slot.Item.ModItem as Jewel).ExaminationLocalization.Value);
        }
        else if (_lastHasItem)
            _dialogue.SetText(Localize("Idle." + _idleChatId));

        _lastHasItem = _slot.HasItem;
    }

    public override void OnInitialize()
    {
        Width = StyleDimension.Fill;
        Height = StyleDimension.Fill;

        UIPanel panel = new()
        {
            Width = StyleDimension.FromPixels(90),
            Height = StyleDimension.FromPixels(90),
            Top = StyleDimension.FromPercent(0.15f),
            HAlign = 0.5f,
            VAlign = 0f
        };

        Append(panel);
        BuildMainPanel(panel);

        _dialogue = new(500)
        {
            HAlign = 0.5f,
            VAlign = 0f,
            Top = StyleDimension.FromPixelsAndPercent(45, 0.15f),
        };

        Append(_dialogue);
    }

    private void BuildMainPanel(UIPanel panel)
    {
        Item air = new();
        air.TurnToAir();
        _slot = new([air], 0, CutJewelUIState.CanJewelSlotAcceptItem)
        {
            HAlign = 0.5f,
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill
        };
        panel.Append(_slot);
    }

    protected override void DrawChildren(SpriteBatch spriteBatch)
    {
        float oldScale = Main.inventoryScale;
        Main.inventoryScale = 0.9f;
        base.DrawChildren(spriteBatch);
        Main.inventoryScale = oldScale;
    }

    public void Close()
    {
        if (_slot.HasItem)
            Main.LocalPlayer.QuickSpawnItem(new EntitySource_OverfullInventory(Main.LocalPlayer), _slot.Item, _slot.Item.stack);
    }
}
