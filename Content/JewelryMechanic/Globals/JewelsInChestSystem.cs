﻿using PeculiarJewelry.Content.JewelryMechanic.Items.Jewels;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PeculiarJewelry.Content.JewelryMechanic.Globals;

internal class JewelsInChestSystem : ModSystem
{
    public override void PostWorldGen()
    {
        for (int i = 0; i < Main.maxChests; i++)
        {
            var chest = Main.chest[i];

            if (chest is not null && Main.tile[chest.x, chest.y].TileType < TileID.Count)
                chest.AddItemToShop(new Item(ModContent.ItemType<MinorJewel>()));
        }
    }
}
