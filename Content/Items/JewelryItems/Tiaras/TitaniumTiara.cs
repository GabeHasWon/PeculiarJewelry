namespace PeculiarJewelry.Content.Items.JewelryItems.Tiaras;

[AutoloadEquip(EquipType.Head)]
public class TitaniumTiara : BaseTiara
{
    public override string MaterialCategory => "Titanium";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.TitaniumBar, 8)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}