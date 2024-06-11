namespace PeculiarJewelry.Content.Items.JewelryItems.Tiaras;

[AutoloadEquip(EquipType.Head)]
public class CopperTiara : BaseTiara
{
    public override string MaterialCategory => "Copper";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.CopperBar, 8)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}