namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class PalladiumEarring : BaseEarring
{
    public override string MaterialCategory => "Palladium";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.PalladiumBar, 4)
            .AddIngredient(ItemID.Chain)
            .AddTile(TileID.Chairs)
            .AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}