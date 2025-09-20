using PeculiarJewelry.Content.Items;
using PeculiarJewelry.Content.JewelryMechanic.Desecration;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.IO;

namespace PeculiarJewelry.Content.NPCs;

internal class BossLootGlobal : GlobalNPC
{
    public static ref JewelTier HighestBeatenTier => ref ModContent.GetInstance<TierStorage>().highestBeatenTier;

    public static JewelTier GetBossTier(NPC npc)
    {
        Mod checklist = ModLoader.GetMod("BossChecklist");
        var dict = (Dictionary<string, Dictionary<string, object>>)checklist.Call("GetBossInfoDictionary", ModLoader.GetMod("PeculiarJewelry"), "0.0.0.0");
        string index = dict.Keys.FirstOrDefault(x => (dict[x]["npcIDs"] as List<int>).Contains(npc.type));

        if (index is null)
            return JewelTier.Invalid;

        float progression = (float)dict[index]["progression"];

        if (progression == (int)progression)
            return (JewelTier)progression;
        else
            return (JewelTier)(Main.rand.NextFloat() < progression % 1 ? progression + 1 : progression);
    }

    public static JewelTier GetBossTierAdjustedByDesecration(NPC npc)
    {
        JewelTier baseTier = GetBossTier(npc);
        baseTier = (JewelTier)(DesecratedSystem.AdditionalJewelTier + (int)baseTier);
        return baseTier;
    }

    public override void ModifyGlobalLoot(GlobalLoot globalLoot) => globalLoot.Add(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<BagOfShinies>()));

    public override void OnKill(NPC npc)
    {
        if (npc.boss)
        {
            JewelTier tier = GetBossTier(npc);

            if (tier == JewelTier.Invalid)
                return;

            HighestBeatenTier = tier;
        }
    }

    private class TierStorage : ModSystem
    {
        internal JewelTier highestBeatenTier = JewelTier.Natural;

        public override void SaveWorldData(TagCompound tag) => tag.Add("worldTier", (byte)highestBeatenTier);
        public override void LoadWorldData(TagCompound tag) => highestBeatenTier = (JewelTier)tag.GetByte("worldTier");
    }
}
