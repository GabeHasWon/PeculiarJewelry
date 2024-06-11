namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class ShroomiteEarring : BaseEarring
{
    public override string MaterialCategory => "Shroomite";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.ShroomiteBar, 4)
            .AddIngredient(ItemID.Chain)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}