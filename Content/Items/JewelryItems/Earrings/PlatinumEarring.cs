namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class PlatinumEarring : BaseEarring
{
    public override string MaterialCategory => "Platinum";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.PlatinumBar, 4)
            .AddIngredient(ItemID.Chain)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}