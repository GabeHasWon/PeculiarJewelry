using PeculiarJewelry.Content.JewelryMechanic.Misc;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SoulstoneStats;

internal abstract class SoulstoneStat : JewelStatEffect
{
    public override Color Color => new(93, 93, 93);

    public ClassEnum Class = ClassEnum.Generic;

    public SoulstoneStat(ClassEnum classType = ClassEnum.Invalid)
    {
        Description = Language.GetText("Mods.PeculiarJewelry.Jewelry.StatTypes." + Type + ".Description");
        DisplayName = Language.GetText("Mods.PeculiarJewelry.Jewelry.StatTypes." + Type + ".DisplayName");
        Class = classType == ClassEnum.Invalid ? (ClassEnum)Main.rand.Next(5) : classType;
    }

    public override void Apply(Player player, float strength) { }
    protected override float InternalEffectBonus(float multiplier, Player player) => multiplier;
    protected override bool SkipStatMods(Player player, float strength) => true;
    internal override string GetDescription(Player player, string stars, float str, bool negative = false) => Description.Value;
}
