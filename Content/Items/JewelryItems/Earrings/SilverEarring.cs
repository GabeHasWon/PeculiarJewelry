namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class SilverEarring : BaseEarring
{
    public override string MaterialCategory => "Silver";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.SilverBar, 4)
            .AddIngredient(ItemID.Chain)
            .AddTile(TileID.Anvils)
            .Register()
            .DisableRecipe();
    }
}