using System;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

internal static class AmberAccessoryPool
{
    private static bool Initialized = false;

    private static readonly Dictionary<string, int[]> IDsInProgression = [];

    internal static int Get()
    {
        if (!Initialized)
            Initialize();

        List<int> allIds = [];

        if (NPC.downedMoonlord)
            allIds.AddRange(IDsInProgression["Moonlord"]);

        if (NPC.downedAncientCultist)
            allIds.AddRange(IDsInProgression["PostCultist"]);

        if (NPC.downedGolemBoss)
            allIds.AddRange(IDsInProgression["PostGolem"]);

        if (NPC.downedPlantBoss)
            allIds.AddRange(IDsInProgression["PostPlantera"]);

        if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            allIds.AddRange(IDsInProgression["PostMechs"]);

        if (Main.hardMode)
            allIds.AddRange(IDsInProgression["FirstHardmode"]);

        if (NPC.downedBoss3)
            allIds.AddRange(IDsInProgression["PostSkele"]);

        if (NPC.downedBoss2)
            allIds.AddRange(IDsInProgression["PostEvil"]);

        if (NPC.downedBoss1)
            allIds.AddRange(IDsInProgression["PostEye"]);

        allIds.AddRange(IDsInProgression["PreBoss"]);
        return Main.rand.Next(allIds);
    }

    private static void Initialize()
    {
        IDsInProgression.Add("Moonlord", [ItemID.WingsNebula, ItemID.WingsSolar, ItemID.WingsStardust, ItemID.WingsVortex, ItemID.AnkhShield, ItemID.MasterNinjaGear]);
        IDsInProgression.Add("PostCultist", [ItemID.AnkhCharm, ItemID.PDA, ItemID.BetsyWings, ItemID.RainbowWings]);
        IDsInProgression.Add("PostGolem", [ItemID.EyeoftheGolem, ItemID.FrozenShield, ItemID.Hoverboard, ItemID.MothronWings, ItemID.CelestialShell, ItemID.HeroShield]);
        IDsInProgression.Add("PostPlantera", [ItemID.FrozenTurtleShell, ItemID.CelestialEmblem, ItemID.NecromanticScroll, ItemID.ArchitectGizmoPack, ItemID.PapyrusScarab]);
        IDsInProgression.Add("PostMechs", [ItemID.CharmofMyths, ItemID.StarCloak, ItemID.BundleofBalloons, ItemID.FairyWings, ItemID.MoonCharm, ItemID.AvengerEmblem]);
        IDsInProgression.Add("FirstHardmode", [ItemID.PhilosophersStone, ItemID.StarCloak, ItemID.BundleofBalloons, ItemID.FrostsparkBoots, ItemID.ArcticDivingGear,
            ItemID.Magiluminescence, ItemID.CobaltShield, ItemID.BerserkerGlove]);
        IDsInProgression.Add("PostSkele", [ItemID.SandstorminaBottle, ItemID.CloudinaBalloon, ItemID.RocketBoots, ItemID.LightningBoots, ItemID.BlizzardinaBottle,
            ItemID.MagmaStone, ItemID.MoltenSkullRose]);
        IDsInProgression.Add("PostEvil", [ItemID.HermesBoots, ItemID.AnkletoftheWind, ItemID.FlyingCarpet, ItemID.CloudinaBottle, ItemID.BlizzardinaBottle,
            ItemID.SharkToothNecklace]);
        IDsInProgression.Add("PostEye", [ItemID.BandofRegeneration, ItemID.BandofStarpower, ItemID.NaturesGift, ItemID.Bezoar, ItemID.GuideVoodooDoll, ItemID.AncientChisel]);
        IDsInProgression.Add("PreBoss", [ItemID.Aglet, ItemID.Radar, ItemID.PortableStool, ItemID.Shackle, ItemID.Flipper, ItemID.LuckyHorseshoe, ItemID.FrogLeg,
            ItemID.CordageGuide, ItemID.JellyfishNecklace, ItemID.RainbowString]);

        Initialized = true;
    }
}