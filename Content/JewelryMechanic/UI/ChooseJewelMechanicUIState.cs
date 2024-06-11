using Newtonsoft.Json.Converters;
using PeculiarJewelry.Content.JewelryMechanic.Desecration;
using PeculiarJewelry.Content.JewelryMechanic.UI.Superimposition;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace PeculiarJewelry.Content.JewelryMechanic.UI;

internal class ChooseJewelMechanicUIState(int whoAmI) : UIState
{
    private static readonly Asset<Texture2D> _CutTexture;
    private static readonly Asset<Texture2D> _SetTexture;
    private static readonly Asset<Texture2D> _SuperimpositionTexture;
    private static readonly Asset<Texture2D> _DesecrationTexture;

    private NPC LapidaristOwner => Main.npc[_lapidaristWhoAmI];

    private readonly int _lapidaristWhoAmI = whoAmI;

    static ChooseJewelMechanicUIState()
    {
        _CutTexture = ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/UI/JewelCut");
        _SetTexture = ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/UI/JewelSet");
        _SuperimpositionTexture = ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/UI/Superimposition");
        _DesecrationTexture = ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/UI/Desecration");
    }

    internal static string Localize(string postfix) => Language.GetTextValue("Mods.PeculiarJewelry.UI.Misc." + postfix);

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (LapidaristOwner.DistanceSQ(Main.LocalPlayer.Center) > 400 * 400 || Main.npcChatText == string.Empty)
            JewelUISystem.SwitchUI(null);
    }

    public override void OnInitialize()
    {
        UIPanel panel = new() // Main back panel
        {
            Width = StyleDimension.FromPixels(436),
            Height = StyleDimension.FromPixels(60),
            HAlign = 0.5f,
            VAlign = 0.25f
        };
        Append(panel);

        UIImageButton cutButton = new(_CutTexture)
        {
            Width = StyleDimension.FromPixels(32),
            Height = StyleDimension.FromPixels(32),
            Left = StyleDimension.FromPixels(20),
            Top = StyleDimension.FromPixelsAndPercent(-30, 1),
            VAlign = 1f,
        };

        cutButton.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
        {
            Main.npcChatText = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.Open.Cut");
            Main.playerInventory = true;

            JewelUISystem.SwitchUI(new CutJewelUIState());
        };

        cutButton.OnRightClick += CutHelp;
        panel.Append(cutButton);

        UIText cutJewelsText = new(Localize("CutJewels"), 0.8f)
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(-10)
        };
        cutButton.Append(cutJewelsText);

        UIImageButton setButton = new(_SetTexture)
        {
            Width = StyleDimension.FromPixels(32),
            Height = StyleDimension.FromPixels(32),
            Left = StyleDimension.FromPixels(120),
            Top = StyleDimension.FromPixelsAndPercent(-30, 1),
            VAlign = 1f,
        };
        setButton.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
        {
            Main.npcChatText = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.Open.Set");
            Main.playerInventory = true;

            JewelUISystem.SwitchUI(new SetJewelUIState());
        };

        setButton.OnRightClick += SetHelp;
        panel.Append(setButton);

        UIText setText = new(Localize("SetJewels"), 0.8f)
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(-10)
        };
        setButton.Append(setText);

        // Superimposition

        UIImageButton impositionButton = new(_SuperimpositionTexture)
        {
            Width = StyleDimension.FromPixels(32),
            Height = StyleDimension.FromPixels(32),
            Left = StyleDimension.FromPixels(220),
            Top = StyleDimension.FromPixelsAndPercent(-30, 1),
            VAlign = 1f,
        };

        impositionButton.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
        {
            Main.npcChatText = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.Open.Imposition");
            Main.playerInventory = true;

            JewelUISystem.SwitchUI(new SuperimpositionUIState());
        };

        impositionButton.OnRightClick += ImposHelp;

        panel.Append(impositionButton);

        UIText impositionText = new(Language.GetTextValue("Mods.PeculiarJewelry.UI.Superimposition.Superimposition"), 0.8f)
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(-10)
        };
        impositionButton.Append(impositionText);

        // Desecration

        UIImageButton desecrationButton = new(_DesecrationTexture)
        {
            Width = StyleDimension.FromPixels(32),
            Height = StyleDimension.FromPixels(32),
            Left = StyleDimension.FromPixels(340),
            Top = StyleDimension.FromPixelsAndPercent(-30, 1),
            VAlign = 1f,
        };

        desecrationButton.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
        {
            Main.npcChatText = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.Open.Desecration");
            Main.playerInventory = true;

            JewelUISystem.SwitchUI(new DesecrationUIState());
        };

        desecrationButton.OnRightClick += DeseHelp;

        panel.Append(desecrationButton);

        UIText desecrationText = new(Localize("Path"), 0.8f)
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(-10)
        };
        desecrationButton.Append(desecrationText);
    }

    private void ImposHelp(UIMouseEvent e, UIElement lE) => Main.npcChatText = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.Help.Imposition");
    private void SetHelp(UIMouseEvent e, UIElement lE) => Main.npcChatText = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.Help.Set");
    private void CutHelp(UIMouseEvent e, UIElement lE) => Main.npcChatText = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.Help.Cut");
    private void DeseHelp(UIMouseEvent e, UIElement lE) => Main.npcChatText = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.Help.Desecration");
}