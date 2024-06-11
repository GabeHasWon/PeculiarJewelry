namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class LeadNecklace : BaseNecklace
{
    public override string MaterialCategory => "Lead";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.LeadBar, 6)
            .AddTile(TileID.Anvils)
            .Register()
            .DisableRecipe();
    }
}