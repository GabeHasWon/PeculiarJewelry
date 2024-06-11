using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;

namespace PeculiarJewelry.Content.Items.Jewels;

public class MajorJewel : Jewel
{
    protected override Type InfoType => typeof(MajorJewelInfo);
    protected override byte MaxVariations => 3;

    public sealed override void Defaults()
    {
        Item.width = 42;
        Item.height = 40;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 50, 0, 0);
    }
}
