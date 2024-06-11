namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class LeadEarring : BaseEarring
{
    public override string MaterialCategory => "Lead";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.LeadBar, 4)
            .AddIngredient(ItemID.Chain)
            .AddTile(TileID.Chairs)
            .AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}