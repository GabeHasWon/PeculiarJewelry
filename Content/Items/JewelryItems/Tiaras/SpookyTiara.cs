namespace PeculiarJewelry.Content.Items.JewelryItems.Tiaras;

[AutoloadEquip(EquipType.Head)]
public class SpookyTiara : BaseTiara
{
    public override string MaterialCategory => "Spooky";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.SpookyWood, 8)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}