namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class PlatinumNecklace : BaseNecklace
{
    public override string MaterialCategory => "Platinum";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.PlatinumBar, 6)
            .AddTile(TileID.Anvils)
            .Register()
            .DisableRecipe();
    }
}