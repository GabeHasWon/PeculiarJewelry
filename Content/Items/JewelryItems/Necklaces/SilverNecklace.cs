namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class SilverNecklace : BaseNecklace
{
    public override string MaterialCategory => "Silver";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.SilverBar, 6)
            .AddTile(TileID.Anvils)
            .Register()
            .DisableRecipe();
    }
}