namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class ChlorophyteNecklace : BaseNecklace
{
    public override string MaterialCategory => "Chlorophyte";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.ChlorophyteBar, 6)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}