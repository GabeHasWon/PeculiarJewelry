namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class MythrilEarring : BaseEarring
{
    public override string MaterialCategory => "Mythril";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.MythrilBar, 4)
            .AddIngredient(ItemID.Chain)
            .AddTile(TileID.Chairs)
            .AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}