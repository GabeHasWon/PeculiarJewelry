namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class IronEarring : BaseEarring
{
    public override string MaterialCategory => "Iron";

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient(ItemID.IronBar, 4)
        .AddIngredient(ItemID.Chain)
        .AddTile(TileID.Chairs)
        .AddTile(TileID.Tables)
        .Register()
        .DisableRecipe();
}