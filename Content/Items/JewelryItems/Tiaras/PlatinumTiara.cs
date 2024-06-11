namespace PeculiarJewelry.Content.Items.JewelryItems.Tiaras;

[AutoloadEquip(EquipType.Head)]
public class PlatinumTiara : BaseTiara
{
    public override string MaterialCategory => "Platinum";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.PlatinumBar, 8)
            .AddTile(TileID.Anvils)
            .Register()
            .DisableRecipe();
    }
}