using PeculiarJewelry.Content.Items.Jewels.Rares.Pearl;
using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;
using System;

namespace PeculiarJewelry.Content.Items.Jewels.Rares.CatEye;

public class MinorCatEye : Jewel
{
    public override LocalizedText ExaminationLocalization => Language.GetText("Mods.PeculiarJewelry.UI.Exam.Help.MinorCat");
    protected override Type InfoType => typeof(MinorCatEyeInfo);
    protected override byte MaxVariations => 5;

    public sealed override void Defaults()
    {
        Item.width = 24;
        Item.height = 22;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 15, 0, 0);
    }

    public override bool PreDrawJewel(Texture2D texture, Vector2 position, Rectangle frame, Color color, float rotation, Vector2 origin, float scale, bool inInventory)
    {
        var col = inInventory ? Color.White : Lighting.GetColor((position + Main.screenPosition).ToTileCoordinates());
        Main.spriteBatch.Draw(texture, position, frame, col, rotation, origin, scale, SpriteEffects.None, 0);
        return false;
    }
}
