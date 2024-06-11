namespace PeculiarJewelry.Content.Items.JewelryItems.Tiaras;

[AutoloadEquip(EquipType.Head)]
public class AdamantiteTiara : BaseTiara
{
    public override string MaterialCategory => "Adamantite";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.AdamantiteBar, 8)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}