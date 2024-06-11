namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class DemoniteNecklace : BaseNecklace
{
    public override string MaterialCategory => "Demonite";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.DemoniteBar, 6)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}