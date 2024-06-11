namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class TungstenEarring : BaseEarring
{
    public override string MaterialCategory => "Tungsten";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.TungstenBar, 4)
            .AddIngredient(ItemID.Chain)
            .AddTile(TileID.Anvils)
            .Register()
            .DisableRecipe();
    }
}