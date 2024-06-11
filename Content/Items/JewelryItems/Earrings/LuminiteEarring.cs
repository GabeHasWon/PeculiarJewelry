namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class LuminiteEarring : BaseEarring
{
    public override string MaterialCategory => "Luminite";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.LunarBar, 4)
            .AddIngredient(ItemID.Chain)
            .AddTile(TileID.Chairs)
            .AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}