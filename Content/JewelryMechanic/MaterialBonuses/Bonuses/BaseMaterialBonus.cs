using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal abstract class BaseMaterialBonus : ModType
{
    public static readonly Dictionary<string, BaseMaterialBonus> BonusesByKey = [];

    public abstract string MaterialKey { get; }
    public virtual int GetMajorJewelCount => 1;

    protected sealed override void Register()
    {
        ModTypeLookup<BaseMaterialBonus>.Register(this);
        BonusesByKey.Add(MaterialKey, this);
    }

    public float CountMaterial(Player player) => player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);

    /// <summary>
    /// Whether this material bonus applies to the given stat on the given player.
    /// </summary>
    /// <param name="player">The player that is wearing the jewelry with this stat.</param>
    /// <param name="type">The type of the stat.</param>
    public virtual bool AppliesToStat(Player player, StatType type) => false;

    /// <summary>
    /// Bonus to all jewel stats that succeed <see cref="AppliesToStat(Player, StatType)"/>.
    /// </summary>
    /// <param name="player">The player that has the equipped jewel for the given stat.</param>
    /// <param name="statType">The stat that is being modified.</param>
    /// <returns></returns>
    public virtual float EffectBonus(Player player, StatType statType) => 1f;

    /// <summary>
    /// Called statically per material rather than per jewel. One-time interactions can be run with <paramref name="firstSet"/>.
    /// </summary>
    /// <param name="player">The player who is being affected.</param>
    /// <param name="firstSet">If this is the first time the material bonus is set.</param>
    public virtual void StaticBonus(Player player, bool firstSet) { }

    /// <summary>
    /// Applies per jewel. Redo changes in <see cref="ResetSingleJewelBonus(Player, BasicJewelry)"/>.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="jewel"></param>
    public virtual void SingleJewelBonus(Player player, BasicJewelry jewel) { }

    /// <summary>
    /// Undoes changes per jewel. Make changes in <see cref="SingleJewelBonus(Player, BasicJewelry)"/>.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="jewel"></param>
    public virtual void ResetSingleJewelBonus(Player player, BasicJewelry jewel) { }

    public virtual float TriggerCoefficientBonus() => 1;
}
