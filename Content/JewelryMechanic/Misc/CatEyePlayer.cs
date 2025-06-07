using System;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc;

/// <summary>
/// Used to stack 1-set bonuses. Use <see cref="GetBonus(string, float)"/> to add stacks to a given value.
/// </summary>
internal class CatEyePlayer : ModPlayer
{
    public Dictionary<string, float> StackBonusesByMaterial = [];

    public override void ResetEffects()
    {
        foreach (string key in StackBonusesByMaterial.Keys)
            StackBonusesByMaterial[key] = 0f;
    }

    public int Repeats(string material)
    {
        if (!StackBonusesByMaterial.TryGetValue(material, out float value))
            value = 0;

        return (int)Math.Floor(value);
    }
    /// <summary>
    /// Accumulates the stacked 1-set bonuses for a given material. Returns minimum <paramref name="baseBonus"/>, 
    /// maximum <c><paramref name="baseBonus"/> + <paramref name="baseBonus"/></c> repeated stack times.
    /// </summary>
    /// <param name="material"></param>
    /// <param name="baseBonus"></param>
    /// <returns></returns>
    public float GetBonus(string material, float baseBonus)
    {
        float final = baseBonus;

        for (int i = 0; i < Repeats(material); i++)
            final += baseBonus;

        return final;
    }
}
