namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class CobaltNecklace : BaseNecklace
{
    public override string MaterialCategory => "Cobalt";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.CobaltBar, 6)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}