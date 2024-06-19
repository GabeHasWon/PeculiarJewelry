using Mono.Cecil;
using PeculiarJewelry.Content.Items;
using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.Items.Jewels;
using PeculiarJewelry.Content.Items.Jewels.Rares.Impure;
using PeculiarJewelry.Content.Items.JewelSupport;
using PeculiarJewelry.Content.Items.Pliers;
using PeculiarJewelry.Content.Items.RadiantEchoes;
using PeculiarJewelry.Content.Items.Tiles;
using PeculiarJewelry.Content.JewelryMechanic;
using StockableShops.Stock;
using System;
using Terraria.Utilities;

namespace PeculiarJewelry.Content.NPCs;

internal class LapidaristStock : StockedShop
{
    private const int JewelCount = 4;
    private const int JewelryCount = 5;

    internal static WeightedRandom<int> MiscPool
    {
        get
        {
            var pool = new WeightedRandom<int>();
            pool.Add(ModContent.ItemType<CursedDollar>(), 1);
            pool.Add(ModContent.ItemType<IrradiatedPearl>(), 2);
            pool.Add(ModContent.ItemType<GoldenCarpScales>(), 2);
            pool.Add(ModContent.ItemType<CelestialEye>(), 2);
            pool.Add(ModContent.ItemType<BrokenStopwatch>(), 4);
            pool.Add(ModContent.ItemType<StellarJade>(), 1);
            pool.Add(ModContent.ItemType<How2GetRich>(), 0.15f);
            pool.Add(ModContent.ItemType<HeavenlyRevelation>(), 1);
            pool.Add(ModContent.ItemType<TranscendantEcho>(), 1);
            return pool;
        }
    }

    private static WeightedRandom<BasicJewelry.JewelryTier> JewelryTierPool
    {
        get
        {
            var pool = new WeightedRandom<BasicJewelry.JewelryTier>(Main.rand);
            pool.Add(BasicJewelry.JewelryTier.Ordinary, 5);
            pool.Add(BasicJewelry.JewelryTier.Pretty, 4);
            pool.Add(BasicJewelry.JewelryTier.Elegant, 3);
            pool.Add(BasicJewelry.JewelryTier.Elaborate, 2);
            pool.Add(BasicJewelry.JewelryTier.Extravagant, 1);
            return pool;
        }
    }

    public override int NPCType => ModContent.NPCType<Lapidarist>();
    public override string RestockCondition => Language.GetTextValue("Mods.PeculiarJewelry.OnceADay");

    private readonly ShopItem[] _jewelItems = new ShopItem[JewelCount];
    private readonly ShopItem[] _jewelryItems = new ShopItem[JewelryCount];

    private int _miscItemID = 0;

    public override void SetupStock(NPC npc)
    {
        // Consistent items
        FullStock.Add(new ShopItem(new Item(ModContent.ItemType<SparklyDust>(), 100) { shopCustomPrice = Item.buyPrice(silver: 10) }));
        FullStock.Add(new ShopItem(new Item(ModContent.ItemType<RoughPliers>(), 15) { shopCustomPrice = Item.buyPrice(gold: 2) }));
        FullStock.Add(new ShopItem(new Item(ModContent.ItemType<NicePliers>(), 10) { shopCustomPrice = Item.buyPrice(gold: 10) }));
        FullStock.Add(new ShopItem(new Item(ModContent.ItemType<GentlePliers>(), 5) { shopCustomPrice = Item.buyPrice(gold: 33) }));
        FullStock.Add(new ShopItem(new Item(ModContent.ItemType<SilkenPliers>(), 1) { shopCustomPrice = Item.buyPrice(platinum: 1) }));
        FullStock.Add(new ShopItem(new Item(ModContent.ItemType<DisplayCase>(), 10)));
        FullStock.Add(new ShopItem(new Item(ModContent.ItemType<Grindstone>(), 1)));

        // Misc, mutually exclusive items
        FullStock.Add(MiscShopItem<CursedDollar>(10, Item.buyPrice(gold: 25)));
        FullStock.Add(MiscShopItem<IrradiatedPearl>(5, Item.buyPrice(gold: 10)));
        FullStock.Add(MiscShopItem<GoldenCarpScales>(5, Item.buyPrice(gold: 15)));
        FullStock.Add(MiscShopItem<CelestialEye>(5, Item.buyPrice(gold: 25)));
        FullStock.Add(MiscShopItem<BrokenStopwatch>(10, Item.buyPrice(gold: 25)));
        FullStock.Add(MiscShopItem<StellarJade>(1, Item.buyPrice(platinum: 1)));
        FullStock.Add(MiscShopItem<How2GetRich>(1, Item.buyPrice(platinum: 5)));
        FullStock.Add(MiscShopItem<HeavenlyRevelation>(1, Item.buyPrice(platinum: 1)));
        FullStock.Add(MiscShopItem<TranscendantEcho>(1, Item.buyPrice(platinum: 1)));

        // Jewels (3), Impure Jewel (1)
        for (int i = 0; i < JewelCount; ++i)
        {
            int type = i == 3 ? JewelryCommon.MajorMinorType<ImpureMajor, ImpureMinor>() : JewelryCommon.MajorMinorType();
            Item item = new(type);
            (item.ModItem as Jewel).info.Setup(BossLootGlobal.HighestBeatenTier);
            _jewelItems[i] = new(item);
            FullStock.Add(_jewelItems[i]);
        }

        // Jewelry (5)
        for (int i = 0; i < JewelryCount; ++i)
        {
            Item item = new(JewelryCommon.GetRandomJewelryType(JewelryCommon.GetAllUnlockedMaterials()));
            (item.ModItem as BasicJewelry).SetTier(JewelryTierPool.Get());
            _jewelryItems[i] = new(item);

            ScalePriceToTier(_jewelryItems[i]);
            FullStock.Add(_jewelryItems[i]);
        }
    }

    private static void ScalePriceToTier(ShopItem shopItem)
    {
        shopItem.Item.value = (shopItem.Item.ModItem as BasicJewelry).tier switch
        {
            BasicJewelry.JewelryTier.Pretty => Item.buyPrice(0, 5),
            BasicJewelry.JewelryTier.Elegant => Item.buyPrice(0, 10),
            BasicJewelry.JewelryTier.Elaborate => Item.buyPrice(0, 25),
            BasicJewelry.JewelryTier.Extravagant => Item.buyPrice(1),
            _ => Item.buyPrice(0, 1),
        };
    }

    protected override void RestockShop(NPC npc, Item[] shop)
    {
        _miscItemID = MiscPool.Get();

        for (int i = 0; i < JewelCount; ++i)
        {
            Item item = new(JewelryCommon.MajorMinorType());
            item.value = Item.buyPrice(0, item.type == ModContent.ItemType<MajorJewel>() ? 15 : 8, 0, 0);
            (item.ModItem as Jewel).info.Setup(BossLootGlobal.HighestBeatenTier);
            _jewelItems[i] = new(item);
        }
    }

    public ShopItem MiscShopItem<T>(int stack, int price) where T : ModItem
    {
        var condition = MiscCondition<T>();
        return new(condition, new Item(ModContent.ItemType<T>(), stack) { shopCustomPrice = price });
    }

    public Condition MiscCondition<T>() where T : ModItem => new("Localization", () => _miscItemID == ModContent.ItemType<T>());
}
