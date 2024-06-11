namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class TitaniumNecklace : BaseNecklace
{
    public override string MaterialCategory => "Titanium";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.TitaniumBar, 6)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}