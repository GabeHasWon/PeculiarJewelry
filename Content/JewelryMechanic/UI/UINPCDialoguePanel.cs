using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.UI.Chat;

namespace PeculiarJewelry.Content.JewelryMechanic.UI;

internal class UINPCDialoguePanel(float width = 200, Color? backColor = null) : UIElement
{
    static DynamicSpriteFont Font => FontAssets.MouseText.Value;

    readonly float _width = width;

    UIPanel _panel;
    UIText _text;
    StyleDimension? _originalTop;

    public override void OnInitialize()
    {
        Width = StyleDimension.FromPixels(_width);
        Height = StyleDimension.FromPixels(40);

        _panel = new()
        {
            Width = StyleDimension.FromPixels(_width),
            Height = StyleDimension.Fill,
            HAlign = 0.5f,
            Top = Top
        };

        if (backColor is not null)
            _panel.BackgroundColor = backColor.Value;

        Append(_panel);

        _text = new UIText(Main.npcChatText)
        {
            HAlign = 0.5f,
            Width = StyleDimension.FromPixels(_width),
            Height = StyleDimension.Fill,
            IsWrapped = true,
        };
        _panel.Append(_text);
    }

    public void SetText(string text) => _text.SetText(text);

    public override void Update(GameTime gameTime)
    {
        _originalTop ??= Top;

        if (Main.npcChatText != string.Empty)
            _text.SetText(Main.npcChatText);

        var size = ChatManager.GetStringSize(Font, Font.CreateWrappedText(_text.Text, _panel.GetInnerDimensions().Width), Vector2.One);
        size = !_text.IsWrapped ? new Vector2(size.X, 16f) : new Vector2(size.X, size.Y + _text.WrappedTextBottomPadding);

        Height = StyleDimension.FromPixels(size.Y);
        Top = StyleDimension.FromPixelsAndPercent(_originalTop.Value.Pixels - size.Y / 8f, _originalTop.Value.Percent);
        Recalculate();
    }
}
