﻿namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom;

internal class AmberStat : JewelStatEffect
{
    public override StatType Type => StatType.Amber;
    public override Color Color => new(217, 110, 4);

    internal Item accessory = null;

    public AmberStat()
    {
        Description = Language.GetText("Mods.PeculiarJewelry.Jewelry.StatTypes." + Type + ".Description");
        DisplayName = Language.GetText("Mods.PeculiarJewelry.Jewelry.StatTypes." + Type + ".DisplayName");
    }

    public override void Apply(Player player, float strength)
    {
        if (accessory.type == ItemID.None)
            return;

        if (accessory.expertOnly && !Main.expertMode)
            return;

        player.ApplyEquipFunctional(accessory, false);
    }

    protected override float InternalEffectBonus(float multiplier, Player player) => 0;

    internal override string GetDescription(Player player, string stars, float str, bool negative = false) => accessory is null || accessory.IsAir
        ? Language.GetTextValue("Mods.PeculiarJewelry.AmberCanUse")
        : Description.WithFormatArgs($"{accessory.Name} [i:{accessory.type}]").Value + stars;
}
