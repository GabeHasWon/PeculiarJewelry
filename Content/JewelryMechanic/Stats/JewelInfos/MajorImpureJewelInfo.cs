
namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class MajorImpureJewelInfo : MajorJewelInfo
{
    public override string JewelTitle => "Impure";
    public override bool HasExclusivity => false;

    public override bool PreBuffStat(out float result)
    {
        var config = ModContent.GetInstance<JewelryStatConfig>();
        result = GetScale(Main.rand.Next(config.PowerScaleStepCount + 1) / (float)config.PowerScaleStepCount);
        return true;
    }

    private static float GetScale(float factor)
    {
        var config = ModContent.GetInstance<JewelryStatConfig>();
        float result = MathHelper.Lerp(0, 1 + config.GlobalPowerScaleMinimum, factor);
        return result + 0.1f;
    }

    public override (float, float) BuffStatRange() => (GetScale(0), GetScale(1));
}
