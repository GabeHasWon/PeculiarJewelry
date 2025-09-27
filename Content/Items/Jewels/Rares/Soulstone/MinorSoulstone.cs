using PeculiarJewelry.Content.JewelryMechanic.Stats;
using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;
using System;

namespace PeculiarJewelry.Content.Items.Jewels.Rares.Soulstone;

public class MinorSoulstone : Jewel
{
    public override LocalizedText ExaminationLocalization => Language.GetText("Mods.PeculiarJewelry.UI.Exam.Help.MinorSoulstone");
    protected override Type InfoType => typeof(MinorSoulstoneInfo);
    protected override byte MaxVariations => 6;

    public override void Defaults()
    {
        Item.width = 24;
        Item.height = 22;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 15, 0, 0);
    }

    public override bool PreDrawJewel(Texture2D texture, Vector2 position, Rectangle frame, Color color, float rotation, Vector2 origin, float scale, bool inInventory)
    {
        var col = inInventory ? Color.White : Lighting.GetColor((position + Main.screenPosition).ToTileCoordinates());
        int id = (info as MajorSoulstoneInfo).Major.Type switch
        {
            StatType.SoulAgony => 0,
            StatType.SoulGrief => 1,
            StatType.SoulSacrifice => 2,
            StatType.SoulBetrayal => 3,
            StatType.SoulPlague => 4,
            StatType.SoulTorture => 5,
            _ => throw null,
        };

        Main.spriteBatch.Draw(texture, position, frame with { X = id * 26 }, col, rotation, origin, scale, SpriteEffects.None, 0);
        return false;
    }
}
