﻿using PeculiarJewelry.Content.Items.Jewels;
using PeculiarJewelry.Content.Items.JewelSupport;
using PeculiarJewelry.Content.JewelryMechanic;
using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using PeculiarJewelry.Content.NPCs;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.IO;

namespace PeculiarJewelry.Content.Items;

public class BagOfShinies : ModItem
{
    public JewelTier tier;

    public override ModItem Clone(Item newEntity)
    {
        var clone = base.Clone(newEntity);
        (clone as BagOfShinies).tier = tier;
        return clone;
    }

    public override void SetDefaults()
    {
        Item.Size = new(32, 34);
        Item.rare = ModContent.RarityType<JewelRarity>();
        Item.maxStack = 1;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        string text = Language.GetText("Mods.PeculiarJewelry.Items.BagOfShinies.Contains").WithFormatArgs($"{tier.Localize()}").Value;
        tooltips.Insert(1, new TooltipLine(Mod, "BossTier", text));
    }

    public override bool CanRightClick()
    {
        Main.LocalPlayer.GetModPlayer<StupidIdiotItemLootWorkaroundPlayer>().storedTier = tier;
        return true;
    }

    public override void ModifyItemLoot(ItemLoot itemLoot)
    {
        const int SupportDropChance = 30;

        itemLoot.AddCommon<SparklyDust>(1, 5);

        // Not expert rule
        LeadingConditionRule notExpert = new(new Conditions.NotExpert());
        var majorRule = ItemDropRule.Common(ModContent.ItemType<MajorJewel>(), (int)Math.Round(1 / ModContent.GetInstance<JewelryStatConfig>().ChanceForMajor));
        majorRule.OnFailedRoll(ItemDropRule.Common(ModContent.ItemType<MinorJewel>(), 1));
        notExpert.OnSuccess(majorRule);
        notExpert.OnSuccess(SupportItemDrops(SupportDropChance));
        LeadingConditionRule notMaster = new(new Conditions.NotMasterMode());
        notMaster.OnSuccess(notExpert);
        itemLoot.Add(notMaster);

        // Expert rule
        LeadingConditionRule expert = new(new Conditions.IsExpert());
        expert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MajorJewel>(), 2));
        AddMultipleJewels<MinorJewel>(expert, 3);
        notExpert.OnSuccess(SupportItemDrops(SupportDropChance / 2));
        itemLoot.Add(expert);

        // Master rule
        LeadingConditionRule notExpertForMaster = new(new Conditions.NotExpert());
        LeadingConditionRule master = new(new Conditions.IsMasterMode());
        AddMultipleJewels<MajorJewel>(master, 2, 2);
        AddMultipleJewels<MinorJewel>(master, 4);
        master.OnSuccess(SupportItemDrops(SupportDropChance / 2));
        notExpertForMaster.OnSuccess(master);
        itemLoot.Add(notExpertForMaster);
    }

    private static void AddMultipleJewels<T>(LeadingConditionRule expert, int max, int baseChance = 1) where T : Jewel
    {
        expert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<T>(), baseChance));

        for (int i = 0; i < max - 1; i++)
            expert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<T>(), max - 1));
    }

    private static IItemDropRule SupportItemDrops(int chance)
    {
        const int TotalRules = 3;

        chance /= TotalRules;

        var options = ItemDropRule.OneFromOptions(chance, ModContent.ItemType<IrradiatedPearl>(), ModContent.ItemType<CursedDollar>());
        var scales = options.OnFailedRoll(ItemDropRule.ByCondition(new TierCutoffCondition(7), ModContent.ItemType<GoldenCarpScales>(), chance));
        scales.OnFailedRoll(ItemDropRule.ByCondition(new TierCutoffCondition(13), ModContent.ItemType<CelestialEye>(), chance));
        return options;
    }

    public override void OnSpawn(IEntitySource source)
    {
        if (source is EntitySource_Loot loot && loot.Entity is NPC npc && npc.boss)
            tier = BossLootGlobal.GetBossTierAdjustedByDesecration(npc);
    }

    public override void SaveData(TagCompound tag) => tag.Add("tier", (byte)tier);
    public override void LoadData(TagCompound tag) => tier = (JewelTier)tag.GetByte("tier");
    public override void NetSend(BinaryWriter writer) => writer.Write((byte)tier);
    public override void NetReceive(BinaryReader reader) => tier = (JewelTier)reader.ReadByte();
}

class StupidIdiotItemLootWorkaroundPlayer : ModPlayer
{
    internal JewelTier storedTier;
}
