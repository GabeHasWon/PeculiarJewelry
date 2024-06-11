namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class AdamantiteNecklace : BaseNecklace
{
    public override string MaterialCategory => "Adamantite";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.AdamantiteBar, 6)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}