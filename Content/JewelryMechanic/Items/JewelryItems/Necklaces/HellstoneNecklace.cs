namespace PeculiarJewelry.Content.JewelryMechanic.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Neck)]
public class HellstoneNecklace : BaseNecklace
{
    public override string MaterialCategory => "Hellstone";

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.HellstoneBar, 6)
            .AddTile(TileID.Anvils)
            .Register();
    }
}