using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Stats;
using System;

namespace PeculiarJewelry.Content.Items.Jewels.Rares.Amber;

public class PureAmber : Jewel
{
    public override LocalizedText ExaminationLocalization => Language.GetText("Mods.PeculiarJewelry.UI.Exam.Help.PureAmber");
    protected override Type InfoType => typeof(AmberInfo);
    protected override byte MaxVariations => 3;
    protected override bool CloneNewInstances => true;

    public override ModItem Clone(Item newEntity)
    {
        ModItem entity = base.Clone(newEntity);
        (entity as PureAmber).info.Major = info.Major;
        return entity;
    }

    public override void Defaults()
    {
        Item.width = 42;
        Item.height = 40;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 60, 0, 0);
    }

    public override void RightClick(Player player)
    {
        if (player.HeldItem.type == ItemID.None || player.HeldItem.IsAir || (info.Major as AmberStatContainer).stat.accessory.type != ItemID.None)
            return;

    }

    public override bool PreDrawJewel(Texture2D texture, Vector2 position, Rectangle frame, Color color, float rotation, Vector2 origin, float scale, bool inInventory)
    {
        var col = inInventory ? Color.White : Lighting.GetColor((position + Main.screenPosition).ToTileCoordinates());
        Main.spriteBatch.Draw(texture, position, frame, col, rotation, origin, scale, SpriteEffects.None, 0);
        return false;
    }
}
