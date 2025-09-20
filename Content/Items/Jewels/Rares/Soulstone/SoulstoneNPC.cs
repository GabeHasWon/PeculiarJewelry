using PeculiarJewelry.Content.JewelryMechanic.Misc.SoulstoneDebuffs;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.Items.Jewels.Rares.Soulstone;

internal class SoulstoneNPC : GlobalNPC
{
    private class SoulstoneStack
    {
        public int Stacks = 0;
        public float TimeToReset = 0;

        public override string ToString() => $"{Stacks} {TimeToReset}";
    }

    public override bool InstancePerEntity => true;

    private readonly Dictionary<StatType, SoulstoneStack> StatTypeStacks = new()
    {
        { StatType.SoulAgony, new() },
        { StatType.SoulGrief, new() },
        { StatType.SoulBetrayal, new() },
        { StatType.SoulPlague, new() },
        { StatType.SoulSacrifice, new() },
        { StatType.SoulTorture, new() },
    };

    public override void ResetEffects(NPC npc)
    {
        foreach (var value in StatTypeStacks.Values)
        {
            value.TimeToReset--;

            if (value.TimeToReset <= 0)
                value.Stacks = 0;
        }
    }

    public static void AddStack(NPC npc, StatType type)
    {
        ref int stacks = ref npc.GetGlobalNPC<SoulstoneNPC>().StatTypeStacks[type].Stacks;
        ref float time = ref npc.GetGlobalNPC<SoulstoneNPC>().StatTypeStacks[type].TimeToReset;
        stacks++;
        time = 15 * 60;

        if (stacks >= 4)
        {
            npc.AddBuff(GetBuffType(type), 10 * 60);
            stacks = 0;
        }
    }

    private static int GetBuffType(StatType type) => type switch
    {
        StatType.SoulAgony => ModContent.BuffType<AgonyDebuff>(),
        StatType.SoulTorture => ModContent.BuffType<TortureDebuff>(),
        StatType.SoulSacrifice => ModContent.BuffType<SacrificeDebuff>(),
        _ => BuffID.OnFire,
    };
}
