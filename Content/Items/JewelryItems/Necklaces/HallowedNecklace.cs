namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class HallowedNecklace : BaseNecklace
{
    public override string MaterialCategory => "Hallowed";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.HallowedBar, 6)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}