namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class IronNecklace : BaseNecklace
{
    public override string MaterialCategory => "Iron";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.IronBar, 6)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}