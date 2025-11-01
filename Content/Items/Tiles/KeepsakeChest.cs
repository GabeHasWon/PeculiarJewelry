using PeculiarJewelry.Content.Items.Jewels;
using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.UI;
using PeculiarJewelry.Content.JewelryMechanic.UI.JewelStorage;
using Terraria;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace PeculiarJewelry.Content.Items.Tiles;

public class KeepsakeChest : ModItem
{
    public override void SetStaticDefaults() => ItemID.Sets.DisableAutomaticPlaceableDrop[Type] = true;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<KeepsakeChestTile>());
        Item.Size = new(36, 38);
        Item.rare = ModContent.RarityType<JewelRarity>();
        Item.maxStack = Item.CommonMaxStack;
        Item.value = Item.buyPrice(silver: 5);
    }

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient(ItemID.Wood, 20)
        .AddIngredient<MajorJewel>()
        .AddIngredient(ItemID.Glass, 5)
        .AddTile(TileID.Sawmill)
        .Register();
}

public class KeepsakeChestTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
        TileObjectData.newTile.Origin = new Point16(1);
        TileObjectData.addTile(Type);

        DustType = DustID.Stone;

        AddMapEntry(new Color(203, 179, 73));
        RegisterItemDrop(ModContent.ItemType<KeepsakeChest>());
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 4;

    public override bool RightClick(int i, int j)
    {
        JewelUISystem.SwitchUI(new JewelryStorageUI());
        Main.playerInventory = true;
        return true;
    }

    public override void MouseOver(int i, int j)
    {
        Player player = Main.LocalPlayer;
        player.cursorItemIconID = ModContent.ItemType<MajorJewel>();
        player.cursorItemIconText = "";
        player.noThrow = 2;
        player.cursorItemIconEnabled = true;
    }

    public override void MouseOverFar(int i, int j)
    {
        Player player = Main.LocalPlayer;
        player.cursorItemIconEnabled = false;
        player.cursorItemIconID = ItemID.None;
    }
}
