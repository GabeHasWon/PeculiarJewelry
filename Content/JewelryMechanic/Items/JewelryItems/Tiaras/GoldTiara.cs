namespace PeculiarJewelry.Content.JewelryMechanic.Items.JewelryItems.Tiaras;

[AutoloadEquip(EquipType.Head)]
public class GoldTiara : BaseTiara
{
    public override string MaterialCategory => "Gold";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.GoldBar, 8)
            .AddTile(TileID.Anvils)
            .Register();
    }
}