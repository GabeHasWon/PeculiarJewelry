using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses;
using ReLogic.Content;
using System.Linq;

namespace PeculiarJewelry.Content.Items.JewelryItems.Tiaras;

[AutoloadEquip(EquipType.Head)]
public abstract class BaseTiara : BasicJewelry
{
    static Asset<Texture2D> _jewels;
    static Asset<Texture2D> _jewelsEquip;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        _jewels ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Tiaras/TiaraJewels");
        _jewelsEquip ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Tiaras/TiaraJewels_Head");
    }

    public override void Unload() => _jewels = null;

    protected override void Defaults()
    {
        Item.width = 38;
        Item.height = 22;
        Item.accessory = false;
        Item.defense = (int)((float)tier * 0.33f);
    }

    protected override void EquipEffect(Player player, bool isVanity = false)
    {
        if (Info.Count != 0 && (isVanity || player.armor[10].IsAir))
            player.GetModPlayer<MaterialPlayer>().SetEquip(EquipType.Head, new MaterialPlayer.EquipLayerInfo(GetDisplayColor(), _jewelsEquip.Value));
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
}