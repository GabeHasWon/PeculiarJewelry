using PeculiarJewelry.Content.Items.Jewels;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic;

internal class JewelryCommon
{
    public static JewelryStatConfig Config => ModContent.GetInstance<JewelryStatConfig>();

    public static string[] PrehardmodeMetals = "Copper Tin Iron Lead Silver Tungsten Gold Platinum".Split(' ');
    public static string[] AllMaterials = ("Copper Tin Iron Lead Silver Tungsten Gold Platinum Meteorite Crimtane Demonite Hellstone" +
        "Cobalt Palladium Mythril Orichalcum Adamantite Titanium Chlorophyte Hallowed Spooky Spectre Shroomite Beetle Luminite").Split(' ');

    public static int MajorMinorType() => Main.rand.NextFloat() <= Config.ChanceForMajor ? ModContent.ItemType<MajorJewel>() : ModContent.ItemType<MinorJewel>();
    public static int MajorMinorType<TMajor, TMinor>() where TMajor : Jewel where TMinor : Jewel
        => Main.rand.NextFloat() <= Config.ChanceForMajor ? ModContent.ItemType<TMajor>() : ModContent.ItemType<TMinor>();

    public static float StatStrengthRange(JewelInfo info)
    {
        if (Config.GlobalPowerScaleMinimum == 1 || Config.PowerScaleStepCount == 1)
            return 1;

        float factor = Main.rand.Next(Config.PowerScaleStepCount) / (float)(Config.PowerScaleStepCount - 1);
        float result = MathHelper.Lerp(Config.GlobalPowerScaleMinimum, 1, factor);

        return result;
    }

    public static int GetRandomJewelryType(string[] materials)
    {
        string[] types = "Anklet Bracelet Brooch Choker Earring Hairpin Ring Tiara".Split(' ');
        string name = nameof(PeculiarJewelry) + "/" + Main.rand.Next(materials) + Main.rand.Next(types);
        return ModContent.Find<ModItem>(name).Type;
    }

    public static string[] GetAllUnlockedMaterials()
    {
        List<string> materials = new(PrehardmodeMetals);

        if (NPC.downedBoss2)
            materials.Add("Meteorite");

        if (NPC.downedBoss3)
        {
            materials.Add("Crimtane");
            materials.Add("Demonite");
        }

        if (Main.hardMode)
            materials.Add("Hellstone");

        if (NPC.downedMechBossAny)
        {
            materials.Add("Cobalt");
            materials.Add("Palladium");
            materials.Add("Mythril");
            materials.Add("Orichalcum");
            materials.Add("Adamantite");
            materials.Add("Titanium");
        }

        if (NPC.downedPlantBoss)
            materials.Add("Hallowed");

        if (NPC.downedGolemBoss)
            materials.Add("Chlorophyte");

        if (NPC.downedAncientCultist)
        {
            materials.Add("Spectre");
            materials.Add("Beetle");
            materials.Add("Shroomite");
        }

        if (NPC.downedMoonlord)
            materials.Add("Luminite");

        return [..materials];
    }
}
