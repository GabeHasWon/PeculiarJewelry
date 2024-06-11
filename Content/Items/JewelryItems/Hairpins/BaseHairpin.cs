using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses;
using ReLogic.Content;
using System.Linq;

namespace PeculiarJewelry.Content.Items.JewelryItems.Hairpins;

[AutoloadEquip(EquipType.Face)]
[Autoload(false)]
public class BaseHairpin : BasicJewelry
{
    static Asset<Texture2D> _jewels;
    static Asset<Texture2D> _jewelsEquip;

    protected override bool CloneNewInstances => true;

    public override string Name => _name;
    public override string MaterialCategory => _category;

    protected int _material;
    protected string _category = string.Empty;
    protected string _name = string.Empty;
    protected bool _isHardmode = false;

    public BaseHairpin(string name, string category, int mat, bool isHardmode)
    {
        _name = name;
        _category = category;
        _material = mat;
        _isHardmode = isHardmode;
    }

    public override ModItem Clone(Item newEntity)
    {
        var clone = base.Clone(newEntity) as BaseHairpin;
        clone._material = _material;
        clone._category = _category;
        clone._name = _name;
        return clone;
    }

    public override void SetStaticDefaults()
    {
        _jewels ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Hairpins/HairpinJewels");
        _jewelsEquip ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Hairpins/HairpinJewels_Face");
    }

    public override void Unload() => _jewels = null;

    protected override void Defaults()
    {
        Item.width = 36;
        Item.height = 36;
        Item.accessory = true;
    }

    protected override void EquipEffect(Player player, bool isVanity = false)
    {
        if (Info.Count != 0)
            player.GetModPlayer<MaterialPlayer>().SetEquip(EquipType.Face, new MaterialPlayer.EquipLayerInfo(GetDisplayColor(), _jewelsEquip.Value));
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color i, Vector2 origin, float scale)
    {
        if (Info.Count != 0)
            spriteBatch.Draw(_jewels.Value, position, frame, GetDisplayColor(), 0f, origin, scale, SpriteEffects.None, 0);

        base.PostDrawInInventory(spriteBatch, position, frame, drawColor, i, origin, scale);
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);

        if (Info.Count != 0)
        {
            Color col = lightColor.MultiplyRGB(GetDisplayColor());
            spriteBatch.Draw(_jewels.Value, Item.Center - Main.screenPosition, null, col, rotation, _jewels.Size() / 2f, scale, SpriteEffects.None, 0);
        }
    }

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient(_material, 2)
        .AddTile(_isHardmode ? TileID.MythrilAnvil : TileID.Anvils)
        .Register()
        .DisableRecipe();

    public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
    {
        if (equippedItem.ModItem is BaseHairpin)
            return incomingItem.ModItem is not BaseHairpin;

        return true;
    }
}

internal class HairpinLoader : ILoadable
{
    public void Load(Mod mod)
    {
        // Prehardmode
        AddHairpin("Copper", ItemID.CopperBar);
        AddHairpin("Tin", ItemID.TinBar);
        AddHairpin("Iron", ItemID.IronBar);
        AddHairpin("Lead", ItemID.LeadBar);
        AddHairpin("Silver", ItemID.SilverBar);
        AddHairpin("Tungsten", ItemID.TungstenBar);
        AddHairpin("Gold", ItemID.GoldBar);
        AddHairpin("Platinum", ItemID.PlatinumBar);
        AddHairpin("Demonite", ItemID.DemoniteBar);
        AddHairpin("Crimtane", ItemID.CrimtaneBar);
        AddHairpin("Meteorite", ItemID.MeteoriteBar);
        AddHairpin("Hellstone", ItemID.HellstoneBar);

        // Hardmode
        AddHairpin("Cobalt", ItemID.CobaltBar);
        AddHairpin("Palladium", ItemID.PalladiumBar);
        AddHairpin("Mythril", ItemID.MythrilBar);
        AddHairpin("Orichalcum", ItemID.OrichalcumBar);
        AddHairpin("Adamantite", ItemID.AdamantiteBar);
        AddHairpin("Titanium", ItemID.TitaniumBar);
        AddHairpin("Hallowed", ItemID.HallowedBar);
        AddHairpin("Chlorophyte", ItemID.ChlorophyteBar);
        AddHairpin("Beetle", ItemID.BeetleHusk);
        AddHairpin("Shroomite", ItemID.ShroomiteBar);
        AddHairpin("Spectre", ItemID.SpectreBar);
        AddHairpin("Spooky", ItemID.SpookyWood);
        AddHairpin("Luminite", ItemID.LunarBar);
    }

    private static bool AddHairpin(string category, int material, bool isHardmode = false)
        => ModContent.GetInstance<PeculiarJewelry>().AddContent(new BaseHairpin(category + "Hairpin", category, material, isHardmode));

    public void Unload()
    {
    }
}