namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class AdamantiteEarring : BaseEarring
{
    public override string MaterialCategory => "Adamantite";

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient(ItemID.AdamantiteBar, 4)
        .AddIngredient(ItemID.Chain)
        .AddTile(TileID.Chairs)
        .AddTile(TileID.Tables)
        .Register()
        .DisableRecipe();
}