using PeculiarJewelry.Content.JewelryMechanic;
using System.Linq;
using Terraria.DataStructures;

namespace PeculiarJewelry.Common.Globals;

internal class JewelGlobalItem : GlobalItem
{
    private static int[] GemOres = [];

    public override void Load() => GemOres = [TileID.Ruby, TileID.Amethyst, TileID.Emerald, TileID.Ruby, TileID.Diamond, TileID.Topaz, TileID.Sapphire];

    public override void OnSpawn(Item item, IEntitySource source)
    {
        if (source is EntitySource_TileBreak tile && WorldGen.InWorld(tile.TileCoords.X, tile.TileCoords.Y) && GemOres.Contains(Main.tile[tile.TileCoords].TileType))
            if (Main.rand.NextBool(60))
            {
                item.SetDefaults(JewelryCommon.MajorMinorType());
                return;
            }
    }
}
