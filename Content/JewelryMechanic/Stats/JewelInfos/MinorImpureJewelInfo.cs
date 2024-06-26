﻿using System.Collections.Generic;
using System.Numerics;
using tModPorter;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class MinorImpureJewelInfo : JewelInfo
{
    public override string Prefix => "Minor";
    public override string JewelTitle => "Impure";
    public override int MaxCuts => 10 + (int)((int)tier / 2f);
    public override bool HasExclusivity => false;

    internal override void InternalSetup() => SubStats = new List<JewelStat>(2);

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
