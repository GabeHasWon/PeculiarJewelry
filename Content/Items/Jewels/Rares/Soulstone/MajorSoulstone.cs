using PeculiarJewelry.Content.JewelryMechanic.Stats;
using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;
using System;

namespace PeculiarJewelry.Content.Items.Jewels.Rares.Soulstone;

public class MajorSoulstone : Jewel
{
    public override LocalizedText ExaminationLocalization => Language.GetText("Mods.PeculiarJewelry.UI.Exam.Help.MajorSoulstone");
    protected override Type InfoType => typeof(MajorSoulstoneInfo);
    protected override byte MaxVariations => 3;

    public override void Defaults()
    {
        Item.width = 42;
        Item.height = 40;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 60, 0, 0);
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

        Main.spriteBatch.Draw(texture, position, frame with { X = id * 44, Y = 168 }, col, rotation, origin, scale, SpriteEffects.None, 0);
        return false;
    }
}
