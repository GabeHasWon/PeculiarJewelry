using PeculiarJewelry.Content.JewelryMechanic.GrindstoneSystem;
using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.UI;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace PeculiarJewelry.Content.Items.Tiles;

public class Grindstone : ModItem
{
    public override void SetStaticDefaults() => ItemID.Sets.DisableAutomaticPlaceableDrop[Type] = true;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<GrindstoneTile>());
        Item.Size = new(38, 36);
        Item.rare = ModContent.RarityType<JewelRarity>();
        Item.maxStack = Item.CommonMaxStack;
        Item.value = Item.buyPrice(gold: 5);
    }
}

public class GrindstoneTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
        TileObjectData.newTile.Origin = new Point16(2, 2);
        TileObjectData.addTile(Type);

        DustType = DustID.Stone;

        AddMapEntry(new Color(80, 91, 91));
        RegisterItemDrop(ModContent.ItemType<Grindstone>());
    }

    public override void MouseOver(int i, int j) => GrindstoneHandler.Input(i, j);
    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 4;

    public override bool RightClick(int i, int j)
    {
        JewelUISystem.SwitchUI(new JewelryCraftingUI());
        return true;
    }
}
