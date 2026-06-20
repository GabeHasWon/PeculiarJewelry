using Microsoft.CodeAnalysis.CSharp.Syntax;
using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;

namespace PeculiarJewelry.Common.Globals;

internal class JewelsInChestSystem : ModSystem
{
    public override void PostWorldGen()
    {
        for (int i = 0; i < Main.maxChests; i++)
        {
            if (Main.rand.NextBool(2))
                continue;

            var chest = Main.chest[i];

            if (chest is not null && Main.tile[chest.x, chest.y].TileType < TileID.Count && Main.tile[chest.x, chest.y].TileFrameX == 36)
                chest.AddItemToShop(GenerateJewelry());
        }
    }

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
