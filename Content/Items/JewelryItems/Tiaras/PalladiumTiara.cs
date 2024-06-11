namespace PeculiarJewelry.Content.Items.JewelryItems.Tiaras;

[AutoloadEquip(EquipType.Head)]
public class PalladiumTiara : BaseTiara
{
    public override string MaterialCategory => "Palladium";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.PalladiumBar, 8)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}