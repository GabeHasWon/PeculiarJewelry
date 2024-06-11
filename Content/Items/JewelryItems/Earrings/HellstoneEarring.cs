namespace PeculiarJewelry.Content.Items.JewelryItems.Earrings;

[AutoloadEquip(EquipType.Face)]
public class HellstoneEarring : BaseEarring
{
    public override string MaterialCategory => "Hellstone";

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient(ItemID.HellstoneBar, 4)
        .AddIngredient(ItemID.Chain)
        .AddTile(TileID.Chairs)
        .AddTile(TileID.Tables)
        .Register()
        .DisableRecipe();
}