namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class MeteoriteNecklace : BaseNecklace
{
    public override string MaterialCategory => "Meteorite";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.MeteoriteBar, 6)
            .AddTile(TileID.Anvils)
            .Register()
            .DisableRecipe();
    }
}