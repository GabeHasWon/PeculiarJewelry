using PeculiarJewelry.Content.Items.Jewels.Rares.Amber;
using PeculiarJewelry.Content.Items.Jewels.Rares.Gelid;
using PeculiarJewelry.Content.Items.Jewels.Rares.Lucky;
using System;
using System.Collections.Generic;

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
        {
            count++;
            flags |= OpenRareTypes.Pearl;
        }

        if (player.ZoneCorrupt || player.ZoneCrimson) // Cursed
        {
            count++;
            flags |= OpenRareTypes.Cursed;
        }

        if (player.ZoneDesert) // Amber
        {
            count++;
            flags |= OpenRareTypes.Amber;
        }

        if (player.ZoneDungeon) // Soulstone
        {
            count++;
            flags |= OpenRareTypes.Soulstone;
        }

        if (player.ZoneJungle || player.ZoneGlowshroom)
        {
            count++;
            flags |= OpenRareTypes.Opal;
        }

        if (player.ZoneUnderworldHeight)
        {
            count++;
            flags |= OpenRareTypes.CatsEye;
        }

        if (player.ZoneSnow)
        {
            count++;
            flags |= OpenRareTypes.Spectrolite;
        }

        if (player.ZoneSkyHeight)
        {
            count++;
            flags |= OpenRareTypes.Moonstone;
        }

        if (player.ZoneGranite || player.ZoneMarble)
        {
            count++;
            flags |= OpenRareTypes.Ancient;
        }

        if (player.ZoneHallow)
        {
            count++;
            flags |= OpenRareTypes.Gelid;
        }

        return count > 0;
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

        return types.Count == 0 ? -1 : Main.rand.Next(types);
    }
}
