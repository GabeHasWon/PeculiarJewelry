using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses;
using ReLogic.Content;
using Terraria.GameContent;

namespace PeculiarJewelry.Content.Items.JewelryItems.Pauldrons;

[AutoloadEquip(EquipType.Body)]
[Autoload(false)]
public class BasePauldrons(string name, string category, int mat) : BasicJewelry
{
    static Asset<Texture2D> _jewels;
    static Asset<Texture2D> _jewelsEquip;

    protected override bool CloneNewInstances => true;

    public override string Name => _name;
    public override string MaterialCategory => _category;

    protected int _material = mat;
    protected string _category = category;
    protected string _name = name;

    private int _frame;

    public override ModItem Clone(Item newEntity)
    {
        var clone = base.Clone(newEntity) as BasePauldrons;
        clone._material = _material;
        clone._category = _category;
        clone._frame = _frame;
        clone._name = _name;
        return clone;
    }

    public override void SetStaticDefaults()
    {
        _jewels ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Pauldrons/Jewels");
        _jewelsEquip ??= Mod.Assets.Request<Texture2D>("Content/Items/JewelryItems/Pauldrons/Jewels_Body");
    }

    public override void Unload() => _jewels = null;

    protected override void Defaults()
    {
        Item.width = 46;
        Item.height = 20;
        Item.accessory = false;

        _frame = Main.rand.Next(2);
    }

    protected override void EquipEffect(Player player, bool isVanity = false)
    {
        if (Info.Count != 0)
        {
            player.GetModPlayer<MaterialPlayer>().SetEquip(EquipType.Body, new MaterialPlayer.EquipLayerInfo(GetDisplayColor(), _jewelsEquip.Value));
            // Dumb workaround for non-standard layers
            player.GetModPlayer<MaterialPlayer>().SetEquip((EquipType)(-1), new MaterialPlayer.EquipLayerInfo(GetDisplayColor(), _jewelsEquip.Value));
        }
    }

    public override bool PreDrawInInventory(SpriteBatch s, Vector2 p, Rectangle frame, Color d, Color itemColor, Vector2 o, float sc) => VariantDraw(s, p, d, o, sc * 0.7f);
    public override bool PreDrawInWorld(SpriteBatch s, Color d, Color a, ref float r, ref float sc, int w) =>
        VariantDraw(s, Item.Center - Main.screenPosition, d, null, sc / 2f, r);

    private bool VariantDraw(SpriteBatch spriteBatch, Vector2 position, Color drawColor, Vector2? origin, float scale, float rotation = 0f)
    {
        Rectangle rect = new(0, 26 * _frame, 46, 24);
        Texture2D tex = TextureAssets.Item[Type].Value;

        origin ??= new Vector2(24, 26);

        spriteBatch.Draw(tex, position, rect, drawColor, rotation, origin.Value / new Vector2(1, 2f), scale * 2, SpriteEffects.None, 0);
        return false;
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color i, Vector2 origin, float scale)
    {
        if (Info.Count != 0)
            spriteBatch.Draw(_jewels.Value, position, frame, GetDisplayColor(), 0f, origin / new Vector2(1, 2), scale * 2, SpriteEffects.None, 0);

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
        .AddIngredient(_material, 10)
        .AddTile(TileID.Anvils)
        .Register()
        .DisableRecipe();
}

internal class PauldronLoader : ILoadable
{
    public void Load(Mod mod)
    {
        // Prehardmode
        AddPauldrons("Copper", ItemID.CopperBar);
        AddPauldrons("Tin", ItemID.TinBar);
        AddPauldrons("Iron", ItemID.IronBar);
        AddPauldrons("Lead", ItemID.LeadBar);
        AddPauldrons("Silver", ItemID.SilverBar);
        AddPauldrons("Tungsten", ItemID.TungstenBar);
        AddPauldrons("Gold", ItemID.GoldBar);
        AddPauldrons("Platinum", ItemID.PlatinumBar);
        AddPauldrons("Demonite", ItemID.DemoniteBar);
        AddPauldrons("Crimtane", ItemID.CrimtaneBar);
        AddPauldrons("Meteorite", ItemID.MeteoriteBar);
        AddPauldrons("Hellstone", ItemID.HellstoneBar);

        //// Hardmode
        AddPauldrons("Cobalt", ItemID.CobaltBar);
        AddPauldrons("Palladium", ItemID.PalladiumBar);
        AddPauldrons("Mythril", ItemID.MythrilBar);
        AddPauldrons("Orichalcum", ItemID.OrichalcumBar);
        AddPauldrons("Adamantite", ItemID.AdamantiteBar);
        AddPauldrons("Titanium", ItemID.TitaniumBar);
        AddPauldrons("Hallowed", ItemID.HallowedBar);
        AddPauldrons("Chlorophyte", ItemID.ChlorophyteBar);
        AddPauldrons("Beetle", ItemID.BeetleHusk);
        AddPauldrons("Shroomite", ItemID.ShroomiteBar);
        AddPauldrons("Spectre", ItemID.SpectreBar);
        AddPauldrons("Spooky", ItemID.SpookyWood);
        AddPauldrons("Luminite", ItemID.LunarBar);
    }

    private static bool AddPauldrons(string category, int material)
        => ModContent.GetInstance<PeculiarJewelry>().AddContent(new BasePauldrons(category + "Pauldrons", category, material));

    public void Unload()
    {
    }
}