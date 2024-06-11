namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class MythrilNecklace : BaseNecklace
{
    public override string MaterialCategory => "Mythril";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.MythrilBar, 6)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}