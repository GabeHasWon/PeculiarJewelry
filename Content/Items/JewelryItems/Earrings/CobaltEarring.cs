namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class CobaltEarring : BaseEarring
{
    public override string MaterialCategory => "Cobalt";

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient(ItemID.CobaltBar, 4)
        .AddIngredient(ItemID.Chain)
        .AddTile(TileID.Chairs)
        .AddTile(TileID.Tables)
        .Register()
        .DisableRecipe();
}