namespace PeculiarJewelry.Content.Items.JewelryItems.Rings;

[AutoloadEquip(EquipType.HandsOn)]
public class ChlorophyteRing : BaseRing
{
    public override string MaterialCategory => "Chlorophyte";
    protected override int Material => ItemID.ChlorophyteBar;
    protected override bool Hardmode => true;
}