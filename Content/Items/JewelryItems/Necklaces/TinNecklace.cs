namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class TinNecklace : BaseNecklace
{
    public override string MaterialCategory => "Tin";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.TinBar, 6)
            .AddTile(TileID.Anvils)
            .Register()
            .DisableRecipe();
    }
}