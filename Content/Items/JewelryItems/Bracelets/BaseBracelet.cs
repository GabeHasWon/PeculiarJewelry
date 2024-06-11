using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses;
using ReLogic.Content;
using System.Linq;

namespace PeculiarJewelry.Content.Items.JewelryItems.Bracelets;

[AutoloadEquip(EquipType.HandsOn)]
[Autoload(false)]
public class BaseBracelet(string name, string category, int mat) : BasicJewelry
{
    static Asset<Texture2D> _jewels;
    static Asset<Texture2D> _jewelsEquip;

    protected override bool CloneNewInstances => true;

    public override string Name => _name;
    public override string MaterialCategory => _category;

    protected int _material = mat;
    protected string _category = category;
    protected string _name = name;

    public override ModItem Clone(Item newEntity)
    {
        var clone = base.Clone(newEntity) as BaseBracelet;
        clone._material = _material;
        clone._category = _category;
        clone._name = _name;
        return clone;
    }

    public override void SetStaticDefaults()
    {
        _jewels ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Bracelets/BraceletJewel");
        _jewelsEquip ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Bracelets/BraceletJewel_HandsOn");
    }

    public override void Unload() => _jewels = null;

    protected override void Defaults()
    {
        Item.width = 28;
        Item.height = 24;
        Item.accessory = true;
    }

    protected override void EquipEffect(Player player, bool isVanity = false)
    {
        if (Info.Count != 0)
            player.GetModPlayer<MaterialPlayer>().SetEquip(EquipType.HandsOn, new MaterialPlayer.EquipLayerInfo(GetDisplayColor(), _jewelsEquip.Value));
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
        .AddIngredient(_material, 2)
        .AddTile(TileID.Chairs)
        .AddTile(TileID.Tables)
        .Register()
        .DisableRecipe();

    public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
    {
        if (equippedItem.ModItem is BaseBracelet)
            return incomingItem.ModItem is not BaseBracelet;

        return true;
    }
}

internal class BraceletLoader : ILoadable
{
    public void Load(Mod mod)
    {
        // Prehardmode
        AddBracelet("Copper", ItemID.CopperBar);
        AddBracelet("Tin", ItemID.TinBar);
        AddBracelet("Iron", ItemID.IronBar);
        AddBracelet("Lead", ItemID.LeadBar);
        AddBracelet("Silver", ItemID.SilverBar);
        AddBracelet("Tungsten", ItemID.TungstenBar);
        AddBracelet("Gold", ItemID.GoldBar);
        AddBracelet("Platinum", ItemID.PlatinumBar);
        AddBracelet("Demonite", ItemID.DemoniteBar);
        AddBracelet("Crimtane", ItemID.CrimtaneBar);
        AddBracelet("Meteorite", ItemID.MeteoriteBar);
        AddBracelet("Hellstone", ItemID.HellstoneBar);

        //// Hardmode
        AddBracelet("Cobalt", ItemID.CobaltBar);
        AddBracelet("Palladium", ItemID.PalladiumBar);
        AddBracelet("Mythril", ItemID.MythrilBar);
        AddBracelet("Orichalcum", ItemID.OrichalcumBar);
        AddBracelet("Adamantite", ItemID.AdamantiteBar);
        AddBracelet("Titanium", ItemID.TitaniumBar);
        AddBracelet("Hallowed", ItemID.HallowedBar);
        AddBracelet("Chlorophyte", ItemID.ChlorophyteBar);
        AddBracelet("Beetle", ItemID.BeetleHusk);
        AddBracelet("Shroomite", ItemID.ShroomiteBar);
        AddBracelet("Spectre", ItemID.SpectreBar);
        AddBracelet("Spooky", ItemID.SpookyWood);
        AddBracelet("Luminite", ItemID.LunarBar);
    }

    private static bool AddBracelet(string category, int material)
        => ModContent.GetInstance<PeculiarJewelry>().AddContent(new BaseBracelet(category + "Bracelet", category, material));

    public void Unload()
    {
    }
}