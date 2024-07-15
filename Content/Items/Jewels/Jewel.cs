using Microsoft.Build.Tasks.Hosting;
using PeculiarJewelry.Content.Items.Jewels.Rares.Impure;
using PeculiarJewelry.Content.Items.JewelSupport;
using PeculiarJewelry.Content.Items.Tiles;
using PeculiarJewelry.Content.JewelryMechanic;
using PeculiarJewelry.Content.JewelryMechanic.GrindstoneSystem;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using PeculiarJewelry.Content.JewelryMechanic.Stats.IO;
using PeculiarJewelry.Content.JewelryMechanic.UI;
using PeculiarJewelry.Content.NPCs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace PeculiarJewelry.Content.Items.Jewels;

public abstract class Jewel : ModItem, IGrindableItem, IStorableItem
{
    internal static bool InstancedTypeLookup = false;

    internal static Dictionary<string, Jewel> TypeLookup = [];

    public abstract LocalizedText ExaminationLocalization { get; }
    protected abstract Type InfoType { get; }

    protected virtual byte MaxVariations => 1;

    public JewelInfo info;
    public byte variant;


    public override sealed void SetStaticDefaults()
    {
        if (!InstancedTypeLookup)
        {
            TypeLookup.Clear();
            var types = AssemblyManager.GetLoadableTypes(Mod.Code).Where(x => typeof(Jewel).IsAssignableFrom(x) && !x.IsAbstract);

            foreach (var type in types)
            {
                var jewel = Activator.CreateInstance(type) as Jewel;
                TypeLookup.Add(jewel.InfoType.Name, jewel);
            }

            InstancedTypeLookup = true;
        }
    }

    public sealed override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = Item.useAnimation = 8;
        Item.noUseGraphic = true;
        Item.autoReuse = true;
        Item.value = Item.buyPrice(0, 10);
        Item.maxStack = 1;

        info = Activator.CreateInstance(InfoType) as JewelInfo;
        info.Setup(JewelTier.Natural); //Info is tier 0 by default 
        variant = (byte)Main.rand.Next(MaxVariations);

        Defaults();
    }

    public abstract void Defaults();

    public override void OnSpawn(IEntitySource source)
    {
        if (this is MajorJewel or MinorJewel && JewelRarePool.CheckForBiomes(Main.LocalPlayer, out var flags, out int count))
        {
            bool isRare = Main.rand.NextBool(10);
            int rareType = JewelRarePool.GetRareJewelType(flags);

            if (isRare && rareType != -1)
                Item.SetDefaults(rareType);
            else if (Main.rand.NextFloat() < count * 0.05f)
                Item.SetDefaults(JewelryCommon.MajorMinorType<ImpureMajor, ImpureMinor>());
        }

        if (source is EntitySource_Loot loot && loot.Entity is NPC npc && npc.boss)
            info.Setup(BossLootGlobal.GetBossTier(npc));
        else if (source is EntitySource_ItemOpen open && (open.ItemType == ModContent.ItemType<BagOfShinies>() || open.ItemType == ModContent.ItemType<AncientCoffer>()))
            info.Setup(open.Player.GetModPlayer<StupidIdiotItemLootWorkaroundPlayer>().storedTier);
    }

    public sealed override void ModifyTooltips(List<TooltipLine> tooltips) => PlainJewelTooltips(tooltips, info, this);

    public static string Localize(string text) => Language.GetTextValue("Mods.PeculiarJewelry." + text);

    /// <summary>
    /// Adds all relevant tooltips to the given list with the given info.
    /// </summary>
    /// <param name="tooltips">Tooltips to modify.</param>
    /// <param name="info">The info to reference.</param>
    /// <param name="modItem">The mod item this is being attached to.</param>
    /// <param name="displayAsJewel">Whether this is being used directly on a Jewel item or as part of a jewelry accessory. 
    /// This ignores the name modification and hides the exclusivity and cuts left.</param>
    public static void PlainJewelTooltips(List<TooltipLine> tooltips, JewelInfo info, ModItem modItem, bool displayAsJewel = true)
    {
        if (displayAsJewel)
        {
            var name = tooltips.First(x => x.Name == "ItemName");
            name.Text = info.Name;
            name.OverrideColor = info.Major.Get().Color;

            tooltips.Add(new TooltipLine(modItem.Mod, "JewelTier", Language.GetText("Mods.PeculiarJewelry.Jewelry.TierTooltip").WithFormatArgs((int)info.tier).Value));
        }
        else
        {
            string major = info is MajorJewelInfo ? nameof(MajorJewel) : nameof(MinorJewel);
            tooltips.Add(new TooltipLine(modItem.Mod, "JewelName", info.Name) { OverrideColor = info.Major.Get().Color });
        }

        if (info is MajorJewelInfo majorJewelInfo)
            tooltips.Add(new TooltipLine(modItem.Mod, "TriggerEffect", majorJewelInfo.TriggerTooltip(Main.LocalPlayer)));

        if (displayAsJewel || PeculiarJewelry.ShiftDown)
        {
            tooltips.Add(new TooltipLine(modItem.Mod, "MajorStat", info.Major.GetDescription(Main.LocalPlayer, false)) { OverrideColor = info.Major.Get().Color });

            var subStatTooltips = info.SubStatTooltips(Main.LocalPlayer);

            for (int i = 0; i < subStatTooltips.Length; ++i)
            {
                if (!displayAsJewel && subStatTooltips[i] == "-")
                    continue;

                Color color = i < info.SubStats.Count ? info.SubStats[i].Get().Color : Color.White;
                string text = i < info.SubStats.Count ? "   " + subStatTooltips[i] : "   " + subStatTooltips[i];
                tooltips.Add(new TooltipLine(modItem.Mod, "SubStat" + i, text) { OverrideColor = color });
            }

            info.PostAddStatTooltips(tooltips, info, modItem);
        }

        if (displayAsJewel)
        {
            if (info.exclusivity != StatExclusivity.None && info.HasExclusivity)
                tooltips.Add(new TooltipLine(modItem.Mod, "StatExclusivity", info.exclusivity.Localize()));

            if (info.MaxCuts > 0)
                tooltips.Add(new TooltipLine(modItem.Mod, "JewelCuts", info.MaxCuts - info.cuts + "/" + info.MaxCuts + Localize("Jewelry.CutsRemaining")));
        }
    }

    public sealed override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        JewelDrawing.DrawJewel(this, TextureAssets.Item[Type], position, Item.Size / 2f, info.Major.Get().Color, 0f,
            32f / Item.width, Item.width, Item.height + 2, info, variant, true);
        return false;
    }

    public sealed override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
    {
        Color col = lightColor.MultiplyRGB(info.Major.Get().Color);
        JewelDrawing.DrawJewel(this, TextureAssets.Item[Type], Item.Center - Main.screenPosition, Item.Size / 2f, col,
            rotation, scale, Item.width, Item.height + 2, info, variant, false);
        return false;
    }

    public virtual bool PreDrawJewel(Texture2D texture, Vector2 position, Rectangle frame, Color color, float rotation, Vector2 origin, float scale, bool inInventory) => true;
    public virtual void PostDrawJewel(Vector2 position, Rectangle frame, Color color, float scale, float rotation, Vector2 origin, bool inInventory) { }

    public override void SaveData(TagCompound tag)
    {
        tag.Add(nameof(info), info.SaveAs());
        tag.Add(nameof(variant), variant);
    }

    public override void LoadData(TagCompound tag)
    {
        TagCompound infoCompound = tag.GetCompound("info");
        info = JewelIO.LoadInfo(infoCompound);
        variant = tag.GetByte(nameof(variant));
    }

    public override void NetSend(BinaryWriter writer)
    {
        JewelIO.SendJewelInfo(info, writer);
        writer.Write(variant);
    }

    public override void NetReceive(BinaryReader reader)
    {
        info = JewelIO.ReadJewelInfo(reader);
        variant = reader.ReadByte();
    }

    public bool GrindstoneUse(int i, int j, IEntitySource source)
    {
        int dustPayout = (int)(info.TotalDustCost() * 0.2f) + 3 + 3 * (int)(info.tier + 1);

        if (dustPayout < 1)
            dustPayout = 1;

        NewItem.SpawnSynced(source, Main.MouseWorld, ModContent.ItemType<SparklyDust>(), dustPayout, noGrabDelay: true);
        ExtractSubStats(1f, source);

        if (info.cuts > 10)
            ExtractSupportItems(source);

        if (info.cuts - 7 >= 0)
        {
            int echoTier = (int)((info.cuts - 7) / 8f) * 8 + 7;
            int echoType = CutJewelUIState.JewelCutEchoType(echoTier);
            NewItem.SpawnSynced(source, Main.MouseWorld, echoType, 1, noGrabDelay: true);
        }

        if (--Item.stack < 0)
            Item.TurnToAir();

        return true;
    }

    private void ExtractSupportItems(IEntitySource source)
    {
        float chance = (info.cuts - 9f) / 100f;

        if (Main.rand.NextFloat() < chance)
        {
            var pool = new WeightedRandom<int>();
            pool.Add(ModContent.ItemType<CursedDollar>(), 1);
            pool.Add(ModContent.ItemType<IrradiatedPearl>(), 2);
            pool.Add(ModContent.ItemType<GoldenCarpScales>(), 2);
            pool.Add(ModContent.ItemType<CelestialEye>(), 2);
            pool.Add(ModContent.ItemType<BrokenStopwatch>(), 4);
            pool.Add(ModContent.ItemType<StellarJade>(), 1);
            NewItem.SpawnSynced(source, Main.MouseWorld, pool, 1, true);
        }
    }

    private void ExtractSubStats(float ratio, IEntitySource source)
    {
        foreach (var stat in info.SubStats)
        {
            if (Main.rand.NextFloat() * ratio > 0.025f * stat.Strength)
                continue;

            Item item = new(ModContent.ItemType<SubShard>());
            var shard = item.ModItem as SubShard;
            shard.stat = stat;
            NewItem.SpawnSynced(source, Main.MouseWorld, item, true);
        }
    }
}
