namespace PeculiarJewelry.Content.Items.JewelryItems.Tiaras;

[AutoloadEquip(EquipType.Head)]
public class IronTiara : BaseTiara
{
    public override string MaterialCategory => "Iron";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.IronBar, 8)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}