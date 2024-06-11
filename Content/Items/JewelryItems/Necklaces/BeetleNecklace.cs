namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class BeetleNecklace : BaseNecklace
{
    public override string MaterialCategory => "Beetle";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.BeetleHusk, 6)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}