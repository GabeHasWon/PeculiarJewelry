namespace PeculiarJewelry.Content.Items.JewelryItems.Tiaras;

[AutoloadEquip(EquipType.Head)]
public class ChlorophyteTiara : BaseTiara
{
    public override string MaterialCategory => "Chlorophyte";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.ChlorophyteBar, 8)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}