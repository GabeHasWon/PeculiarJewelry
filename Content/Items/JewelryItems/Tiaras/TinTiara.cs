namespace PeculiarJewelry.Content.Items.JewelryItems.Tiaras;

[AutoloadEquip(EquipType.Head)]
public class TinTiara : BaseTiara
{
    public override string MaterialCategory => "Tin";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.TinBar, 8)
            .AddTile(TileID.Anvils)
            .Register()
            .DisableRecipe();
    }
}