using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses;
using ReLogic.Content;
using System.Linq;

namespace PeculiarJewelry.Content.Items.JewelryItems.Necklaces;

[AutoloadEquip(EquipType.Head)]
public abstract class BaseNecklace : BasicJewelry
{
    static Asset<Texture2D> _jewels;
    static Asset<Texture2D> _jewelsEquip;

    public override void SetStaticDefaults()
    {
        _jewels ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Necklaces/NecklaceJewels");
        _jewelsEquip ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Necklaces/NecklaceJewels_Neck");
    }

    public override void Unload() => _jewels = null;

    protected override void Defaults()
    {
        Item.width = 26;
        Item.height = 42;
        Item.accessory = true;
    }

    protected override void EquipEffect(Player player, bool isVanity = false)
    {
        if (Info.Count != 0)
            player.GetModPlayer<MaterialPlayer>().SetEquip(EquipType.Neck, new MaterialPlayer.EquipLayerInfo(GetDisplayColor(), _jewelsEquip.Value));
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        if (Info.Count != 0)
            spriteBatch.Draw(_jewels.Value, position, frame, GetDisplayColor(), 0f, origin, scale, SpriteEffects.None, 0);

        base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        if (Info.Count != 0)
        {
            Color col = lightColor.MultiplyRGB(GetDisplayColor());
            spriteBatch.Draw(_jewels.Value, Item.Center - Main.screenPosition, null, col, rotation, _jewels.Size() / 2f, scale, SpriteEffects.None, 0);
        }

        base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);
    }

    public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
    {
        if (equippedItem.ModItem is BaseNecklace)
            return incomingItem.ModItem is not BaseNecklace;

        return true;
    }
}