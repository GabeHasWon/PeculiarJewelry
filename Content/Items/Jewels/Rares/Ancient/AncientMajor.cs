using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;
using System;

namespace PeculiarJewelry.Content.Items.Jewels.Rares.Ancient;

public class AncientMajor : Jewel
{
    public override LocalizedText ExaminationLocalization => Language.GetText("Mods.PeculiarJewelry.UI.Exam.Help.MajorAncient");
    protected override Type InfoType => typeof(MajorAncientInfo);
    protected override byte MaxVariations => 4;

    public override void Defaults()
    {
        Item.width = 42;
        Item.height = 40;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 60, 0, 0);
    }

    public override bool PreDrawJewel(Texture2D texture, Vector2 position, Rectangle frame, Color color, float rotation, Vector2 origin, float scale, bool inInventory)
    {
        var col = inInventory ? Color.White * (color.A / 255f) : Lighting.GetColor((position + Main.screenPosition).ToTileCoordinates());
        Main.spriteBatch.Draw(texture, position, frame, col, rotation, origin, scale, SpriteEffects.None, 0);
        return false;
    }
}
