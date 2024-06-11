namespace PeculiarJewelry.Content.Items.JewelryItems.Tiaras;

[AutoloadEquip(EquipType.Head)]
public class DemoniteTiara : BaseTiara
{
    public override string MaterialCategory => "Demonite";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.DemoniteBar, 8)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}