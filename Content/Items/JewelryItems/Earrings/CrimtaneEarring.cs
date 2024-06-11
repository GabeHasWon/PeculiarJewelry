namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class CrimtaneEarring : BaseEarring
{
    public override string MaterialCategory => "Crimtane";

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient(ItemID.CrimtaneBar, 4)
        .AddIngredient(ItemID.Chain)
        .AddTile(TileID.Chairs)
        .AddTile(TileID.Tables)
        .Register()
        .DisableRecipe();
}