using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses;
using ReLogic.Content;
using System.Linq;

namespace PeculiarJewelry.Content.Items.JewelryItems.Skirts;

[AutoloadEquip(EquipType.Legs)]
[Autoload(false)]
public class BaseSkirt(string name, string category, int mat) : BasicJewelry
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
        var clone = base.Clone(newEntity) as BaseSkirt;
        clone._material = _material;
        clone._category = _category;
        clone._name = _name;
        return clone;
    }

    public override void SetStaticDefaults()
    {
        _jewels ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Skirts/Jewel");
        _jewelsEquip ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Skirts/JewelEquip");
    }

    public override void Unload() => _jewels = null;

    protected override void Defaults()
    {
        Item.width = 52;
        Item.height = 36;
        Item.accessory = false;
        Item.defense = (int)((float)tier * 0.667f);
    }

    protected override void EquipEffect(Player player, bool isVanity = false)
    {
        if (Info.Count != 0 && (isVanity || player.armor[12].IsAir))
            player.GetModPlayer<MaterialPlayer>().SetEquip(EquipType.Legs, new MaterialPlayer.EquipLayerInfo(GetDisplayColor(), _jewelsEquip.Value));
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

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(_material, 4)
            .AddIngredient(ItemID.Silk, 6)
            .AddTile(TileID.Loom)
            .Register()
            .DisableRecipe();
    }

    public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
    {
        if (equippedItem.ModItem is BaseSkirt)
            return incomingItem.ModItem is not BaseSkirt;

        return true;
    }
}

internal class SkirtLoader : ILoadable
{
    public void Load(Mod mod)
    {
        // Prehardmode
        AddSkirt("Copper", ItemID.CopperBar);
        AddSkirt("Tin", ItemID.TinBar);
        AddSkirt("Iron", ItemID.IronBar);
        AddSkirt("Lead", ItemID.LeadBar);
        AddSkirt("Silver", ItemID.SilverBar);
        AddSkirt("Tungsten", ItemID.TungstenBar);
        AddSkirt("Gold", ItemID.GoldBar);
        AddSkirt("Platinum", ItemID.PlatinumBar);
        AddSkirt("Demonite", ItemID.DemoniteBar);
        AddSkirt("Crimtane", ItemID.CrimtaneBar);
        AddSkirt("Meteorite", ItemID.MeteoriteBar);
        AddSkirt("Hellstone", ItemID.HellstoneBar);

        //// Hardmode
        AddSkirt("Cobalt", ItemID.CobaltBar);
        AddSkirt("Palladium", ItemID.PalladiumBar);
        AddSkirt("Mythril", ItemID.MythrilBar);
        AddSkirt("Orichalcum", ItemID.OrichalcumBar);
        AddSkirt("Adamantite", ItemID.AdamantiteBar);
        AddSkirt("Titanium", ItemID.TitaniumBar);
        AddSkirt("Hallowed", ItemID.HallowedBar);
        AddSkirt("Chlorophyte", ItemID.ChlorophyteBar);
        AddSkirt("Beetle", ItemID.BeetleHusk);
        AddSkirt("Shroomite", ItemID.ShroomiteBar);
        AddSkirt("Spectre", ItemID.SpectreBar);
        AddSkirt("Spooky", ItemID.SpookyWood);
        AddSkirt("Luminite", ItemID.LunarBar);
    }

    private static bool AddSkirt(string category, int material)
        => ModContent.GetInstance<PeculiarJewelry>().AddContent(new BaseSkirt(category + "Skirt", category, material));

    public void Unload()
    {
    }
}