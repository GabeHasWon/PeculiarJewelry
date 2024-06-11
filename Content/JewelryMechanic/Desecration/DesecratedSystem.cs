using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace PeculiarJewelry.Content.JewelryMechanic.Desecration;

public class DesecratedSystem : ModSystem
{
    internal readonly Dictionary<string, DesecrationModifier> Desecrations = [];

    public static float TotalProfanity => ModContent.GetInstance<DesecratedSystem>()._totalProfanity;
    public static float LootScaleFactor => ModContent.GetInstance<DesecratedSystem>()._totalProfanity * 5 / 100f;
    public static int AdditionalJewelTier => (int)(ModContent.GetInstance<DesecratedSystem>()._totalProfanity / 2f);

    internal int timesGivenUp = 0;

    private float _totalProfanity = 0;

    public void SetDesecration(string key, float strength)
    {
        DesecrationModifier.Desecrations[key].strength = strength;

        if (strength <= 0)
            Desecrations.Remove(key);
        else
        {
            if (Desecrations.TryGetValue(key, out DesecrationModifier value))
                value.strength = strength;
            else
                Desecrations.Add(key, DesecrationModifier.Desecrations[key]);
        }

        GetTotalProfanity();
    }

    private void GetTotalProfanity()
    {
        _totalProfanity = 0;

        foreach (var item in Desecrations.Values)
            _totalProfanity += item.strength * item.Profanity * ModContent.GetInstance<JewelryStatConfig>().ProfanityStrength;
    }

    public void ClearDesecrations()
    {
        foreach (var item in Desecrations.Values)
            item.strength = 0;

        Desecrations.Clear();
    }

    public override void SaveWorldData(TagCompound tag)
    {
        tag.Add(nameof(timesGivenUp), timesGivenUp);
        tag.Add("desecrationsCount", Desecrations.Values.Count);
        int index = 0;

        foreach (var item in Desecrations.Values)
        {
            tag.Add("desecration" + index, item.Name);
            tag.Add("desecrationValue" + index++, item.strength);
        }
    }

    public override void LoadWorldData(TagCompound tag)
    {
        if (tag.TryGet(nameof(timesGivenUp), out object value) && value is int integer)
            timesGivenUp = integer;

        if (!tag.TryGet("desecrationsCount", out int count))
            return;

        for (int i = 0; i < count; i++)
        {
            string key = tag.GetString("desecration" + i);
            float str = tag.GetFloat("desecrationValue" + i);
            SetDesecration(key, str);
        }
    }

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) => tasks.Insert(0, new PassLegacy("Peculiar Jewelry: Clear Desecrations", (_, _) => ClearDesecrations()));
    public override void OnWorldLoad() => ClearDesecrations();

    internal class DesecrationNPC : GlobalNPC
    {
        private static DesecratedSystem System => ModContent.GetInstance<DesecratedSystem>();

        public override bool AppliesToEntity(NPC npc, bool lateInstantiation) => !npc.friendly && !npc.townNPC && npc.lifeMax > 5;

        public override void SetDefaults(NPC npc)
        {
            if (!AppliesToEntity(npc, false))
                return;

            foreach (var item in System.Desecrations.Values)
                item.PostSetDefaults(npc);
        }

        public override bool PreAI(NPC npc)
        {
            if (!AppliesToEntity(npc, false))
                return true;

            foreach (var item in System.Desecrations.Values)
                item.PreAI(npc);

            return true;
        }

        public override void ResetEffects(NPC npc)
        {
            if (!AppliesToEntity(npc, false))
                return;

            foreach (var item in System.Desecrations.Values)
                item.ResetEffects(npc);
        }
    }
}