using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.Items.Jewels;
using PeculiarJewelry.Content.Items.Jewels.Rares.Amber;
using PeculiarJewelry.Content.Items.Jewels.Rares.Ancient;
using PeculiarJewelry.Content.Items.Jewels.Rares.CatEye;
using PeculiarJewelry.Content.Items.Jewels.Rares.Lucky;
using PeculiarJewelry.Content.Items.Jewels.Rares.Pearl;
using PeculiarJewelry.Content.Items.Jewels.Rares.Soulstone;
using PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;
using PeculiarJewelry.Content.JewelryMechanic;
using PeculiarJewelry.Content.JewelryMechanic.Stats;

namespace PeculiarJewelry.Common.Globals;

internal class JewelsInChestSystem : ModSystem
{
    public override void PostWorldGen()
    {
        const int HighChance = 2;
        const int DefaultChance = 4;
        const int LowChance = 6;

        for (int i = 0; i < Main.maxChests; i++)
        {
            var chest = Main.chest[i];

            if (chest is null || Main.tile[chest.x, chest.y].TileType != TileID.Containers)
                continue;

            Tile tile = Main.tile[chest.x, chest.y];

            if (tile.TileFrameX == 36 && WorldGen.genRand.NextBool(HighChance))
                chest.AddItemToShop(GenerateJewelry());

            if (tile.TileFrameX == 36 && WorldGen.genRand.NextBool(HighChance))
                AddItemToChest(chest, ModContent.ItemType<MinorLucky>());
            else if (tile.TileFrameX == 0 && WorldGen.genRand.NextBool(LowChance))
                AddItemToChest(chest, ModContent.ItemType<MinorJewel>());
            else if (tile.TileFrameX == 612 && WorldGen.genRand.NextBool(DefaultChance))
                AddItemToChest(chest, ModContent.ItemType<FullAmber>());
            else if (tile.TileFrameX == 144 && WorldGen.genRand.NextBool(DefaultChance))
                AddItemToChest(chest, ModContent.ItemType<MinorCatEye>());
            //else if (tile.TileFrameX is 360 or 1152 && WorldGen.genRand.NextBool(LowChance))
            //{
            //    Item opal = new(ModContent.ItemType<MinorOpal>());
            //    JewelInfo info = (opal.ModItem as MinorOpal).info;
            //    info.SubStats.Add(new JewelStat((StatType)WorldGen.genRand.Next((int)StatType.BasicMax)));

            //    if (WorldGen.genRand.NextBool())
            //        info.SubStats.Add(new JewelStat((StatType)WorldGen.genRand.Next((int)StatType.BasicMax)));

            //    AddItemToChest(chest, opal);
            //}
            else if (tile.TileFrameX == 612 && WorldGen.genRand.NextBool(DefaultChance))
                AddItemToChest(chest, ModContent.ItemType<MinorPearl>());
            else if (tile.TileFrameX == 396 && WorldGen.genRand.NextBool(DefaultChance))
                AddItemToChest(chest, ModContent.ItemType<MinorSpectrolite>());
            else if (tile.TileFrameX == 72 && WorldGen.genRand.NextBool(DefaultChance))
                AddItemToChest(chest, ModContent.ItemType<MinorSoulstone>());
            else if (tile.TileFrameX is 1800 or 1836 && WorldGen.genRand.NextBool(LowChance))
                AddItemToChest(chest, ModContent.ItemType<AncientMinor>());
        }
    }

    private static void AddItemToChest(Chest chest, Item loot) => chest.AddItemToShop(loot);

    private static void AddItemToChest(Chest chest, int type) => AddItemToChest(chest, new Item(type));

    private static Item GenerateJewelry()
    {
        int jewelryType = JewelryCommon.GetRandomJewelryType(JewelryCommon.PrehardmodeMetals);
        var item = new Item(jewelryType);
        var jewelry = item.ModItem as BasicJewelry;
        jewelry.tier = (BasicJewelry.JewelryTier)WorldGen.genRand.Next(5);
        bool hasMajor = false;

        while (jewelry.Info.Count < (int)jewelry.tier + 1)
        {
            if (Main.rand.NextFloat() < 0.25f)
                break;

            JewelInfo info = new MinorJewelInfo();

            if (!hasMajor && Main.rand.NextBool(6))
            {
                info = new MajorJewelInfo();
                hasMajor = true;
            }

            info.Setup((JewelTier)Main.rand.Next(3));
            jewelry.Info.Add(info);
        }

        return item;
    }
}
