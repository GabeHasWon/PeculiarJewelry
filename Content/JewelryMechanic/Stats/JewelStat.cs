using PeculiarJewelry.Content.JewelryMechanic.Stats.Effects;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats;

public class JewelStat(StatType category)
{
    public static JewelStat Random => new((StatType)Main.rand.Next((int)StatType.Max));

    public readonly StatType Type = category;

    public float Strength = JewelryCommon.StatStrengthRange(null);

    public virtual void Apply(Player player, float add = 0, float multiplier = 0) => JewelStatEffect.StatsByType[Type].Apply(player, (Strength + add) * multiplier);
    public float GetEffectValue(Player player, float add = 0f) => JewelStatEffect.StatsByType[Type].GetEffectBonus(player, Strength + add);

    public JewelStatEffect Get() => JewelStatEffect.StatsByType[Type];
    public LocalizedText GetName() => JewelStatEffect.StatsByType[Type].DisplayName;

    public virtual string GetDescription(Player player, bool showStars = true)
    {
        var stat = JewelStatEffect.StatsByType[Type];
        string stars = " ";

        if (showStars)
        {
            if (Strength > 1)
            {
                for (int i = 1; i < Strength - 1; ++i)
                    stars += "⋆";
            }
        }

        return stat.Description.WithFormatArgs(stat.GetEffectBonus(player, Strength).ToString("#0.##")).Value + stars;
    }
}
