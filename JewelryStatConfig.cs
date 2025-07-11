using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace PeculiarJewelry;

public class JewelryStatConfig : ModConfig
{
    private const float BaseDamageBuff = 3f;

    public override ConfigScope Mode => ConfigScope.ServerSide;

    [ReloadRequired]
    [DefaultValue(0.33f)]
    public float ChanceForMajor { get; set; }

    [DefaultValue(3)]
    [Range(0, 10)]
    public int PowerScaleStepCount { get; set; }

    [DefaultValue(0.7f)]
    [Range(0.1f, 1)]
    public float GlobalPowerScaleMinimum { get; set; }

    [Header("Mods.PeculiarJewelry.Config.StatConfigHeader")]
    [DefaultValue(3)]
    [Range(0f, 10f)]
    public float AbsolutionStrength { get; set; }

    [DefaultValue(0.06f)]
    [Range(0.01f, 0.1f)]
    public float AbundanceStrength { get; set; }

    [DefaultValue(5)]
    [Range(0f, 10f)]
    public float ArcaneStrength { get; set; }

    [DefaultValue(3)]
    [Range(0f, 10f)]
    public float CelerityStrength { get; set; }

    [DefaultValue(3)]
    [Range(0f, 10f)]
    public float DexterityStrength { get; set; }

    [DefaultValue(4)]
    [Range(0f, 10f)]
    public float ExactitudeStrength { get; set; }

    [DefaultValue(5)]
    [Range(0f, 10f)]
    public float ExploitationStrength { get; set; }

    [DefaultValue(2)]
    [Range(0f, 10f)]
    public float FrenzyStrength { get; set; }

    [DefaultValue(5f)]
    [Range(0f, 10f)]
    public float GigantismStrength { get; set; }

    [DefaultValue(BaseDamageBuff)]
    [Range(0f, 10f)]
    public float MightStrength { get; set; }

    [DefaultValue(BaseDamageBuff)]
    [Range(0f, 10f)]
    public float OrderStrength { get; set; }

    [DefaultValue(5)]
    [Range(0f, 10f)]
    public float PermenanceStrength { get; set; }

    [DefaultValue(BaseDamageBuff * 0.8f)]
    [Range(0f, 10f)]
    public float PotencyStrength { get; set; }

    [DefaultValue(BaseDamageBuff)]
    [Range(0f, 10f)]
    public float PrecisionStrength { get; set; }

    [DefaultValue(2f)]
    [Range(0f, 10f)]
    public float PreservationStrength { get; set; }

    [DefaultValue(6f)]
    [Range(0f, 10f)]
    public float RenewalStrength { get; set; }

    [DefaultValue(6f)]
    [Range(0f, 10f)]
    public float ResurgenceStrength { get; set; }

    [DefaultValue(1f)]
    [Range(0f, 10f)]
    public float TenacityStrength { get; set; }

    [DefaultValue(4f)]
    [Range(0f, 10f)]
    public float TensionStrength { get; set; }

    [DefaultValue(1f)]
    [Range(0.5f, 2f)]
    public float LegionStrength { get; set; }

    [DefaultValue(5)]
    [Range(0f, 10f)]
    public float VigorStrength { get; set; }

    [DefaultValue(BaseDamageBuff)]
    [Range(0f, 10f)]
    public float WillpowerStat { get; set; }

    [DefaultValue(1f)]
    [Range(0f, 10f)]
    public float AllureStrength { get; set; }

    [DefaultValue(1f)]
    [Range(0f, 10f)]
    public float DiligenceStrength { get; set; }

    [DefaultValue(1f)]
    [Range(0, 10f)]
    public float ToleranceStrength { get; set; }

    [Header("Mods.PeculiarJewelry.Config.RareStats")]
    [DefaultValue(1)]
    [Range(0.25f, 5f)]
    public float SoulAgonyStrength { get; set; }

    [DefaultValue(1)]
    [Range(0.25f, 5f)]
    public float SoulTortureStrength { get; set; }

    [DefaultValue(1)]
    [Range(0.25f, 5f)]
    public float SoulGriefStrength { get; set; }

    [DefaultValue(1)]
    [Range(0.25f, 5f)]
    public float SoulSacrificeStrength { get; set; }

    [DefaultValue(1)]
    [Range(0.25f, 5f)]
    public float SoulPlagueStrength { get; set; }

    [DefaultValue(1)]
    [Range(0.25f, 5f)]
    public float SoulBetrayalStrength { get; set; }

    [Header("Mods.PeculiarJewelry.Config.DesecrationHeader")]
    [DefaultValue(1)]
    [Range(0.25f, 2f)]
    public float ProfanityStrength { get; set; }
}
