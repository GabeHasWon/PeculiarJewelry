using System;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal static class AmberAccessoryPool
{
    private static bool Initialized = false;

    private static Dictionary<string, int[]> OptionsPerProgression = [];

    internal static int Get()
    {
        if (!Initialized)
            Initialize();

        List<int> allIds = [];

        if (NPC.downedMoonlord)
            allIds.AddRange(OptionsPerProgression["Moonlord"]);

        if (NPC.downedAncientCultist)
            allIds.AddRange(OptionsPerProgression["PostCultist"]);
        
        if (NPC.downedGolemBoss)
            allIds.AddRange(OptionsPerProgression["PostGolem"]);
        
        if (NPC.downedPlantBoss)
            allIds.AddRange(OptionsPerProgression["PostPlantera"]);
        
        if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            allIds.AddRange(OptionsPerProgression["PostMechs"]);
        
        if (Main.hardMode)
            allIds.AddRange(OptionsPerProgression["FirstHardmode"]);
        
        if (NPC.downedBoss3)
            allIds.AddRange(OptionsPerProgression["PostSkele"]);

        if (NPC.downedBoss2)
            allIds.AddRange(OptionsPerProgression["PostEvil"]);

        if (NPC.downedBoss1)
            allIds.AddRange(OptionsPerProgression["PostEye"]);

        allIds.AddRange(OptionsPerProgression["PreBoss"]);
        return Main.rand.Next(allIds);
    }

    private static void Initialize()
    {
        OptionsPerProgression.Add("Moonlord", [ItemID.WingsNebula, ItemID.WingsSolar, ItemID.WingsStardust, ItemID.WingsVortex, ItemID.AnkhShield, ItemID.MasterNinjaGear]);
        OptionsPerProgression.Add("PostCultist", [ItemID.AnkhCharm, ItemID.PDA, ItemID.BetsyWings, ItemID.RainbowWings]);
        OptionsPerProgression.Add("PostGolem", [ItemID.EyeoftheGolem, ItemID.FrozenShield, ItemID.Hoverboard, ItemID.MothronWings, ItemID.CelestialShell, ItemID.HeroShield]);
        OptionsPerProgression.Add("PostPlantera", [ItemID.FrozenTurtleShell, ItemID.CelestialEmblem, ItemID.NecromanticScroll, ItemID.ArchitectGizmoPack, ItemID.PapyrusScarab]);
        OptionsPerProgression.Add("PostMechs", [ItemID.CharmofMyths, ItemID.StarCloak, ItemID.BundleofBalloons, ItemID.FairyWings, ItemID.MoonCharm, ItemID.AvengerEmblem]);
        OptionsPerProgression.Add("FirstHardmode", [ItemID.PhilosophersStone, ItemID.StarCloak, ItemID.BundleofBalloons, ItemID.FrostsparkBoots, ItemID.ArcticDivingGear,
            ItemID.Magiluminescence, ItemID.CobaltShield, ItemID.BerserkerGlove]);
        OptionsPerProgression.Add("PostSkele", [ItemID.SandstorminaBottle, ItemID.CloudinaBalloon, ItemID.RocketBoots, ItemID.LightningBoots, ItemID.BlizzardinaBottle,
            ItemID.MagmaStone, ItemID.MoltenSkullRose]);
        OptionsPerProgression.Add("PostEvil", [ItemID.HermesBoots, ItemID.AnkletoftheWind, ItemID.FlyingCarpet, ItemID.CloudinaBottle, ItemID.BlizzardinaBottle,
            ItemID.SharkToothNecklace]);
        OptionsPerProgression.Add("PostEye", [ItemID.BandofRegeneration, ItemID.BandofStarpower, ItemID.NaturesGift, ItemID.Bezoar, ItemID.GuideVoodooDoll, ItemID.AncientChisel]);
        OptionsPerProgression.Add("PreBoss", [ItemID.Aglet, ItemID.Radar, ItemID.PortableStool, ItemID.Shackle, ItemID.Flipper, ItemID.LuckyHorseshoe, ItemID.FrogLeg, 
            ItemID.CordageGuide, ItemID.JellyfishNecklace, ItemID.RainbowString]);

        Initialized = true;
    }
}