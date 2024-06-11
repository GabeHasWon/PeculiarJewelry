namespace PeculiarJewelry.Content.Items.JewelryItems.Tiaras;

[AutoloadEquip(EquipType.Head)]
public class MeteoriteTiara : BaseTiara
{
    public override string MaterialCategory => "Meteorite";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.MeteoriteBar, 8)
            .AddTile(TileID.Anvils)
            .Register()
            .DisableRecipe();
    }
}