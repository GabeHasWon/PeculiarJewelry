namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class TungstenNecklace : BaseNecklace
{
    public override string MaterialCategory => "Tungsten";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.TungstenBar, 6)
            .AddTile(TileID.Anvils)
            .Register()
            .DisableRecipe();
    }
}