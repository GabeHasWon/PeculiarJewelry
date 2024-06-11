namespace PeculiarJewelry.Content.Items.JewelryItems.Tiaras;

[AutoloadEquip(EquipType.Head)]
public class SpectreTiara : BaseTiara
{
    public override string MaterialCategory => "Spectre";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.SpectreBar, 8)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}