using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses;

internal class MaterialPlayer : ModPlayer
{
    internal readonly struct EquipLayerInfo(Color color, Texture2D texture)
    {
        public readonly Color Color = color;
        public readonly Texture2D Texture = texture;
    }

    internal readonly Dictionary<EquipType, EquipLayerInfo?> jewelryEquips = new();

    private Dictionary<string, int> _materialsWornCount = null;

    public override void ResetEffects()
    {
        _materialsWornCount = new Dictionary<string, int>();

        foreach (var mat in BaseMaterialBonus.BonusesByKey)
            _materialsWornCount.Add(mat.Key, 0);

        foreach (var item in _materialsWornCount.Keys)
            _materialsWornCount[item] = 0;

        foreach (var item in jewelryEquips.Keys)
            jewelryEquips[item] = null;
    }

    public void AddMaterial(string name) => _materialsWornCount[name]++;
    internal int MaterialCount(string materialKey) => _materialsWornCount[materialKey];

    internal void SetEquip(EquipType type, EquipLayerInfo info)
    {
        if (!jewelryEquips.ContainsKey(type))
            jewelryEquips.Add(type, info);
        else
            jewelryEquips[type] = info;
    }

    internal bool HasEquip(EquipType type, out EquipLayerInfo info)
    {
        info = default;

        if (jewelryEquips.ContainsKey(type) && jewelryEquips[type] is not null)
        {
            info = jewelryEquips[type].Value;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Applies all static effects as many times as the player is wearing them.
    /// </summary>
    internal void StaticMaterialEffects()
    {
        foreach (var mat in _materialsWornCount.Where(x => x.Value > 0))
            for (int i = 0; i < _materialsWornCount[mat.Key]; ++i)
                BaseMaterialBonus.BonusesByKey[mat.Key].StaticBonus(Player, i == 0);
    }

    internal float CompoundCoefficientTriggerBonuses()
    {
        float bonus = 1f;

        foreach (var mat in _materialsWornCount.Where(x => x.Value > 0))
            for (int i = 0; i < _materialsWornCount[mat.Key]; ++i)
                bonus *= BaseMaterialBonus.BonusesByKey[mat.Key].TriggerCoefficientBonus();

        return bonus;
    }
}
