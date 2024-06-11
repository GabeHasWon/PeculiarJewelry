namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class OrichalcumNecklace : BaseNecklace
{
    public override string MaterialCategory => "Orichalcum";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.OrichalcumBar, 6)
            .AddTile(TileID.Chairs).AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}