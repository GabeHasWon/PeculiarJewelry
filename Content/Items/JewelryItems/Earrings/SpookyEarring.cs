namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class SpookyEarring : BaseEarring
{
    public override string MaterialCategory => "Spooky";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.SpookyWood, 4)
            .AddIngredient(ItemID.Chain)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}