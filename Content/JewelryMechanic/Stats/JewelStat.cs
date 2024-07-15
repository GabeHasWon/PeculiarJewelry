using PeculiarJewelry.Content.JewelryMechanic.Stats.Effects;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats;

public class JewelStat(StatType category)
{
    public static JewelStat Random => new((StatType)Main.rand.Next((int)StatType.BasicMax));

    public readonly StatType Type = category;

    public float Strength = category < StatType.BasicMax ? JewelryCommon.StatStrengthRange(null) : 1;

    public virtual void Apply(Player player, float add = 0, float multiplier = 0) => Get().Apply(player, (Strength + add) * multiplier);
    public float GetEffectValue(Player player, float add = 0f) => Get().GetEffectBonus(player, Strength + add);

    public virtual JewelStatEffect Get() => JewelStatEffect.StatsByType[Type];
    public LocalizedText GetName() => Get().DisplayName;

    public string GetDescription(Player player, bool showStars = true)
    {
        string stars = " ";

        if (showStars && Strength > 1)
            for (int i = 1; i < Strength - 1; ++i)
                stars += "⋆";

        return GetFinalDescriptionString(player, stars);
    }

    private string GetFinalDescriptionString(Player player, string stars) => Get().GetDescription(player, stars, Strength);
}
