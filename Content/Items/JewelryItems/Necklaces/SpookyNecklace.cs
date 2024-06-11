namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class SpookyNecklace : BaseNecklace
{
    public override string MaterialCategory => "Spooky";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.SpookyWood, 6)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}