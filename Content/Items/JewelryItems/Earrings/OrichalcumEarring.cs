namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class OrichalcumEarring : BaseEarring
{
    public override string MaterialCategory => "Orichalcum";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.OrichalcumBar, 4)
            .AddIngredient(ItemID.Chain)
            .AddTile(TileID.Chairs)
            .AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}