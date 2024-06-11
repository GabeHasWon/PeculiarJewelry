namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class DemoniteEarring : BaseEarring
{
    public override string MaterialCategory => "Demonite";

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient(ItemID.DemoniteBar, 4)
        .AddIngredient(ItemID.Chain)
        .AddTile(TileID.Chairs)
        .AddTile(TileID.Tables)
        .Register()
        .DisableRecipe();
}