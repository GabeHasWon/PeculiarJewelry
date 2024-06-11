namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class CopperEarring : BaseEarring
{
    public override string MaterialCategory => "Copper";

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient(ItemID.CopperBar, 4)
        .AddIngredient(ItemID.Chain)
        .AddTile(TileID.Chairs)
        .AddTile(TileID.Tables)
        .Register()
        .DisableRecipe();
}