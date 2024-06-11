using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses;
using ReLogic.Content;
using System.Linq;
using Terraria.GameContent;

namespace PeculiarJewelry.Content.Items.JewelryItems.Brooches;

[AutoloadEquip(EquipType.Front)]
[Autoload(false)]
public class BaseBrooch : BasicJewelry
{
    static Asset<Texture2D> _jewels;
    static Asset<Texture2D> _jewelsEquip;

    protected override bool CloneNewInstances => true;

    public override string Name => _name;
    public override string MaterialCategory => _category;

    protected int _material;
    protected string _category = string.Empty;
    protected string _name = string.Empty;

    private int _frame;

    public BaseBrooch(string name, string category, int mat)
    {
        _name = name;
        _category = category;
        _material = mat;
    }

    public override ModItem Clone(Item newEntity)
    {
        var clone = base.Clone(newEntity) as BaseBrooch;
        clone._material = _material;
        clone._category = _category;
        clone._frame = _frame;
        clone._name = _name;
        return clone;
    }

    public override void SetStaticDefaults()
    {
        _jewels ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Brooches/BroochJewel");
        _jewelsEquip ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Brooches/BroochJewel_Front");
    }

    public override void Unload() => _jewels = null;

    protected override void Defaults()
    {
        Item.width = 34;
        Item.height = 30;
        Item.accessory = true;

        _frame = Main.rand.Next(3);
    }

    protected override void EquipEffect(Player player, bool isVanity = false)
    {
        if (Info.Count != 0)
            player.GetModPlayer<MaterialPlayer>().SetEquip(EquipType.Front, new MaterialPlayer.EquipLayerInfo(GetDisplayColor(), _jewelsEquip.Value));
    }

    public override bool PreDrawInInventory(SpriteBatch s, Vector2 p, Rectangle frame, Color d, Color itemColor, Vector2 o, float sc) => VariantDraw(s, p, d, o, sc);
    public override bool PreDrawInWorld(SpriteBatch s, Color d, Color a, ref float r, ref float sc, int w) =>
        VariantDraw(s, Item.Center - Main.screenPosition, d, null, sc / 3f, r);

    private bool VariantDraw(SpriteBatch spriteBatch, Vector2 position, Color drawColor, Vector2? origin, float scale, float rotation = 0f)
    {
        Rectangle rect = new(0, 32 * _frame, 34, 30);
        Texture2D tex = TextureAssets.Item[Type].Value;

        origin ??= new Vector2(17, 45);

        spriteBatch.Draw(tex, position, rect, drawColor, rotation, origin.Value / new Vector2(1, 3), scale * 3, SpriteEffects.None, 0);
        return false;
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color i, Vector2 origin, float scale)
    {
        if (Info.Count != 0)
            spriteBatch.Draw(_jewels.Value, position, frame, GetDisplayColor(), 0f, origin / new Vector2(1, 3), scale * 3, SpriteEffects.None, 0);

        base.PostDrawInInventory(spriteBatch, position, frame, drawColor, i, origin, scale * 2);
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

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient(_material, 6)
        .AddTile(TileID.Chairs)
        .AddTile(TileID.Tables)
        .Register()
        .DisableRecipe();

    public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
    {
        if (equippedItem.ModItem is BaseBrooch)
            return incomingItem.ModItem is not BaseBrooch;

        return true;
    }
}

internal class BroochLoader : ILoadable
{
    public void Load(Mod mod)
    {
        // Prehardmode
        AddBrooch("Copper", ItemID.CopperBar);
        AddBrooch("Tin", ItemID.TinBar);
        AddBrooch("Iron", ItemID.IronBar);
        AddBrooch("Lead", ItemID.LeadBar);
        AddBrooch("Silver", ItemID.SilverBar);
        AddBrooch("Tungsten", ItemID.TungstenBar);
        AddBrooch("Gold", ItemID.GoldBar);
        AddBrooch("Platinum", ItemID.PlatinumBar);
        AddBrooch("Demonite", ItemID.DemoniteBar);
        AddBrooch("Crimtane", ItemID.CrimtaneBar);
        AddBrooch("Meteorite", ItemID.MeteoriteBar);
        AddBrooch("Hellstone", ItemID.HellstoneBar);

        // Hardmode
        AddBrooch("Cobalt", ItemID.CobaltBar);
        AddBrooch("Palladium", ItemID.PalladiumBar);
        AddBrooch("Mythril", ItemID.MythrilBar);
        AddBrooch("Orichalcum", ItemID.OrichalcumBar);
        AddBrooch("Adamantite", ItemID.AdamantiteBar);
        AddBrooch("Titanium", ItemID.TitaniumBar);
        AddBrooch("Hallowed", ItemID.HallowedBar);
        AddBrooch("Chlorophyte", ItemID.ChlorophyteBar);
        AddBrooch("Beetle", ItemID.BeetleHusk);
        AddBrooch("Shroomite", ItemID.ShroomiteBar);
        AddBrooch("Spectre", ItemID.SpectreBar);
        AddBrooch("Spooky", ItemID.SpookyWood);
        AddBrooch("Luminite", ItemID.LunarBar);
    }

    private static bool AddBrooch(string category, int material)
        => ModContent.GetInstance<PeculiarJewelry>().AddContent(new BaseBrooch(category + "Brooch", category, material));

    public void Unload()
    {
    }
}