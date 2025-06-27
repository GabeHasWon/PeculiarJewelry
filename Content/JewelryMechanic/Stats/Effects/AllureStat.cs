using System;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects;

/// <summary>
/// Fishing power. MP safe.
/// </summary>
internal class AllureStat : JewelStatEffect
{
    public override StatType Type => StatType.Allure;
    public override Color Color => Color.PaleTurquoise;

    public override StatExclusivity Exclusivity => StatExclusivity.Utility;

    public override void Apply(Player player, float strength) => player.fishingSkill += (int)GetEffectBonus(player, strength);
    protected override float InternalEffectBonus(float multiplier, Player player) => (int)Math.Ceiling(PeculiarJewelry.StatConfig.AllureStrength * multiplier);
}

public class AllureGlobalNPC : GlobalNPC
{
    public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
    {
        int skill = spawnInfo.Player.fishingSkill;

        if (skill > 0)
        {
            return;
        }

        if (skill < -100)
        {
            pool[NPCID.DukeFishron] = MathHelper.Lerp(0.2f, 2f, Utils.GetLerpValue(-25, -60, skill));
        }
        else if (skill < -60)
        {
            // Sharknado
        }
        else if (skill < -25)
        {
            pool[NPCID.Shark] = MathHelper.Lerp(0.2f, 2f, Utils.GetLerpValue(-25, -60, skill));
        }
        else
        {
            pool[NPCID.CorruptGoldfish] = MathHelper.Lerp(0.2f, 2f, -skill / 25f);
        }
    }
}
