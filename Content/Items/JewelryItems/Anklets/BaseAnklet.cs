using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses;
using ReLogic.Content;
using System.Linq;

namespace PeculiarJewelry.Content.Items.JewelryItems.Anklets;

[AutoloadEquip(EquipType.Shoes)]
[Autoload(false)]
public class BaseAnklet : BasicJewelry
{
    static Asset<Texture2D> _jewels;
    static Asset<Texture2D> _jewelsEquip;

    protected override bool CloneNewInstances => true;

    public override string Name => _name;
    public override string MaterialCategory => _category;

    protected int _material;
    protected string _category = string.Empty;
    protected string _name = string.Empty;

    public BaseAnklet(string name, string category, int mat)
    {
        _name = name;
        _category = category;
        _material = mat;
    }

    public override ModItem Clone(Item newEntity)
    {
        var clone = base.Clone(newEntity) as BaseAnklet;
        clone._material = _material;
        clone._category = _category;
        clone._name = _name;
        return clone;
    }

    public override void SetStaticDefaults()
    {
        _jewels ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Anklets/Jewels");
        _jewelsEquip ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Anklets/JewelsEquip");
    }

    public override void Unload() => _jewels = null;

    protected override void Defaults()
    {
        Item.width = 38;
        Item.height = 28;
        Item.accessory = true;
    }

    protected override void EquipEffect(Player player, bool isVanity = false)
    {
        if (Info.Any())
            player.GetModPlayer<MaterialPlayer>().SetEquip(EquipType.Shoes, new MaterialPlayer.EquipLayerInfo(GetDisplayColor(), _jewelsEquip.Value));
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color i, Vector2 origin, float scale)
    {
        if (Info.Any())
            spriteBatch.Draw(_jewels.Value, position, frame, GetDisplayColor(), 0f, origin, scale, SpriteEffects.None, 0);
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        if (Info.Any())
            spriteBatch.Draw(_jewels.Value, Item.Center - Main.screenPosition, null, GetDisplayColor(), rotation, _jewels.Size() / 2f, scale, SpriteEffects.None, 0);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(_material, 4)
            .AddTile(TileID.Chairs)
            .AddTile(TileID.Tables)
            .Register();
    }

    public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
    {
        if (equippedItem.ModItem is BaseAnklet)
            return incomingItem.ModItem is not BaseAnklet;

        return true;
    }
}

internal class AnkletLoader : ILoadable
{
    public void Load(Mod mod)
    {
        // Prehardmode
        AddAnklet("Copper", ItemID.CopperBar);
        AddAnklet("Tin", ItemID.TinBar);
        AddAnklet("Iron", ItemID.IronBar);
        AddAnklet("Lead", ItemID.LeadBar);
        AddAnklet("Silver", ItemID.SilverBar);
        AddAnklet("Tungsten", ItemID.TungstenBar);
        AddAnklet("Gold", ItemID.GoldBar);
        AddAnklet("Platinum", ItemID.PlatinumBar);
        AddAnklet("Demonite", ItemID.DemoniteBar);
        AddAnklet("Crimtane", ItemID.CrimtaneBar);
        AddAnklet("Meteorite", ItemID.MeteoriteBar);
        AddAnklet("Hellstone", ItemID.HellstoneBar);

        // Hardmode
        AddAnklet("Cobalt", ItemID.CobaltBar);
        AddAnklet("Palladium", ItemID.PalladiumBar);
        AddAnklet("Mythril", ItemID.MythrilBar);
        AddAnklet("Orichalcum", ItemID.OrichalcumBar);
        AddAnklet("Adamantite", ItemID.AdamantiteBar);
        AddAnklet("Titanium", ItemID.TitaniumBar);
        AddAnklet("Hallowed", ItemID.HallowedBar);
        AddAnklet("Chlorophyte", ItemID.ChlorophyteBar);
        AddAnklet("Beetle", ItemID.BeetleHusk);
        AddAnklet("Shroomite", ItemID.ShroomiteBar);
        AddAnklet("Spectre", ItemID.SpectreBar);
        AddAnklet("Spooky", ItemID.SpookyWood);
        AddAnklet("Luminite", ItemID.LunarBar);
    }

    private static bool AddAnklet(string category, int material)
        => ModContent.GetInstance<PeculiarJewelry>().AddContent(new BaseAnklet(category + "Anklet", category, material));

    public void Unload()
    {
    }
}