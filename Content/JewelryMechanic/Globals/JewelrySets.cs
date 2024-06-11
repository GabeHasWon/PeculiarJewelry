using PeculiarJewelry.Content.Items.JewelryItems;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Globals;

internal class JewelrySets : ModSystem
{
    public static readonly Dictionary<int, string> MaterialToName = new()
    {
        { ItemID.CopperBar, "Copper" }, { ItemID.IronBar, "Iron" }, { ItemID.SilverBar, "Silver" }, { ItemID.GoldBar, "Gold" }, { ItemID.TinBar, "Tin" },
        { ItemID.LeadBar, "Lead" }, { ItemID.TungstenBar, "Tungsten" }, { ItemID.PlatinumBar, "Platinum" }, { ItemID.DemoniteBar, "Demonite" },
        { ItemID.CrimtaneBar, "Crimtane"}, { ItemID.MeteoriteBar, "Meteorite" }, { ItemID.HellstoneBar, "Hellstone"}, { ItemID.CobaltBar, "Cobalt" }, 
        { ItemID.MythrilBar, "Mythril" }, { ItemID.AdamantiteBar, "Adamantite" }, { ItemID.PalladiumBar, "Palladium" }, { ItemID.OrichalcumBar, "Orichalcum" }, 
        { ItemID.TitaniumBar, "Titanium" }, { ItemID.HallowedBar, "Hallowed" }, { ItemID.ChlorophyteBar, "Chlorophyte" }, { ItemID.BeetleHusk, "Beetle" },
        { ItemID.SpectreBar, "Spectre" }, { ItemID.ShroomiteBar, "Shroomite" }, { ItemID.SpookyWood, "Spooky" }, { ItemID.LunarBar, "Luminite" }
    };
    
    public static readonly Dictionary<int, HashSet<int>> MatIdToItemSet = [];

    public override void PostSetupContent()
    {
        foreach (int id in MaterialToName.Keys)
        {
            HashSet<int> set = [];

            foreach (var item in ModContent.GetContent<ModItem>())
            {
                if (item is BasicJewelry jewelry && jewelry.MaterialCategory == MaterialToName[id])
                    set.Add(item.Type);
            }

            MatIdToItemSet.Add(id, set);
        }
    }
}
