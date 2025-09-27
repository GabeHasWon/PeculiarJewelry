using PeculiarJewelry.Content.Items.Jewels.Rares.Amber;
using PeculiarJewelry.Content.Items.Jewels.Rares.Ancient;
using PeculiarJewelry.Content.Items.Jewels.Rares.Gelid;
using PeculiarJewelry.Content.Items.Jewels.Rares.Lucky;
using PeculiarJewelry.Content.Items.Jewels.Rares.Pearl;
using PeculiarJewelry.Content.Items.Jewels.Rares.Soulstone;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace PeculiarJewelry.Content.JewelryMechanic;

internal class JewelRarePool
{
    [Flags]
    public enum OpenRareTypes
    {
        None = 0,
        Pearl,
        Cursed,
        Amber,
        Soulstone,
        Lucky,
        Opal,
        CatsEye,
        Spectrolite,
        Moonstone,
        Ancient,
        Gelid
    }

    public static bool CheckForBiomes(Player player, out OpenRareTypes flags, out int count)
    {
        count = 0;
        flags = OpenRareTypes.None;

        if (player.ZoneForest || player.ZoneNormalUnderground) // This doesn't count as a biome
        {
            flags |= OpenRareTypes.Lucky;
            return false;
        }

        if (player.ZoneBeach) // Pearls
            flags |= OpenRareTypes.Pearl;

        if (player.ZoneCorrupt || player.ZoneCrimson)
            flags |= OpenRareTypes.Cursed;

        if (player.ZoneDesert)
            flags |= OpenRareTypes.Amber;

        if (player.ZoneDungeon)
            flags |= OpenRareTypes.Soulstone;

        if (player.ZoneJungle || player.ZoneGlowshroom)
            flags |= OpenRareTypes.Opal;

        if (player.ZoneUnderworldHeight)
            flags |= OpenRareTypes.CatsEye;

        if (player.ZoneSnow)
            flags |= OpenRareTypes.Spectrolite;

        if (player.ZoneSkyHeight)
            flags |= OpenRareTypes.Moonstone;

        if (player.ZoneGranite || player.ZoneMarble)
            flags |= OpenRareTypes.Ancient;

        if (player.ZoneHallow)
            flags |= OpenRareTypes.Gelid;

        return BitOperations.PopCount((uint)flags) > 0;
    }

    public static int GetRareJewelType(OpenRareTypes rareTypes)
    {
        List<int> types = [];

        if (rareTypes.HasFlag(OpenRareTypes.Lucky))
            types.Add(JewelryCommon.MajorMinorType<MajorLucky, MinorLucky>());

        if (rareTypes.HasFlag(OpenRareTypes.Amber))
            types.Add(JewelryCommon.MajorMinorType<PureAmber, FullAmber>());

        if (rareTypes.HasFlag(OpenRareTypes.Gelid))
            types.Add(JewelryCommon.MajorMinorType<MajorGelid, MinorGelid>());

        if (rareTypes.HasFlag(OpenRareTypes.Ancient))
            types.Add(JewelryCommon.MajorMinorType<AncientMajor, AncientMinor>());

        if (rareTypes.HasFlag(OpenRareTypes.Pearl))
            types.Add(JewelryCommon.MajorMinorType<MajorPearl, MinorPearl>());

        if (rareTypes.HasFlag(OpenRareTypes.Soulstone))
            types.Add(JewelryCommon.MajorMinorType<MajorSoulstone, MinorSoulstone>());

        return types.Count == 0 ? -1 : Main.rand.Next(types);
    }
}
