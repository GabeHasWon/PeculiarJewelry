namespace PeculiarJewelry.Content.Items.JewelryItems.Tiaras;

[AutoloadEquip(EquipType.Head)]
public class LeadTiara : BaseTiara
{
    public override string MaterialCategory => "Lead";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.LeadBar, 8)
            .AddTile(TileID.Anvils)
            .Register()
            .DisableRecipe();
    }
}