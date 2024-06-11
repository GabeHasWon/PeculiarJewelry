using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses;
using ReLogic.Content;
using System.Linq;

namespace PeculiarJewelry.Content.Items.JewelryItems.Chatelaines;

[AutoloadEquip(EquipType.Waist)]
[Autoload(false)]
public class BaseChatelaine : BasicJewelry
{
    static Asset<Texture2D> _jewels;
    static Asset<Texture2D> _jewelsEquip;

    protected override bool CloneNewInstances => true;

    public override string Name => _name;
    public override string MaterialCategory => _category;

    protected int _material;
    protected string _category = string.Empty;
    protected string _name = string.Empty;

    public BaseChatelaine(string name, string category, int mat)
    {
        _name = name;
        _category = category;
        _material = mat;
    }

    public override ModItem Clone(Item newEntity)
    {
        var clone = base.Clone(newEntity) as BaseChatelaine;
        clone._material = _material;
        clone._category = _category;
        clone._name = _name;
        return clone;
    }

    public override void SetStaticDefaults()
    {
        _jewels ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Chatelaines/Jewel");
        _jewelsEquip ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Chatelaines/JewelEquip");
    }

    public override void Unload() => _jewels = null;

    protected override void Defaults()
    {
        Item.width = 46;
        Item.height = 44;
        Item.accessory = true;
    }

    protected override void EquipEffect(Player player, bool isVanity = false)
    {
        if (Info.Count != 0)
            player.GetModPlayer<MaterialPlayer>().SetEquip(EquipType.Waist, new MaterialPlayer.EquipLayerInfo(GetDisplayColor(), _jewelsEquip.Value));
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color i, Vector2 origin, float scale)
    {
        if (Info.Count != 0)
            spriteBatch.Draw(_jewels.Value, position, frame, GetDisplayColor(), 0f, origin, scale, SpriteEffects.None, 0);

        base.PostDrawInInventory(spriteBatch, position, frame, drawColor, i, origin, scale);
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
        .AddIngredient(_material, 8)
        .AddTile(TileID.Anvils)
        .Register()
        .DisableRecipe();

    public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
    {
        if (equippedItem.ModItem is BaseChatelaine)
            return incomingItem.ModItem is not BaseChatelaine;

        return true;
    }
}

internal class BaseChatelaineLoader : ILoadable
{
    public void Load(Mod mod)
    {
        // Prehardmode
        AddChatelaine("Copper", ItemID.CopperBar);
        AddChatelaine("Tin", ItemID.TinBar);
        AddChatelaine("Iron", ItemID.IronBar);
        AddChatelaine("Lead", ItemID.LeadBar);
        AddChatelaine("Silver", ItemID.SilverBar);
        AddChatelaine("Tungsten", ItemID.TungstenBar);
        AddChatelaine("Gold", ItemID.GoldBar);
        AddChatelaine("Platinum", ItemID.PlatinumBar);
        AddChatelaine("Demonite", ItemID.DemoniteBar);
        AddChatelaine("Crimtane", ItemID.CrimtaneBar);
        AddChatelaine("Meteorite", ItemID.MeteoriteBar);
        AddChatelaine("Hellstone", ItemID.HellstoneBar);

        ////Hardmode
        AddChatelaine("Cobalt", ItemID.CobaltBar);
        AddChatelaine("Palladium", ItemID.PalladiumBar);
        AddChatelaine("Mythril", ItemID.MythrilBar);
        AddChatelaine("Orichalcum", ItemID.OrichalcumBar);
        AddChatelaine("Adamantite", ItemID.AdamantiteBar);
        AddChatelaine("Titanium", ItemID.TitaniumBar);
        AddChatelaine("Hallowed", ItemID.HallowedBar);
        AddChatelaine("Chlorophyte", ItemID.ChlorophyteBar);
        AddChatelaine("Beetle", ItemID.BeetleHusk);
        AddChatelaine("Shroomite", ItemID.ShroomiteBar);
        AddChatelaine("Spectre", ItemID.SpectreBar);
        AddChatelaine("Spooky", ItemID.SpookyWood);
        AddChatelaine("Luminite", ItemID.LunarBar);
    }

    private static bool AddChatelaine(string category, int material)
        => ModContent.GetInstance<PeculiarJewelry>().AddContent(new BaseChatelaine(category + "Chatelaine", category, material));

    public void Unload()
    {
    }
}