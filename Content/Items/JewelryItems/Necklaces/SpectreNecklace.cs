namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class SpectreNecklace : BaseNecklace
{
    public override string MaterialCategory => "Spectre";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.SpectreBar, 6)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}