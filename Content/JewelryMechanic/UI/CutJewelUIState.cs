using PeculiarJewelry.Content.Items;
using PeculiarJewelry.Content.Items.Jewels;
using PeculiarJewelry.Content.Items.JewelSupport;
using PeculiarJewelry.Content.Items.RadiantEchoes;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace PeculiarJewelry.Content.JewelryMechanic.UI;

internal class CutJewelUIState : UIState, IClosableUIState
{
    private const int CutPanelHeight = 180;
    private const int RequiredEchosPerUpgrade = 1;

    private Jewel JewelItem => _storedItem.ModItem as Jewel;

    private ItemSlotUI _cutSlot = null;
    private ItemSlotUI[] _supportItems = null;
    private Item _storedItem = null;
    private bool _hasJewel = false;
    private UIPanel _statPanel = null;
    private UIPanel _helpPanel = null;
    private bool _hoveringAnvil = false;

    // Info stuff
    private UIText _infoText = null;
    private string _statusText = string.Empty;

    internal static string Localize(string postfix) => Language.GetTextValue("Mods.PeculiarJewelry.UI.CutMenu." + postfix);

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _storedItem = _cutSlot.Item;
        _hasJewel = _storedItem.ModItem is Jewel;
        ResetStatPanelWith(_storedItem);
        UpdateInfo();
    }

    private void UpdateInfo(string status = null)
    {
        static string HighlightStat(int value, int cutoff)
        {
            string white = "FFFFFF";
            string red = "FF0000";
            return $"[c/{(value >= cutoff ? white : red)}:{value}]";
        }

        string noJewel = Localize("NoJewel");
        float price = _hasJewel ? JewelCutCoinPrice(JewelItem.info) : 0;
        int goldCoins = (int)(Utils.CoinsCount(out bool _, Main.LocalPlayer.inventory) / Item.buyPrice(0, 1, 0, 0));
        int endCoins = Utils.CoinsSplit((long)price)[2];
        string coinPrice = !_hasJewel ? noJewel : HighlightStat(goldCoins, endCoins) + "/" + endCoins + $" {Localize("Gold")} [i:{ItemID.GoldCoin}]";

        int dustPriceNum = _hasJewel ? JewelCutDustPrice(JewelItem.info) : 0;
        string dustPriceText = HighlightStat(Main.LocalPlayer.CountItem(ModContent.ItemType<SparklyDust>(), dustPriceNum), dustPriceNum);
        string dustPrice = !_hasJewel ? "" : dustPriceText + "/" + dustPriceNum.ToString() + $" {Localize("Dust")} [i:{ModContent.ItemType<SparklyDust>()}]";

        float delta = -1;
        float chance = _hasJewel ? JewelCutChance(JewelItem.info, _supportItems, out delta) : -1;
        float chanceAmount = Math.Min(chance * 100, 100);

        if (delta == -1)
            delta = chanceAmount;

        bool thresholdCut = _hasJewel && JewelItem.info.InThresholdCut();
        string echoCost = "";

        if (thresholdCut)
        {
            int itemID = JewelCutEchoType(JewelItem.info.cuts);
            echoCost = $"0/{RequiredEchosPerUpgrade} [i:{itemID}]\n({Lang.GetItemNameValue(itemID)})";
        }

        string hex;

        if (chanceAmount > 80)
            hex = "00FF00";
        else if (chanceAmount > 40)
            hex = "FFFF00";
        else if (chanceAmount > 0)
            hex = "FF0000";
        else
            hex = "000000";

        string cutChance = chance == -1 ? "" : $"[c/{hex}:{chanceAmount:#0.##}]% ([c/{(delta >= 0 ? "00FF00" : "FF0000")}:{delta * 100:#0.##}%]) {Localize("SuccessChance")}";

        if (!_hasJewel)
            _statusText = string.Empty;
        else if (status is not null)
            _statusText = status;

        _infoText.SetText(coinPrice + "\n" + dustPrice + "\n" + cutChance + "\n" + echoCost + "\n" + _statusText);
    }

    private static int JewelCutCoinPrice(JewelInfo info) => Item.buyPrice(0, 1) * ((int)info.tier + info.successfulCuts + 1);
    public static int JewelCutDustPrice(JewelInfo info) => JewelCutDustPrice(info.tier, info.successfulCuts);

    public static int JewelCutDustPrice(JewelTier tier, int successfulCuts) => ((int)tier+ 1) * (successfulCuts + 1);

    public static int JewelCutEchoType(int cuts) => cuts switch
    {
        7 => ModContent.ItemType<RadiantEcho>(),
        15 => ModContent.ItemType<DoubleRadiantEcho>(),
        23 => ModContent.ItemType<TripleRadiantEcho>(),
        31 => ModContent.ItemType<QuadRadiantEcho>(),
        39 => ModContent.ItemType<QuintRadiantEcho>(),
        47 => ModContent.ItemType<SextupleRadiantEcho>(),
        55 => ModContent.ItemType<SeptRadiantEcho>(),
        63 => ModContent.ItemType<OctoRadiantEcho>(),
        71 => ModContent.ItemType<NonaRadiantEcho>(),
        _ => throw new ArgumentException("Uh oh! You aren't in a threshold but you are somehow."),
    };

    private static float JewelCutChance(JewelInfo info, ItemSlotUI[] support, out float delta, bool consumeSupport = true)
    {
        delta = 0f;
        float originalChance = 1f - (info.successfulCuts * 0.05f);
        float baseChance = originalChance;
        float baseOverrideChance = -1;
        Dictionary<int, int> itemCountsById = new();

        foreach (var item in support)
        {
            if (itemCountsById.ContainsKey(item.Item.type))
                itemCountsById[item.Item.type]++;
            else
                itemCountsById.Add(item.Item.type, 1);
        }

        foreach (var item in support)
        {
            if (item.Item.ModItem is not JewelSupportItem supportItem)
                continue;

            if (supportItem.HardOverrideJewelCutChance(info, out float newChance))
            {
                if (baseOverrideChance == -1 || newChance > baseOverrideChance)
                    baseOverrideChance = newChance;
            }
        }

        if (baseOverrideChance != -1)
        {
            if (!consumeSupport)
                ClearSupportItems(support);

            delta = baseOverrideChance;
            return baseOverrideChance;
        }

        foreach (var item in support)
        {
            if (item.Item.ModItem is not JewelSupportItem supportItem)
                continue;

            baseChance = (item.Item.ModItem as JewelSupportItem).ModifyJewelCutChance(info, baseChance);
        }

        if (!consumeSupport)
            ClearSupportItems(support);

        delta = baseChance - originalChance;
        return baseChance;
    }

    private static void ClearSupportItems(ItemSlotUI[] support, int onlyType = -1)
    {
        foreach (var item in support)
        {
            if (!item.HasItem)
                continue;

            if (onlyType != -1 && item.Item.type != onlyType)
                continue;

            item.Item.stack--;

            if (item.Item.stack <= 0)
                item.Item.TurnToAir();
        }
    }

    public override void OnInitialize()
    {
        _supportItems = new ItemSlotUI[6];
        SetDialoguePanel();
        SetCutPanel();
        SetPriceInfoPanel();
        SetStatPanel();
    }

    private void SetStatPanel()
    {
        _statPanel = new() // Main back panel
        {
            Width = StyleDimension.FromPixels(360),
            Height = StyleDimension.FromPixels(CutPanelHeight + 80),
            Left = StyleDimension.FromPixelsAndPercent(166, 0.5f),
            Top = StyleDimension.FromPercent(0.15f),
        };
        Append(_statPanel);

        ResetStatPanelWith(_cutSlot.Item);
    }

    private void ResetStatPanelWith(Item item)
    {
        _statPanel.RemoveAllChildren();

        int width = -1;

        if (item.IsAir || item.ModItem is not Jewel)
            _statPanel.Append(new UIText(Localize("NoJewel")));
        else
        {
            var subStats = (item.ModItem as Jewel).info.SubStats;
            var tooltips = new List<TooltipLine>() { new TooltipLine(ModLoader.GetMod("PeculiarJewelry"), "ItemName", "Item") };
            item.ModItem.ModifyTooltips(tooltips);

            if (_hoveringAnvil && (item.ModItem as Jewel).info.successfulCuts % 4 == 3)
                tooltips.Add(new TooltipLine(ModLoader.GetMod("PeculiarJewelry"), "SubstatUpgrade", Localize("SubstatUpgrade") + 
                    (subStats.Count == subStats.Capacity ? Localize("Upgraded") : Localize("Added"))));

            for (int i = 0; i < tooltips.Count; i++)
            {
                var line = tooltips[i];
                string display = TextModification(line, (item.ModItem as Jewel).info, out Color? overrideColor);
                var color = overrideColor ?? Color.White;

                var text = new UIText(display)
                {
                    Top = StyleDimension.FromPixels(20 * i),
                    TextColor = color
                };
                _statPanel.Append(text);

                Vector2 vector = FontAssets.MouseText.Value.MeasureString(display);

                if (width < vector.X)
                    width = (int)vector.X + 20;
            }
        }

        if (width != -1)
            _statPanel.Width = StyleDimension.FromPixels(width);
        _statPanel.Height = StyleDimension.FromPixels(Math.Max(_statPanel.Children.Count(), 2) * 22 + 6);
    }

    private string TextModification(TooltipLine text, JewelInfo info, out Color? overrideColor)
    {
        // This whole method is really messy and it sucks and I'm not going to fix it
        // Pay me a ton of money and it'll be better code and otherwise too bad

        overrideColor = null;

        if (info.cuts == info.MaxCuts)
        {
            overrideColor = Color.Gray;
            return text.Text;
        }

        if (!_hoveringAnvil)
            return text.Text;

        static string Replacement(string text, out bool invalidText)
        {
            invalidText = false;

            if (!text.Contains('+'))
            {
                invalidText = true;
                return text;
            }

            int plus = text.IndexOf('+') + 1;
            int percent = text.IndexOf('%', plus);
            int space = text.IndexOf(' ', plus);
            int end = percent;

            if ((space < percent && space != 0) || percent < 0)
                end = space;

            if ((percent < 0 && space < 0) || plus - end > 0)
            {
                invalidText = true;
                return text;
            }

            return text[plus..end];
        }

        if (text.Name == "JewelCuts")
        {
            string replacement = text.Text[..(text.Text.IndexOf("/") + 1)];
            return text.Text.Replace(replacement, "[c/ffff00:" + (float.Parse(replacement.Replace("/", "")) - 1).ToString() + "]/");
        }

        if (text.Name == "SubstatUpgrade")
        {
            overrideColor = Color.LightBlue;
            return text.Text;
        }

        if (text.Name.StartsWith("MajorStat"))
        {
            overrideColor = Color.Yellow;

            string replacement = Replacement(text.Text, out bool _);
            float current = info.Major.GetEffectValue(Main.LocalPlayer, PeculiarJewelry.StatConfig.GlobalPowerScaleMinimum);
            return text.Text.Replace(replacement, "[c/ffa500:" + current.ToString("#0.##") + " - " + info.Major.GetEffectValue(Main.LocalPlayer, 1f).ToString("#0.##") + "]");
        }

        if (text.Name.StartsWith("SubStat"))
        {
            overrideColor = Color.LightBlue;

            int statID = int.Parse(text.Name[^1].ToString());
            string replacement = Replacement(text.Text, out bool invalidText);

            if (invalidText)
                return text.Text;

            float current = info.SubStats[statID].GetEffectValue(Main.LocalPlayer, PeculiarJewelry.StatConfig.GlobalPowerScaleMinimum);
            return text.Text.Replace(replacement, "[c/ffa500:" + current.ToString("#0.##") + " - " + info.SubStats[statID].GetEffectValue(Main.LocalPlayer, 1f).ToString("#0.##") + "]");
        }

        return text.Text;
    }

    private void SetPriceInfoPanel()
    {
        UIPanel infoPanel = new() // Main back panel
        {
            Width = StyleDimension.FromPixels(240),
            Height = StyleDimension.FromPixels(CutPanelHeight + 30),
            Left = StyleDimension.FromPixelsAndPercent(-404, 0.5f),
            Top = StyleDimension.FromPercent(0.15f),
        };
        Append(infoPanel);

        _infoText = new(string.Empty)
        {
            IsWrapped = false,
            Width = StyleDimension.Fill
        };
        infoPanel.Append(_infoText);
    }

    private void SetCutPanel()
    {
        UIPanel panel = new() // Main back panel
        {
            Width = StyleDimension.FromPixels(320),
            Height = StyleDimension.FromPixels(CutPanelHeight + 30),
            Top = StyleDimension.FromPercent(0.15f),
            HAlign = 0.5f,
        };
        Append(panel);

        UIImageButton close = new(ModContent.Request<Texture2D>("PeculiarJewelry/Content/JewelryMechanic/UI/Close"))
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

        Item air = new();
        air.TurnToAir();
        _cutSlot = new([air], 0, CanJewelSlotAcceptItem)
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(20)
        };
        panel.Append(_cutSlot);

        UIText cutText = new(Localize("Name"))
        {
            HAlign = 0.5f,
            TextColor = Color.Aquamarine,
        };
        panel.Append(cutText);

        Main.instance.LoadItem(ItemID.IronAnvil);
        UIImageButton button = new(TextureAssets.Item[ItemID.IronAnvil])
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(98),
            Width = StyleDimension.FromPixels(30),
            Height = StyleDimension.FromPixels(30),
        };
        button.OnLeftClick += TryCutJewel;
        button.OnMouseOver += (UIMouseEvent evt, UIElement listeningElement) => _hoveringAnvil = true;
        button.OnMouseOut += (UIMouseEvent evt, UIElement listeningElement) => _hoveringAnvil = false;
        panel.Append(button);

        UIText cutJewelText = new(Localize("Cut"))
        {
            Top = StyleDimension.FromPixels(-22)
        };
        button.Append(cutJewelText);

        UIText supportText = new(Localize("Support"))
        {
            Top = StyleDimension.FromPixels(CutPanelHeight - 72),
            Left = StyleDimension.FromPixels(20),
        };
        panel.Append(supportText);

        int max = _supportItems.Length;
        for (int i = 0; i < max; ++i)
        {
            _supportItems[i] = new ItemSlotUI([air], 0, CanInputSupportItem)
            {
                HAlign = (float)i / (max - 1),
                VAlign = 0.95f
            };
            panel.Append(_supportItems[i]);
        }

        UIButton<string> questionButton = new($"?")
        {
            Width = StyleDimension.FromPixels(30),
            Height = StyleDimension.FromPixels(30),
            HAlign = 1,
            VAlign = 0
        };
        questionButton.OnLeftClick += ToggleQuestionPanel;
        panel.Append(questionButton);
    }

    private void ToggleQuestionPanel(UIMouseEvent evt, UIElement listeningElement)
    {
        if (_helpPanel is null)
        {
            _helpPanel = new()
            {
                Width = StyleDimension.FromPixels(450),
                Height = StyleDimension.FromPixels(290),
                Top = StyleDimension.FromPixels(368),
                HAlign = 0.5f,
                Left = StyleDimension.FromPixels(-332)
            };
            Append(_helpPanel);

            _helpPanel.Append(new UIText(Localize("Help"), 0.9f)
            {
                IsWrapped = true,
                Width = StyleDimension.Fill,
                Height = StyleDimension.Fill,
            });
        }
        else
        {
            RemoveChild(_helpPanel);
            _helpPanel = null;
        }
    }

    public static bool CanJewelSlotAcceptItem(Item item, ItemSlotUI _)
    {
        bool isMouseItem = Main.LocalPlayer.selectedItem == 58 && Main.LocalPlayer.HeldItem == item;
        return item.ModItem is Jewel || item.IsAir || !isMouseItem;
    }

    private bool CanInputSupportItem(Item item, ItemSlotUI self)
    {
        if (JewelItem is null)
            return false;

        if (item.ModItem is not JewelSupportItem && !item.IsAir && item == Main.mouseItem)
            return false;

        foreach (var slot in _supportItems)
            if (!slot.Item.IsAir && item.type == slot.Item.type)
                return false;
        
        return item.ModItem is not JewelSupportItem support || support.CanBePlacedInSupportSlot(JewelItem.info);
    }

    private void SetDialoguePanel()
    {
        Append(new UINPCDialoguePanel()
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixelsAndPercent(100, 0.15f),
            Width = StyleDimension.FromPixels(320),
            Height = StyleDimension.FromPixels(600)
        });
    }

    private void TryCutJewel(UIMouseEvent evt, UIElement listeningElement)
    {
        if (!_hasJewel)
        {
            UpdateInfo(Localize("MissingJewel"));
            return; // No jewel stored
        }

        JewelInfo info = JewelItem.info;

        if (info.cuts >= info.MaxCuts)
        {
            UpdateInfo(Localize("NoCuts"));
            return; // Too many cuts
        }

        bool hasLuckyCoin = _supportItems.Any(x => !x.Item.IsAir && x.Item.type == ModContent.ItemType<LuckyCoin>());

        if (!hasLuckyCoin) // Skip all price checks and consumption checks
        {
            int coinPrice = JewelCutCoinPrice(info);
            if (Utils.CoinsCount(out bool overflow, Main.LocalPlayer.inventory) < coinPrice || overflow)
            {
                UpdateInfo(Localize("NoCoins"));
                return;
            }

            int dustPrice = JewelCutDustPrice(info);
            if (Main.LocalPlayer.CountItem(ModContent.ItemType<SparklyDust>(), dustPrice) < dustPrice)
            {
                UpdateInfo(Language.GetText("Mods.PeculiarJewelry.UI.CutMenu.NoDust").WithFormatArgs(ModContent.ItemType<SparklyDust>()).Value);
                return;
            }

            if (info.InThresholdCut())
            {
                int echoType = JewelCutEchoType(info.cuts);
                int count = Main.LocalPlayer.CountItem(echoType, RequiredEchosPerUpgrade) + Main.LocalPlayer.CountItem(ModContent.ItemType<TranscendantEcho>(), RequiredEchosPerUpgrade);

                if (count < RequiredEchosPerUpgrade)
                {
                    UpdateInfo($"{Localize("NotEnough")} [i:{echoType}]\n[c/ff0000:({Lang.GetItemNameValue(echoType)})]!");
                    return;
                }
            }

            for (int i = 0; i < dustPrice; ++i) // Consume dust
                Main.LocalPlayer.ConsumeItem(ModContent.ItemType<SparklyDust>(), true);
            
            Main.LocalPlayer.BuyItem(coinPrice); // Consume coins
        }

        TryConsumeEchoes(info);

        bool success = info.TryAddCut(JewelCutChance(info, _supportItems, out _, !hasLuckyCoin));
        SoundEngine.PlaySound(SoundID.NPCHit4, Main.LocalPlayer.Center);

        if (hasLuckyCoin)
            ClearSupportItems(_supportItems, ModContent.ItemType<LuckyCoin>());

        if (success)
        {
            UpdateInfo(Localize("SuccessfulCut"));
            Main.npcChatText = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.SuccessfulCuts." + Main.rand.Next(3));
        }
        else
        {
            UpdateInfo(Localize("FailedCut"));
            Main.npcChatText = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.FailedCuts." + Main.rand.Next(3));
        }
    }

    private static void TryConsumeEchoes(JewelInfo info)
    {
        if (info.InThresholdCut())
        {
            int count = RequiredEchosPerUpgrade;
            int transcendantCount = Main.LocalPlayer.CountItem(ModContent.ItemType<TranscendantEcho>(), 2);

            if (transcendantCount > 0)
            {
                int min = Math.Min(count, transcendantCount);

                for (int i = 0; i < min; ++i)
                    Main.LocalPlayer.ConsumeItem(ModContent.ItemType<TranscendantEcho>(), true);
            }

            count -= transcendantCount;

            if (count > 0)
            {
                int echoType = JewelCutEchoType(info.cuts);

                for (int i = 0; i < count; ++i)
                    Main.LocalPlayer.ConsumeItem(echoType, true);
            }
        }
    }

    public void Close()
    {
        if (_cutSlot.HasItem)
            Main.LocalPlayer.QuickSpawnItem(new EntitySource_OverfullInventory(Main.LocalPlayer), _cutSlot.Item, _cutSlot.Item.stack);

        foreach (var slot in _supportItems)
        {
            if (slot.HasItem)
                Main.LocalPlayer.QuickSpawnItem(new EntitySource_OverfullInventory(Main.LocalPlayer), slot.Item, slot.Item.stack);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        float oldScale = Main.inventoryScale;
        Main.inventoryScale = 0.9f;
        base.Draw(spriteBatch);
        Main.inventoryScale = oldScale;
    }
}
