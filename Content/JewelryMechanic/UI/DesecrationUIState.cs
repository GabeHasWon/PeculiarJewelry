﻿using PeculiarJewelry.Content.JewelryMechanic.Desecration;
using PeculiarJewelry.Content.JewelryMechanic.Syncing;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace PeculiarJewelry.Content.JewelryMechanic.UI;

internal class DesecrationUIState : UIState
{
    private readonly Dictionary<string, float> TemporaryStrength = [];

    private bool _confirmGiveUp = false;
    private int _confirmGiveUpTime = 0;
    private UIButton<string> _giveUpButton = null;

    private static string Localize(string postfix) => Language.GetTextValue("Mods.PeculiarJewelry.UI.Misc." + postfix);

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_confirmGiveUpTime-- <= 0)
        {
            _giveUpButton.SetText(Localize("GiveUp"));
            _confirmGiveUp = false;
        }
    }

    public override void OnInitialize()
    {
        UIPanel panel = new()
        {
            Width = StyleDimension.FromPixels(500),
            Height = StyleDimension.FromPixels(360),
            HAlign = 0.5f,
            VAlign = 0.15f
        };

        Append(panel);

        UINPCDialoguePanel dialoguePanel = new(500)
        {
            HAlign = 0.5f,
            VAlign = 0.15f,
            Top = StyleDimension.FromPixels(180)
        };

        panel.Append(dialoguePanel);
        BuildList(panel);

        UIButton<string> confirm = new(Localize("Confirm"))
        {
            Width = StyleDimension.FromPixels(80),
            Height = StyleDimension.FromPixels(32),
            Top = StyleDimension.FromPixels(322),
            Left = StyleDimension.FromPixels(80),
            VAlign = 0.15f,
            HAlign = 0.5f,
        };
        confirm.OnLeftClick += ConfirmClick;
        Append(confirm);

        UIButton<string> reset = new(Localize("Reset"))
        {
            Width = StyleDimension.FromPixels(80),
            Height = StyleDimension.FromPixels(32),
            Top = StyleDimension.FromPixels(322),
            Left = StyleDimension.FromPixels(-80),
            VAlign = 0.15f,
            HAlign = 0.5f,
        };
        reset.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) => TemporaryStrength.Clear();
        Append(reset);

        UIButton<string> exit = new(Localize("Exit"))
        {
            Width = StyleDimension.FromPixels(80),
            Height = StyleDimension.FromPixels(32),
            Top = StyleDimension.FromPixels(322),
            VAlign = 0.15f,
            HAlign = 0.5f,
        };

        exit.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
        {
            JewelUISystem.SwitchUI(null);
            SoundEngine.PlaySound(SoundID.MenuClose);
        };

        Append(exit);

        _giveUpButton = new(Localize("GiveUp"))
        {
            Width = StyleDimension.FromPixels(120),
            Height = StyleDimension.FromPixels(32),
            Top = StyleDimension.FromPixels(322),
            Left = StyleDimension.FromPixels(380),
            VAlign = 0.15f,
            HAlign = 0.5f,
        };

        _giveUpButton.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
        {
            if (!_confirmGiveUp)
            {
                _confirmGiveUp = true;
                _confirmGiveUpTime = 360;
                _giveUpButton.SetText(Localize("AreYouSure"));
            }
            else
            {
                JewelUISystem.SwitchUI(null);
                SoundEngine.PlaySound(SoundID.MenuClose);
                ModContent.GetInstance<DesecratedSystem>().givenUp = true;
                ModContent.GetInstance<DesecratedSystem>().ClearDesecrations();
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.PeculiarJewelry.Desecrations.GiveUp"), Colors.Hardcore);
            }
        };

        Append(_giveUpButton);
        InfoPanel();
    }

    private void InfoPanel()
    {
        UIPanel panel = new()
        {
            Width = StyleDimension.FromPixels(250),
            Height = StyleDimension.FromPixels(360),
            Left = StyleDimension.FromPixels(380),
            HAlign = 0.5f,
            VAlign = 0.15f
        };

        Append(panel);

        UIText profanity = new(Localize("TotalProfanity") + " " + DesecratedSystem.TotalProfanity)
        {
            HAlign = 0.5f,
        };
        profanity.OnUpdate += (self) => (self as UIText).SetText(Localize("TotalProfanity") + " " + DesecratedSystem.TotalProfanity);
        panel.Append(profanity);

        UIText lootBonus = new($"+{DesecratedSystem.LootScaleFactor * 100:#0.##}% {Localize("EnemyLoot")}\n")
        { 
            VAlign = 0.5f,
            HAlign = 0.5f
        };
        lootBonus.OnUpdate += (self) => (self as UIText).SetText($"+{DesecratedSystem.LootScaleFactor * 100:#0.##}% {Localize("EnemyLoot")}");
        panel.Append(lootBonus);

        UIText tierBonus = new($"+{DesecratedSystem.AdditionalJewelTier} {Localize("JewelTiers")}")
        {
            VAlign = 0.5f,
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(32)
        };
        tierBonus.OnUpdate += (self) => (self as UIText).SetText($"+{DesecratedSystem.AdditionalJewelTier} {Localize("JewelTiers")}");
        panel.Append(tierBonus);
    }

    private void ConfirmClick(UIMouseEvent evt, UIElement listeningElement)
    {
        foreach (var (key, value) in TemporaryStrength)
        {
            ModContent.GetInstance<DesecratedSystem>().SetDesecration(key, value);

            if (Main.netMode != NetmodeID.SinglePlayer)
                new DesecrationModule(key, value, Main.myPlayer).Send();
        }
        TemporaryStrength.Clear();
    }

    private void BuildList(UIPanel panel)
    {
        UIList desecrationsList = new()
        {
            Width = StyleDimension.FromPixelsAndPercent(-20, 1),
            Height = StyleDimension.FromPixelsAndPercent(0, 1f),
        };

        UIScrollbar bar = new()
        {
            Width = StyleDimension.FromPixels(20),
            Height = StyleDimension.FromPercent(1f),
            Left = StyleDimension.FromPixelsAndPercent(-20, 1)
        };

        desecrationsList.SetScrollbar(bar);
        panel.Append(bar);
        panel.Append(desecrationsList);

        PopulateList(desecrationsList);
    }

    private void PopulateList(UIList desecrationsList)
    {
        foreach (var (key, value) in DesecrationModifier.Desecrations)
        {
            UIPanel singleItemPanel = new()
            {
                Width = StyleDimension.FromPixelsAndPercent(-4, 1f),
                Height = StyleDimension.FromPixels(50),
            };

            float size = FontAssets.MouseText.Value.MeasureString(value.DesecrationName).X;
            UIText text = new(value.DesecrationName)
            {
                Width = StyleDimension.FromPixels(size),
                VAlign = 0.5f,
            };
            singleItemPanel.Append(text);

            UIButton<string> increaseButton = new("+")
            {
                Width = StyleDimension.FromPixels(40),
                Height = StyleDimension.FromPixels(50),
                Left = StyleDimension.FromPixels(size + 16),
                VAlign = 0.5f,
            };
            increaseButton.OnLeftClick += (sender, e) => ClickIncrease(key);
            singleItemPanel.Append(increaseButton);

            UIText countToMax = new("0/" + (value.StrengthCap != -1 ? value.StrengthCap : "∞"))
            {
                HAlign = 1f,
                VAlign = 0.5f,
            };

            countToMax.OnUpdate += (self) => ModifyCountText(self as UIText, key);
            singleItemPanel.Append(countToMax);
            desecrationsList.Add(singleItemPanel);
        }
    }

    private void ClickIncrease(string key)
    {
        float cap = DesecrationModifier.Desecrations[key].StrengthCap;

        if (DesecrationModifier.Desecrations[key].strength == cap)
            return;

        if (!TemporaryStrength.ContainsKey(key))
            TemporaryStrength.Add(key, DesecrationModifier.Desecrations[key].strength);

        if (cap != -1)
            TemporaryStrength[key] = MathHelper.Clamp(TemporaryStrength[key] + 1, 0, cap);
        else
            TemporaryStrength[key]++;
    }

    private void ModifyCountText(UIText uiText, string key)
    {
        var dese = DesecrationModifier.Desecrations[key];
        bool hasTemp = TemporaryStrength.TryGetValue(key, out float strength);
        var str = hasTemp ? strength : dese.strength;

        uiText.SetText(str + "/" + (dese.StrengthCap != -1 ? dese.StrengthCap : "∞"));
        uiText.TextColor = hasTemp ? Color.Red : Color.White;
    }
}
