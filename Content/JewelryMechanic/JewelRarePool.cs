using FullSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        return -1;
    }
}
