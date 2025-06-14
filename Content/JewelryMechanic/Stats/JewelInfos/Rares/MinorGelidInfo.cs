﻿using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;

internal class MinorGelidInfo : MajorGelidInfo
{
    public override string Prefix => "Minor";
    public override bool CountsAsMajor => false;

    internal override void InternalSetup() => SubStats = new List<JewelStat>(2);
}
