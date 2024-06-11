namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class CopperNecklace : BaseNecklace
{
    public override string MaterialCategory => "Copper";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.CopperBar, 6)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}