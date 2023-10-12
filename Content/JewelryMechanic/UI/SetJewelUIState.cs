﻿using PeculiarJewelry.Content.JewelryMechanic.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Items.Jewels;
using PeculiarJewelry.Content.JewelryMechanic.Items.JewelSupport;
using PeculiarJewelry.Content.JewelryMechanic.Items.Pliers;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.UI.Chat;

namespace PeculiarJewelry.Content.JewelryMechanic.UI;

internal class SetJewelUIState : UIState, IClosableUIState
{
    public const int JewelSlots = 5;

    private BasicJewelry Jewelry => _jewelrySlot.Item.ModItem as BasicJewelry;
    private bool HasJewelry => _jewelrySlot.HasItem;

    DynamicSpriteFont Font => FontAssets.MouseText.Value;

    ItemSlotUI _jewelrySlot = null;
    ItemSlotUI[] _jewelSlots = null;
    ItemSlotUI[] _supportSlots = null;
    Item[] _displayJewelItems = null;
    bool[] _displayJewel = null;

    private static string Localize(string postfix) => Language.GetTextValue("Mods.PeculiarJewelry.UI.SetMenu." + postfix);

    public override void OnInitialize()
    {
        SetDialoguePanel();
        SetSetPanel();
        SetCurrentStatPanel();
        SetFutureStatPanel();
    }

    private void SetFutureStatPanel()
    {
        UIPanel panel = new()
        {
            Width = StyleDimension.FromPixels(500),
            Height = StyleDimension.FromPixels(280),
            HAlign = 0.5f,
            Left = StyleDimension.FromPixels(396),
            Top = StyleDimension.FromPixels(60)
        };
        Append(panel);

        UIText stats = new(CutJewelUIState.Localize("NoJewel"))
        {
            IsWrapped = false,
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill,
        };
        stats.OnUpdate += UpdateFutureStats;
        panel.Append(stats);
    }

    private void UpdateFutureStats(UIElement affectedElement)
    {
        UIText self = affectedElement as UIText;
        self.SetText(GetStats(true));

        UIPanel parent = self.Parent as UIPanel;
        var wrapped = Font.CreateWrappedText(self.Text, parent.GetInnerDimensions().Width);
        var size = ChatManager.GetStringSize(Font, wrapped, Vector2.One);
        size = !self.IsWrapped ? new Vector2(size.X, size.Y + 16) : new Vector2(size.X, size.Y + self.WrappedTextBottomPadding);
        parent.Height = StyleDimension.FromPixels(size.Y);
        parent.Recalculate();
    }

    private void SetCurrentStatPanel()
    {
        UIPanel panel = new()
        {
            Width = StyleDimension.FromPixels(300),
            Height = StyleDimension.FromPixels(280),
            HAlign = 0.5f,
            Left = StyleDimension.FromPixels(-296),
            Top = StyleDimension.FromPixels(60)
        };
        Append(panel);

        UIText stats = new(CutJewelUIState.Localize("NoJewel"))
        {
            IsWrapped = true,
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill,
        };
        stats.OnUpdate += UpdateCurrentStats;
        panel.Append(stats);
    }

    private void UpdateCurrentStats(UIElement affectedElement)
    {
        UIText self = affectedElement as UIText;
        self.SetText(GetStats(false));

        UIPanel parent = self.Parent as UIPanel;
        var wrapped = Font.CreateWrappedText(self.Text, parent.GetInnerDimensions().Width);
        var size = ChatManager.GetStringSize(Font, wrapped, Vector2.One);
        size = !self.IsWrapped ? new Vector2(size.X, 16f) : new Vector2(size.X, size.Y + self.WrappedTextBottomPadding);
        parent.Height = StyleDimension.FromPixels(size.Y);
        parent.Recalculate();
    }

    private string GetStats(bool isFuture, Player player = null)
    {
        if (!HasJewelry)
            return CutJewelUIState.Localize("NoJewel");

        player ??= Main.LocalPlayer;

        string allStats = isFuture ? $"[c/00FF00:{Localize("WillContain")}:]\n" : $"[c/00FF00:{Localize("Contains")}:]\n";

        if (isFuture && Jewelry.Info.Count >= Jewelry.Info.Capacity)
            return Localize("Full");

        List<JewelInfo> info = new(Jewelry.Info);

        if (isFuture)
        {
            int added = 0;

            for (int i = 0; i < JewelSlots; i++)
            {
                if (Jewelry.Info.Count + added >= Jewelry.Info.Capacity)
                    break;

                if (_displayJewel[i])
                    continue;

                if (_jewelSlots[i].HasItem)
                {
                    info.Add((_jewelSlots[i].Item.ModItem as Jewel).info);
                    added++;
                }
            }
        }

        if (!info.Any())
            return Localize("Empty");

        const string Hex = "[c/ffff00:";

        string ChangeIfDifferent(JewelInfo inf, string text) => info.IndexOf(inf) >= Jewelry.Info.Count ? $"{Hex}{text}]\n" : $"{text}\n";

        if (PeculiarJewelry.ShiftDown)
        {
            foreach (var inf in info)
            {
                allStats += ChangeIfDifferent(inf, $"{inf.Name}");

                if (inf is MajorJewelInfo major)
                    allStats += ChangeIfDifferent(inf, major.TriggerTooltip());

                allStats += ChangeIfDifferent(inf, inf.Major.GetDescription(player));

                foreach (var item in inf.SubStatTooltips(player))
                    allStats += ChangeIfDifferent(inf, item);

                if (Jewelry.Info.IndexOf(inf) < Jewelry.Info.Count - 1)
                    allStats += "\n\n";
            }
        }
        else
        {
            List<TooltipLine> lines = new();
            List<TooltipLine> originalLines = new();

            BasicJewelry.SummaryJewelryTooltips(lines, info, Jewelry.Mod);
            BasicJewelry.SummaryJewelryTooltips(originalLines, Jewelry.Info, Jewelry.Mod);

            foreach (var item in lines)
            {
                if (!originalLines.Any(x => x.Name == item.Name) || originalLines.First(x => x.Name == item.Name).Text != item.Text)
                    allStats += $"{Hex}{item.Text}]\n";
                else
                    allStats += item.Text + "\n";
            }
        }

        return allStats;
    }

    private void SetSetPanel()
    {
        UIPanel panel = new() // Main back panel
        {
            Width = StyleDimension.FromPixels(280),
            Height = StyleDimension.FromPixels(280),
            Top = StyleDimension.FromPixels(60),
            HAlign = 0.5f,
        };
        Append(panel);

        UIImageButton close = new UIImageButton(ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/UI/Close"))
        {
            Width = StyleDimension.FromPixels(32),
            Height = StyleDimension.FromPixels(32),
        };

        close.OnLeftClick += (_, _) =>
        {
            JewelUISystem.SwitchUI(null);
            SoundEngine.PlaySound(SoundID.MenuClose);
        };

        panel.Append(close);

        UIText cutText = new(Localize("Name"))
        {
            HAlign = 0.5f,
            TextColor = Color.Aquamarine,
        };
        panel.Append(cutText);

        _displayJewel = new bool[JewelSlots] { false, false, false, false, false };
        _displayJewelItems = new Item[JewelSlots] { null, null, null, null, null };
        Item air = new();
        air.TurnToAir();
        _jewelrySlot = new(new Item[] { air }, 0, ItemSlot.Context.ChestItem, CanJewelrySlotAcceptItem)
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(110)
        };
        _jewelrySlot.OnUpdate += UpdateJewelSlots;
        panel.Append(_jewelrySlot);

        UIText jewelryText = new(Localize("Jewelry"), 0.8f)
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(-14)
        };
        _jewelrySlot.Append(jewelryText);

        _jewelSlots = new ItemSlotUI[JewelSlots];
        for (int i = 0; i < JewelSlots; ++i)
        {
            int slot = i;
            _jewelSlots[i] = new(new Item[] { air }, 0, ItemSlot.Context.ChestItem, (Item item, ItemSlotUI _) => CanJewelSlotAcceptItem(ref item, slot))
            {
                HAlign = 0.5f,
                Left = StyleDimension.FromPixels((i - 2) * 52),
                Top = StyleDimension.FromPixels(42 + (Math.Abs(i - 2) * 20))
            };
            panel.Append(_jewelSlots[i]);
        }

        UIText jewelText = new(Localize("Jewels"), 0.8f)
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(-14)
        };
        _jewelSlots[2].Append(jewelText);

        _supportSlots = new ItemSlotUI[3];
        for (int i = 0; i < 3; ++i)
        {
            _supportSlots[i] = new(new Item[] { air }, 0, ItemSlot.Context.ChestItem, CanSupportSlotAcceptItem)
            {
                HAlign = 0.5f,
                Left = StyleDimension.FromPixels((i - 1) * 52),
                Top = StyleDimension.FromPixels(210)
            };
            panel.Append(_supportSlots[i]);
        }

        UIImageButton button = new(TextureAssets.Item[ItemID.IronAnvil])
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(160),
            Width = StyleDimension.FromPixels(30),
            Height = StyleDimension.FromPixels(30),
        };
        button.OnLeftClick += SetJewel;
        panel.Append(button);

        UIText supportText = new(Localize("SupportItems"), 0.8f)
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(-14)
        };
        _supportSlots[1].Append(supportText);
    }

    public static bool CanSupportSlotAcceptItem(Item item, ItemSlotUI _)
    {
        bool isMouseItem = Main.LocalPlayer.selectedItem == 58 && Main.LocalPlayer.HeldItem == item;
        return item.ModItem is ISetSupportItem || item.IsAir || !isMouseItem;
    }

    private bool CanJewelSlotAcceptItem(ref Item item, int i)
    {
        if (!_jewelrySlot.HasItem)
            return false;

        if (_displayJewel[i])
            return CanTakeOutJewel(ref item, i);

        bool isMouseItem = Main.LocalPlayer.selectedItem == 58 && Main.LocalPlayer.HeldItem == item;
        return item.ModItem is Jewel || item.IsAir || !isMouseItem;
    }

    private bool CanTakeOutJewel(ref Item item, int jewelIndex)
    {
        bool clicking = Main.mouseLeftRelease && Main.mouseLeft;
        if (!clicking)
            return false;

        if (item.ModItem is Plier plier)
        {
            Item jewel = _jewelSlots[jewelIndex].Item;

            void KillJewel() // Kills the current jewel
            {
                for (int i = jewelIndex; i < JewelSlots; ++i) // idk what exactly necessitates this code but it works so idk whatever
                {
                    _jewelSlots[i].ForceItem(i == JewelSlots - 1 ? new Item(ItemID.None) : _jewelSlots[i + 1].Item);

                    if (i < JewelSlots - 1)
                        _jewelSlots[i + 1].ForceItem(new Item(0));
                }

                jewel.TurnToAir();
                Jewelry.Info.RemoveAt(jewelIndex);
            }

            bool hasJade = _supportSlots.Any(x => x.HasItem && x.Item.type == ModContent.ItemType<StellarJade>());
            bool hasStopwatch = _supportSlots.Any(x => x.HasItem && x.Item.type == ModContent.ItemType<BrokenStopwatch>());

            if (hasJade || plier.SuccessfulAttempt())
            {
                Item newJewel = jewel.Clone();
                item = newJewel;
                Main.mouseItem = newJewel;
                KillJewel();

                if (hasJade)
                    ConsumeSupportItem(ModContent.ItemType<StellarJade>());
            }
            else
            {
                if (!hasStopwatch)
                {
                    if (Main.rand.NextBool()) // Kill pliers
                    {
                        item.TurnToAir();
                        Main.mouseItem.TurnToAir();
                    }
                    else
                        KillJewel();
                }
            }

            if (hasStopwatch)
                ConsumeSupportItem(ModContent.ItemType<BrokenStopwatch>());
        }

        return false;
    }

    private void ConsumeSupportItem(int typeToEat)
    {
        for (int i = 0; i < _supportSlots.Length; ++i)
        {
            if (_supportSlots[i].HasItem && _supportSlots[i].Item.type == typeToEat)
            {
                _supportSlots[i].Item.stack--;

                if (_supportSlots[i].Item.stack <= 0)
                    _supportSlots[i].Item.TurnToAir();
            }
        }
    }

    private void UpdateJewelSlots(UIElement affectedElement)
    {
        if (!HasJewelry)
        {
            for (int i = 0; i < JewelSlots; ++i)
            {
                var slot = _jewelSlots[i];

                if (!_displayJewel[i] && slot.HasItem && !slot.Item.IsAir && slot.Item.ModItem is Jewel)
                    Main.LocalPlayer.QuickSpawnItem(new EntitySource_OverfullInventory(Main.LocalPlayer), slot.Item);
            }

            for (int i = 0; i < _jewelSlots.Length; i++)
            {
                var self = _jewelSlots[i];

                if (!self.Item.IsAir)
                    self.Item.TurnToAir();
            }

            _displayJewelItems = new Item[JewelSlots] { null, null, null, null, null };
        }
        else
        {
            _displayJewel = new bool[JewelSlots] { false, false, false, false, false };

            for (int i = 0; i < _jewelSlots.Length; i++)
            {
                if (Jewelry.Info.Count <= i)
                    break;
                
                var self = _jewelSlots[i];
                var info = Jewelry.Info[i];
                bool similarSubs = true;

                if (_displayJewelItems[i] is not null)
                {
                    var displayJewel = _displayJewelItems[i].ModItem as Jewel;

                    for (int j = 0; j < displayJewel.info.SubStats.Count; j++)
                    {
                        var displaySub = displayJewel.info.SubStats[j];
                        var currentSub = Jewelry.Info[i].SubStats[j];

                        if (displaySub.Type != currentSub.Type || displaySub.Strength != currentSub.Strength)
                        {
                            similarSubs = false;
                            break;
                        }
                    }
                }

                if (_displayJewelItems[i] is null || (_displayJewelItems[i].ModItem as Jewel).info.Major.Type != info.Major.Type || !similarSubs)
                {
                    Item item = new(info is MajorJewelInfo ? ModContent.ItemType<MajorJewel>() : ModContent.ItemType<MinorJewel>());
                    (item.ModItem as Jewel).info = info;
                    _displayJewelItems[i] = item;
                }

                _displayJewel[i] = true;
                self.ForceItem(_displayJewelItems[i].Clone());
            }
        }
    }

    private void SetDialoguePanel()
    {
        Append(new UINPCDialoguePanel()
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(174),
            Width = StyleDimension.FromPixels(280),
            Height = StyleDimension.FromPixels(600)
        });
    }

    private void SetJewel(UIMouseEvent evt, UIElement listeningElement)
    {
        if (_jewelrySlot.Item.IsAir)
            return;

        if (!_jewelSlots.Any(x => !x.Item.IsAir))
            return;

        if (Jewelry.Info.Count >= Jewelry.Info.Capacity || Jewelry.Info.Count >= JewelSlots)
            return;

        for (int i = 0; i < Jewelry.Info.Capacity; ++i) 
        {
            if (!_jewelSlots[i].HasItem || _displayJewel[i])
                continue;

            var jewel = _jewelSlots[i].Item.ModItem as Jewel;
            Jewelry.Info.Add(jewel.info);
            _jewelSlots[i].Item.TurnToAir();

            if (Jewelry.Info.Count >= Jewelry.Info.Capacity || Jewelry.Info.Count >= JewelSlots)
                break;
        }

        Main.npcChatText = "There you are! Proud of this one.";
    }

    private static bool CanJewelrySlotAcceptItem(Item item, ItemSlotUI _)
    {
        bool isMouseItem = Main.LocalPlayer.selectedItem == 58 && Main.LocalPlayer.HeldItem == item;
        return item.ModItem is BasicJewelry || item.IsAir || !isMouseItem;
    }

    public void Close()
    {
        if (_jewelrySlot.HasItem)
            Main.LocalPlayer.QuickSpawnItem(new EntitySource_OverfullInventory(Main.LocalPlayer), _jewelrySlot.Item);

        foreach (var slot in _jewelSlots)
        {
            if (slot.HasItem)
                Main.LocalPlayer.QuickSpawnItem(new EntitySource_OverfullInventory(Main.LocalPlayer), slot.Item);
        }

        foreach (var slot in _supportSlots)
        {
            if (slot.HasItem)
                Main.LocalPlayer.QuickSpawnItem(new EntitySource_OverfullInventory(Main.LocalPlayer), slot.Item);
        }
    }
}