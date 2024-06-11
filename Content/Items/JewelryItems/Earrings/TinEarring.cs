namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class TinEarring : BaseEarring
{
    public override string MaterialCategory => "Tin";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.TinBar, 4)
            .AddIngredient(ItemID.Chain)
            .AddTile(TileID.Anvils)
            .Register()
            .DisableRecipe();
    }
}