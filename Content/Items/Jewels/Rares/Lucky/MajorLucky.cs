using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;
using System;

namespace PeculiarJewelry.Content.Items.Jewels.Rares.Lucky;

public class MajorLucky : Jewel
{
    public override LocalizedText ExaminationLocalization => Language.GetText("Mods.PeculiarJewelry.UI.Exam.Help.MajorLucky");
    protected override Type InfoType => typeof(MajorLuckyJewelInfo);
    protected override byte MaxVariations => 3;

    protected override GrindInfo GrindInfo => new()
    {
        DustMultiplier = 1.1f,
        ModifySupportChance = RerollFailedSupportItem,
        SubstatRatio = 0.95f,
    };

    internal static void RerollFailedSupportItem(ref float chance, ref float threshold)
    {
        if (chance < threshold)
            threshold = Main.rand.NextFloat();
    }

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
        Main.spriteBatch.Draw(texture, position, frame, col, rotation, origin, scale, SpriteEffects.None, 0);
        return false;
    }
}
