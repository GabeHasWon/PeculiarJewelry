namespace PeculiarJewelry.Content.Items.JewelryItems.Tiaras;

[AutoloadEquip(EquipType.Head)]
public class TungstenTiara : BaseTiara
{
    public override string MaterialCategory => "Tungsten";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.TungstenBar, 8)
            .AddTile(TileID.Anvils)
            .Register()
            .DisableRecipe();
    }
}