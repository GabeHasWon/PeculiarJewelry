namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class MeteoriteEarring : BaseEarring
{
    public override string MaterialCategory => "Meteorite";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.MeteoriteBar, 4)
            .AddIngredient(ItemID.Chain)
            .AddTile(TileID.Chairs)
            .AddTile(TileID.Tables)
            .Register()
            .DisableRecipe();
    }
}