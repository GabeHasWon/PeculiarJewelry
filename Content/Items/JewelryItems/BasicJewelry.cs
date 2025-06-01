using PeculiarJewelry.Content.Items.Jewels;
using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses;
using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;
using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using PeculiarJewelry.Content.JewelryMechanic.Stats.IO;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Stats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace PeculiarJewelry.Content.Items.JewelryItems;

public abstract class BasicJewelry : ModItem, IStorableItem
{
    public enum JewelryTier : byte
    {
        Ordinary,
        Pretty,
        Elegant,
        Elaborate,
        Extravagant,
    }

    public abstract string MaterialCategory { get; }

    public List<JewelInfo> Info { get; protected set; }
    public JewelryTier tier = JewelryTier.Ordinary;

    public sealed override void SetDefaults()
    {
        Item.accessory = true;
        Item.rare = ModContent.RarityType<JewelRarity>();
        Item.value = Item.sellPrice(gold: 1);

        SetTier(JewelryTier.Ordinary);
        Defaults();
    }

    protected virtual void Defaults() { }

    public sealed override void UpdateEquip(Player player)
    {
        if (!Item.accessory)
            player.GetModPlayer<JewelPlayer>().jewelry.Add(this);

        player.GetModPlayer<MaterialPlayer>().AddMaterial(MaterialCategory);
        EquipEffect(player);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<JewelPlayer>().jewelry.Add(this);
    public override void EquipFrameEffects(Player player, EquipType type) => EquipEffect(player, true);
    public override void UpdateVanity(Player player) => EquipEffect(player, true);
    protected virtual void EquipEffect(Player player, bool isVanity = false) { }
    public static string JewelryPrefix(JewelryTier tier) => Language.GetTextValue("Mods.PeculiarJewelry.Jewelry.JewelryPrefixes." + tier);
    public override bool CanReforge() => false;

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        var name = tooltips.First(x => x.Name == "ItemName");

        if (Info.Count != 0)
        {
            string title = GetBestTitleInfo(Info);
            string of = Language.GetTextValue("Mods.PeculiarJewelry.Jewels.Of");
            name.Text = $"{JewelryPrefix(tier)} {name.Text}{of}{title}";
        }
        else
            name.Text = $"{JewelryPrefix(tier)} {name.Text}";

        float count = Main.LocalPlayer.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialCategory);

        if (count >= 3)
            tooltips.Add(new TooltipLine(Mod, "3Set", Language.GetTextValue("Mods.PeculiarJewelry.Material.Bonuses." + MaterialCategory + ".3Set")));

        if (count >= 5)
            tooltips.Add(new TooltipLine(Mod, "5Set", Language.GetTextValue("Mods.PeculiarJewelry.Material.Bonuses." + MaterialCategory + ".5Set")));

        if (!PeculiarJewelry.ShiftDown)
        {
            SummaryJewelryTooltips(tooltips, this, Mod);

            if (Info.Count != 0)
                tooltips.Add(new TooltipLine(Mod, "ShiftNotice", Language.GetTextValue("Mods.PeculiarJewelry.Jewelry.HoldShift")));
        }
        else
            foreach (var item in Info)
                Jewel.PlainJewelTooltips(tooltips, item, this, false);
    }

    private string GetBestTitleInfo(List<JewelInfo> info)
    {
        if (GetSpecialTitle(info, out JewelInfo newInfo))
            return newInfo.Title;

        return DetermineHighestStat(info).Localize();
    }

    private bool GetSpecialTitle(List<JewelInfo> info, out JewelInfo newInfo)
    {
        newInfo = null;

        if (info.Any(x => x.IsRare))
        {
            newInfo = info.Where(x => x.IsRare).First();
            return true;
        }

        if (info.Any(x => x.CountsAsMajor))
        {
            newInfo = info.Where(x => x.CountsAsMajor).OrderBy(x => x.Major.Strength).First();
            return true;
        }

        return false;
    }

    private static StatType DetermineHighestStat(List<JewelInfo> info)
    {
        Dictionary<StatType, int> typesByUpgrades = [];

        if (info.Count == 0)
            return StatType.Potency;

        foreach (var jewel in info)
        {
            List<JewelStat> stats = [jewel.Major, .. jewel.SubStats];

            foreach (var stat in stats)
            {
                if (typesByUpgrades.TryGetValue(stat.Get().Type, out int value))
                    typesByUpgrades[stat.Get().Type] = ++value;
                else
                    typesByUpgrades.Add(stat.Get().Type, 1);
            }
        }

        int max = 0;
        StatType key = StatType.Willpower;

        foreach (var item in typesByUpgrades.Keys)
        {
            if (max < typesByUpgrades[item])
            {
                max = typesByUpgrades[item];
                key = item;
            }
        }

        return key;
    }

    public static void SummaryJewelryTooltips(List<TooltipLine> tooltips, BasicJewelry jewelry, Mod mod, Player player = null, List<JewelInfo> overrideInfo = null)
    {
        List<JewelInfo> info = overrideInfo ?? jewelry.Info;
        Dictionary<StatType, float> strengthsByType = [];
        Dictionary<StatType, Color> colorsByType = [];
        int miscIndex = 0;

        player ??= Main.LocalPlayer;
        jewelry.ApplySingleJewelBonus(player);

        foreach (var item in info)
        {
            List<JewelStat> stats = [item.Major, .. item.SubStats];

            if (item.PreAddStatTooltips(tooltips, jewelry, false))
                continue;

            foreach (var stat in stats)
            {
                if (stat is AmberStatContainer amber)
                {
                    tooltips.Add(new TooltipLine(mod, "AmberLine" + miscIndex++, amber.GetDescription(player, false)) { OverrideColor = stat.Get().Color });
                    continue;
                }
                else if (stat.Type == StatType.None)
                    continue;

                if (strengthsByType.ContainsKey(stat.Type))
                    strengthsByType[stat.Type] += stat.GetEffectValue(player) * player.GetModPlayer<HallowedBonus.HallowedBonusPlayer>().fiveSetPower;
                else
                {
                    strengthsByType.Add(stat.Type, stat.GetEffectValue(player) * player.GetModPlayer<HallowedBonus.HallowedBonusPlayer>().fiveSetPower);
                    colorsByType.Add(stat.Type, stat.Get().Color);
                }
            }
        }

        foreach (var (type, strength) in strengthsByType)
        {
            string desc = Language.GetText("Mods.PeculiarJewelry.Jewelry.StatTypes." + type + ".Description").WithFormatArgs(strength.ToString("#0.##")).Value;
            tooltips.Add(new TooltipLine(mod, "SummaryInfo" + type, desc) { OverrideColor = colorsByType[type] });
        }

        jewelry.ResetSingleJewelBonus(player);
    }

    public override void OnSpawn(IEntitySource source)
    {
        if (source is not EntitySource_Loot { Context: "JewelCrafting" })
            return;

        if (Main.LocalPlayer.GetModPlayer<RevelationPlayer>().hasReveled)
        {
            Main.LocalPlayer.GetModPlayer<RevelationPlayer>().hasReveled = false;
            SetTier(JewelryTier.Extravagant);
            return;
        }

        if (Main.LocalPlayer.GetModPlayer<RichPlayer>().isRich)
            SetTier((JewelryTier)Main.rand.Next((int)JewelryTier.Extravagant + 1));
    }

    public override void SaveData(TagCompound tag)
    {
        tag.Add("jewelryTier", (byte)tier);
        tag.Add("jewelryJewelCount", (byte)Info.Count);

        for (int i = 0; i < Info.Count; ++i)
        {
            JewelInfo item = Info[i];
            tag.Add("jewelryInfo" + i, item.SaveAs());
        }
    }

    public override void LoadData(TagCompound tag)
    {
        JewelryTier tier = tag.TryGet("jewelryTier", out byte tierByte) ? (JewelryTier)tierByte : JewelryTier.Ordinary;
        SetTier(tier);

        byte count = tag.GetByte("jewelryJewelCount");

        for (int i = 0; i < count; ++i)
        {
            var jewelTag = tag.GetCompound("jewelryInfo" + i);

            if (!jewelTag.ContainsKey("infoType"))
                continue;

            JewelInfo newInfo = JewelIO.LoadInfo(jewelTag);
            Info.Add(newInfo);
        }
    }

    public override void NetSend(BinaryWriter writer)
    {
        writer.Write((byte)tier);
        writer.Write(Info.Count);

        foreach (var item in Info)
            JewelIO.SendJewelInfo(item, writer);
    }

    public override void NetReceive(BinaryReader reader)
    {
        SetTier((JewelryTier)reader.ReadByte());
        int count = reader.ReadInt32();

        for (int i = 0; i < count; ++i)
        {
            JewelInfo info = JewelIO.ReadJewelInfo(reader);
            Info.Add(info);
        }
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        if (tier == JewelryTier.Ordinary)
            return;

        Color color = Info.Count > 0 ? GetDisplayColor() : Color.White;
        JewelDrawing.DrawSparks(position - Item.Size / 2f * scale, Item.Size * scale, (int)tier, color, 1f);
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        if (tier == JewelryTier.Ordinary)
            return;

        Color color = Info.Count > 0 ? GetDisplayColor() : Color.White;
        JewelDrawing.DrawSparks(Item.position - Main.screenPosition, Item.Size * scale, (int)tier, color, 1f);
    }

    internal void SetTier(JewelryTier tier)
    {
        this.tier = tier;
        Info = new((int)tier + 1);
    }

    internal void ApplyTo(Player player, float add = 0, float multiplier = 1f)
    {
        foreach (var item in Info)
            item.ApplyTo(player, add, multiplier);
    }

    internal void ApplySingleJewelBonus(Player player) => player.SingleBonus(MaterialCategory, this);
    internal void ResetSingleJewelBonus(Player player) => player.UndoSingle(MaterialCategory, this);

    /// <summary>
    /// Gets how many major jewels can be applied to this jewelry.
    /// </summary>
    /// <returns></returns>
    internal int MaxMajorCount(Player player)
    {
        int count = BaseMaterialBonus.BonusesByKey[MaterialCategory].GetMajorJewelCount;
        count += (int)MathF.Floor(player.GetModPlayer<CatEyePlayer>().StackBonusesByMaterial["Luminite"]); // Apply luminite stack bonus
        return count;
    }

    public Color GetDisplayColor()
    {
        var info = Info.FirstOrDefault(x => x.CountsAsMajor);

        if (info is null || Info.Count <= 0)
            return Color.White;

        if (info.OverrideDisplayColor(out Color color))
            return color;

        info ??= Info.First();
        return info.Major.Get().Color;
    }

    internal void ApplyConstantTrigger(Player player)
    {
        foreach (var item in Info.Where(x => x is MajorJewelInfo))
        {
            var major = item as MajorJewelInfo;
            float bonus = 0f;

            if (MaterialCategory == "Hellstone")
                bonus = 0.33f;

            major.ConstantTrigger(player, bonus);
        }
    }
}