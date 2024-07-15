﻿using PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Stats;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;

internal class AmberInfo : JewelInfo
{
    public static JewelryStatConfig Config => ModContent.GetInstance<JewelryStatConfig>();

    public override string Prefix => "Pure";
    public override string JewelTitle => "Amber";
    public override bool HasExclusivity => false;
    public override bool CountsAsMajor => true;
    public override int MaxCuts => 0;

    public override void RollSubstats() { }

    internal override void InternalSetup()
    {
        SubStats = new List<JewelStat>(0);
        Major = new AmberStatContainer()
        {
            stat = new AmberStat()
            {
                accessory = new Item(ItemID.None),
            }
        };
    }

    internal override bool OverrideDisplayColor(out Color color)
    {
        color = new Color(217, 110, 4);
        return true;
    }
}
