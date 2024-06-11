namespace PeculiarJewelry.Content.Items.JewelryItems.Tiaras;

[AutoloadEquip(EquipType.Head)]
public class HallowedTiara : BaseTiara
{
    public override string MaterialCategory => "Hallowed";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.HallowedBar, 8)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}