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
    private static readonly Asset<Texture2D> _ExaminationTexture;

    private NPC LapidaristOwner => Main.npc[_lapidaristWhoAmI];

    private readonly int _lapidaristWhoAmI = whoAmI;

    static ChooseJewelMechanicUIState()
    {
        _CutTexture = ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/UI/JewelCut");
        _SetTexture = ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/UI/JewelSet");
        _SuperimpositionTexture = ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/UI/Superimposition");
        _DesecrationTexture = ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/UI/Desecration");
        _ExaminationTexture = ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/UI/Examination");
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
            Width = StyleDimension.FromPixels(546),
            Height = StyleDimension.FromPixels(70),
            HAlign = 0.5f,
            VAlign = 0.25f
        };
        Append(panel);

        UIImageButton cutButton = new(_CutTexture)
        {
            Width = StyleDimension.FromPixels(32),
            Height = StyleDimension.FromPixels(32),
            Left = StyleDimension.FromPixels(20),
            Top = StyleDimension.FromPixels(6),
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
            Top = StyleDimension.FromPixels(-20)
        };
        cutButton.Append(cutJewelsText);

        UIImageButton setButton = new(_SetTexture)
        {
            Width = StyleDimension.FromPixels(32),
            Height = StyleDimension.FromPixels(32),
            Left = StyleDimension.FromPixels(120),
            Top = StyleDimension.FromPixels(6),
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
            Top = StyleDimension.FromPixels(-20)
        };
        setButton.Append(setText);

        // Superimposition

        UIImageButton impositionButton = new(_SuperimpositionTexture)
        {
            Width = StyleDimension.FromPixels(32),
            Height = StyleDimension.FromPixels(32),
            Left = StyleDimension.FromPixels(220),
            Top = StyleDimension.FromPixels(6),
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
            Top = StyleDimension.FromPixels(-20)
        };
        impositionButton.Append(impositionText);

        // Desecration

        UIImageButton desecrationButton = new(_DesecrationTexture)
        {
            Width = StyleDimension.FromPixels(32),
            Height = StyleDimension.FromPixels(32),
            Left = StyleDimension.FromPixels(346),
            Top = StyleDimension.FromPixels(6),
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
            Top = StyleDimension.FromPixels(-20)
        };
        desecrationButton.Append(desecrationText);

        // Examination

        UIImageButton examinationButton = new(_ExaminationTexture)
        {
            Width = StyleDimension.FromPixels(32),
            Height = StyleDimension.FromPixels(32),
            Left = StyleDimension.FromPixels(460),
            Top = StyleDimension.FromPixels(6),
            VAlign = 1f,
        };

        examinationButton.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
        {
            Main.npcChatText = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.Open.Examination");
            Main.playerInventory = true;

            JewelUISystem.SwitchUI(new ExaminationUIState());
        };

        examinationButton.OnRightClick += ExamHelp;

        panel.Append(examinationButton);

        UIText examText = new(Localize("Examination"), 0.8f)
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(-20)
        };
        examinationButton.Append(examText);
    }

    private void ImposHelp(UIMouseEvent e, UIElement lE) => Main.npcChatText = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.Help.Imposition");
    private void SetHelp(UIMouseEvent e, UIElement lE) => Main.npcChatText = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.Help.Set");
    private void CutHelp(UIMouseEvent e, UIElement lE) => Main.npcChatText = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.Help.Cut");
    private void DeseHelp(UIMouseEvent e, UIElement lE) => Main.npcChatText = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.Help.Desecration");
    private void ExamHelp(UIMouseEvent e, UIElement lE) => Main.npcChatText = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.Help.Examination");
}