namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class HallowedEarring : BaseEarring
{
    public override string MaterialCategory => "Hallowed";

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient(ItemID.HallowedBar, 4)
        .AddIngredient(ItemID.Chain)
        .AddTile(TileID.Chairs)
        .AddTile(TileID.Tables)
        .Register()
        .DisableRecipe();
}