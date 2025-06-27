using PeculiarJewelry.Content.Items;
using PeculiarJewelry.Content.Items.Jewels;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace PeculiarJewelry.Content.JewelryMechanic.UI.Superimposition;

internal class SuperimpositionUIState : UIState, IClosableUIState
{
    private readonly List<JewelStat> _storedStats = [];
    private readonly Dictionary<JewelStat, bool> _storedStatsSide = [];

    private UIPanel _helpPanel;
    private ItemSlotUI _leftJewel;
    private ItemSlotUI _rightJewel;
    private ItemSlotUI _resultJewel;
    private ItemSlotUI _subShardSlot;
    private JewelSubstatUI _leftStats;
    private JewelSubstatUI _rightStats;
    private JewelTriggerUI _leftTrigger;
    private JewelTriggerUI _rightTrigger;
    private bool _minor = false;
    private (bool leftSide, bool isContext)? _triggerChoice = null;
    private string _triggerType;

    internal static string Localize(string postfix) => Language.GetTextValue("Mods.PeculiarJewelry.UI.Superimposition." + postfix);

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _minor = _leftJewel.Item.ModItem is MinorJewel || _rightJewel.Item.ModItem is MinorJewel;

        if (_leftJewel.Item.ModItem is Jewel jewel)
        {
            if (!_leftStats.Showing)
                _leftStats.RebuildStats(jewel);

            if (!_minor && !_leftTrigger.Showing)
                _leftTrigger.RebuildStats(jewel);
        }

        if (_leftJewel.Item.ModItem is not Jewel)
        {
            if (_leftStats.Showing)
            {
                _leftStats.Hide(true);
                ClearUnusedStats(true);
            }

            if (!_minor)
            {
                _leftTrigger.Hide();
                _rightTrigger.Highlight(UICommon.DefaultUIBlue, UICommon.DefaultUIBlue, true);
            }
        }

        if (_rightJewel.Item.ModItem is Jewel rightJewel)
        {
            if (!_rightStats.Showing)
                _rightStats.RebuildStats(rightJewel);

            if (!_minor && !_rightTrigger.Showing)
                _rightTrigger.RebuildStats(rightJewel);
        }

        if (_rightJewel.Item.ModItem is not Jewel)
        {
            if (_rightStats.Showing)
            {
                _rightStats.Hide(true);
                ClearUnusedStats(false);
            }

            if (!_minor)
            {
                _leftTrigger.Highlight(UICommon.DefaultUIBlue, UICommon.DefaultUIBlue, true);
                _rightTrigger.Hide();
            }
        }
    }

    private void ClearUnusedStats(bool left)
    {
        List<JewelStat> stats = new();
        foreach (var pair in _storedStatsSide)
            if (pair.Value == left)
                stats.Add(pair.Key);

        foreach (var item in stats)
        {
            _storedStatsSide.Remove(item);
            _storedStats.Remove(item);
        }
    }

    public override void OnInitialize()
    {
        SetDialoguePanel();
        SetSuperimpositionPanel();
    }

    private void SetSuperimpositionPanel()
    {
        UIPanel panel = new() // Main back panel
        {
            Width = StyleDimension.FromPixels(340),
            Height = StyleDimension.FromPixels(474),
            Top = StyleDimension.FromPixels(30),
            HAlign = 0.5f,
        };
        Append(panel);

        panel.Append(new UIText(Localize("Superimposition"))
        {
            HAlign = 0.5f,
        });

        Item air = new();
        air.TurnToAir();
        _leftJewel = new ItemSlotUI([air], 0, (item, ui) => CanJewelSlotAcceptItem(item, true))
        {
            Left = StyleDimension.FromPixels(10)
        };
        panel.Append(_leftJewel);

        _rightJewel = new ItemSlotUI([air], 0, (item, ui) => CanJewelSlotAcceptItem(item, false))
        {
            Left = StyleDimension.FromPixelsAndPercent(-54, 1)
        };
        panel.Append(_rightJewel);

        panel.Append(new UIText(Localize("Stats"))
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(40)
        });

        _leftStats = new JewelSubstatUI((stat) => TryAddStat(stat, true))
        {
            Width = StyleDimension.FromPixels(154),
            Height = StyleDimension.FromPixels(150),
            Top = StyleDimension.FromPixels(70)
        };
        panel.Append(_leftStats);

        _rightStats = new JewelSubstatUI((stat) => TryAddStat(stat, false))
        {
            Width = StyleDimension.FromPixels(154),
            Height = StyleDimension.FromPixels(150),
            Top = StyleDimension.FromPixels(70),
            HAlign = 1f,
        };
        panel.Append(_rightStats);

        panel.Append(new UIText(Localize("Effects"))
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(226)
        });

        _leftTrigger = new JewelTriggerUI((fx, isContext) => SetTrigger(fx, isContext, true))
        {
            Width = StyleDimension.FromPixels(154),
            Height = StyleDimension.FromPixels(90),
            Top = StyleDimension.FromPixels(250)
        };
        panel.Append(_leftTrigger);

        _rightTrigger = new JewelTriggerUI((fx, isContext) => SetTrigger(fx, isContext, false))
        {
            Width = StyleDimension.FromPixels(154),
            Height = StyleDimension.FromPixels(90),
            Top = StyleDimension.FromPixels(250),
            HAlign = 1f,
        };
        panel.Append(_rightTrigger);

        panel.Append(new UIText(Localize("Result"))
        {
            HAlign = 0.25f,
            Top = StyleDimension.FromPixels(344)
        });

        _resultJewel = new ItemSlotUI([air], 0, (item, ui) => true)
        {
            HAlign = 0.25f,
            Top = StyleDimension.FromPixels(366)
        };
        panel.Append(_resultJewel);

        UIButton<string> generateButton = new(Localize("Combine"))
        {
            HAlign = 0.2f,
            VAlign = 1f,
            Width = StyleDimension.FromPixels(100),
            Height = StyleDimension.FromPixels(34)
        };
        generateButton.OnLeftClick += GenerateJewel;
        panel.Append(generateButton);

        panel.Append(new UIText(Localize("Shard"))
        {
            HAlign = 0.78f,
            Top = StyleDimension.FromPixels(344)
        });

        _subShardSlot = new ItemSlotUI([air], 0, (item, ui) => CanSubShardSlotAcceptItem(item))
        {
            HAlign = 0.75f,
            Top = StyleDimension.FromPixels(366)
        };
        panel.Append(_subShardSlot);

        UIButton<string> questionButton = new($"?")
        {
            Width = StyleDimension.FromPixels(30),
            Height = StyleDimension.FromPixels(30),
            HAlign = 0.55f,
            VAlign = 1f
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
                Height = StyleDimension.FromPixels(320),
                Top = StyleDimension.FromPixels(516),
                HAlign = 0.5f,
                Left = StyleDimension.FromPixels(-340)
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

    private void GenerateJewel(UIMouseEvent evt, UIElement listeningElement)
    {
        if (!_leftJewel.HasItem || !_rightJewel.HasItem)
            return;

        if (_storedStats.Count != (_minor ? 1 : 2))
            return;

        if (!_minor && _triggerChoice is null)
            return;

        Item jewelItem = new(_minor ? ModContent.ItemType<MinorJewel>() : ModContent.ItemType<MajorJewel>());
        Jewel jewel = jewelItem.ModItem as Jewel;
        jewel.info.tier = (_leftJewel.Item.ModItem as Jewel).info.tier;
        jewel.info.SubStats.Clear();

        foreach (var item in _storedStats)
            jewel.info.SubStats.Add(item);

        Item airItem = new(0);
        airItem.TurnToAir();

        if (_subShardSlot.HasItem)
        {
            jewel.info.SubStats.Add((_subShardSlot.Item.ModItem as SubShard).stat);
            _subShardSlot.ForceItem(airItem);
        }

        if (jewel is MajorJewel major)
        {
            var majorInfo = major.info as MajorJewelInfo;
            majorInfo.effect = Activator.CreateInstance(Type.GetType(_triggerType)) as TriggerEffect;
            var (leftSide, isContext) = _triggerChoice.Value;
            TriggerContext context = ((_rightJewel.Item.ModItem as MajorJewel).info as MajorJewelInfo).effect.Context;

            if ((leftSide == true && isContext == true) || (leftSide == false && isContext == false))
                context = ((_leftJewel.Item.ModItem as MajorJewel).info as MajorJewelInfo).effect.Context;

            majorInfo.effect.ForceSetContext(context);
        }

        jewel.info.Major = (_leftJewel.Item.ModItem as Jewel).info.Major;
        jewel.info.Major.Strength = JewelryCommon.StatStrengthRange((_leftJewel.Item.ModItem as Jewel).info);

        _leftJewel.ForceItem(airItem);
        _rightJewel.ForceItem(airItem);
        _resultJewel.ForceItem(jewelItem);
    }

    private void SetTrigger(TriggerEffect fx, bool isContext, bool isLeft)
    {
        _triggerChoice = (isLeft, isContext);

        Color select = Color.Lerp(new Color(210, 210, 90), UICommon.DefaultUIBlue, 0.6f);

        if (isLeft)
        {
            _rightTrigger.Highlight(select, UICommon.DefaultUIBlue, !isContext);
            _leftTrigger.Highlight(select, UICommon.DefaultUIBlue, isContext);
        }
        else
        {
            _rightTrigger.Highlight(select, UICommon.DefaultUIBlue, isContext);
            _leftTrigger.Highlight(select, UICommon.DefaultUIBlue, !isContext);
        }

        if (isContext)
            _triggerType = fx.GetType().AssemblyQualifiedName;
        else
            _triggerType = (((isLeft ? _leftJewel : _rightJewel).Item.ModItem as MajorJewel).info as MajorJewelInfo).effect.GetType().AssemblyQualifiedName;
    }

    private void TryAddStat(JewelStat stat, bool left)
    {
        if (_minor)
        {
            if (_storedStats.Any())
                _storedStats.RemoveAt(0);

            _storedStats.Add(stat);
        }
        else
        {
            if (!_storedStats.Contains(stat))
            {
                var side = _storedStats.FirstOrDefault(x =>
                {
                    KeyValuePair<JewelStat, bool>? first = _storedStatsSide.FirstOrDefault(x => x.Value == left);

                    if (first is null)
                        return false;

                    return x == first.Value.Key;
                });

                bool hasAnySameType = _storedStatsSide.Any(x => x.Key.Type == stat.Type);

                if (hasAnySameType)
                    return;

                if (side is not null)
                {
                    _storedStatsSide.Remove(side);
                    _storedStats.Remove(side);
                }

                if (!_storedStatsSide.ContainsValue(left))
                {
                    _storedStats.Add(stat);
                    _storedStatsSide.Add(stat, left);
                }
            }
        }

        Color select = Color.Lerp(new Color(210, 210, 90), UICommon.DefaultUIBlue, 0.6f);
        _leftStats.Highlight(select, UICommon.DefaultUIBlue, _storedStats);
        _rightStats.Highlight(select, UICommon.DefaultUIBlue, _storedStats);
    }

    private bool CanJewelSlotAcceptItem(Item item, bool isLeft)
    {
        bool isMouseItem = Main.LocalPlayer.selectedItem == 58 && Main.LocalPlayer.HeldItem == item;
        bool isCompatible = true;
        string otherName = string.Empty;

        if (isLeft && _rightJewel.HasItem)
            otherName = GetExistingJewelName(_rightJewel.Item.ModItem as Jewel);
        else if (!isLeft && _leftJewel.HasItem)
            otherName = GetExistingJewelName(_leftJewel.Item.ModItem as Jewel);

        if (otherName != string.Empty && item.ModItem is Jewel jewelForName)
        {
            string name = GetExistingJewelName(jewelForName);
            isCompatible = name == otherName;
        }

        return isCompatible && ((item.ModItem is Jewel jewel && jewel.info.SubStats.Any()) || item.IsAir || !isMouseItem);
    }

    private bool CanSubShardSlotAcceptItem(Item item)
    {
        bool isMouseItem = Main.LocalPlayer.selectedItem == 58 && Main.LocalPlayer.HeldItem == item;
        return (item.ModItem is SubShard shard && !_storedStats.Any(x => x.Type == shard.stat.Type)) || item.IsAir || !isMouseItem;
    }

    internal static string GetExistingJewelName(Jewel jewel)
    {
        List<TooltipLine> lines = [new TooltipLine(ModLoader.GetMod("PeculiarJewelry"), "ItemName", "")];
        Jewel.PlainJewelTooltips(lines, jewel.info, jewel, true);
        string jewelName = lines.First(x => x.Name == "ItemName").Text;
        return jewelName;
    }

    private void SetDialoguePanel()
    {
        Append(new UINPCDialoguePanel()
        {
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(268),
            Width = StyleDimension.FromPixels(300),
            Height = StyleDimension.FromPixels(600)
        });
    }

    public void Close()
    {
        if (_leftJewel.HasItem)
            Main.LocalPlayer.QuickSpawnItem(new EntitySource_OverfullInventory(Main.LocalPlayer), _leftJewel.Item, _leftJewel.Item.stack);

        if (_rightJewel.HasItem)
            Main.LocalPlayer.QuickSpawnItem(new EntitySource_OverfullInventory(Main.LocalPlayer), _rightJewel.Item, _rightJewel.Item.stack);

        if (_subShardSlot.HasItem)
            Main.LocalPlayer.QuickSpawnItem(new EntitySource_OverfullInventory(Main.LocalPlayer), _subShardSlot.Item, _subShardSlot.Item.stack);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        float oldScale = Main.inventoryScale;
        Main.inventoryScale = 0.9f;
        base.Draw(spriteBatch);
        Main.inventoryScale = oldScale;
    }
}
