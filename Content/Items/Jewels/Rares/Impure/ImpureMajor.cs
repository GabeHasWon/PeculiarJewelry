using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Graphics;
using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos;
using System;
using Terraria.GameContent;
using Snaker.Common.Effects;
using Terraria.ModLoader.IO;

namespace PeculiarJewelry.Content.Items.Jewels.Rares.Impure;

public class ImpureMajor : Jewel
{
    protected override Type InfoType => typeof(MajorImpureJewelInfo);
    protected override byte MaxVariations => 3;
    protected override bool CloneNewInstances => true;

    protected float _seed = -1;

    public override ModItem Clone(Item newEntity)
    {
        ModItem clone = base.Clone(newEntity);
        (clone as ImpureMajor)._seed = _seed;
        return clone;
    }

    public override void Defaults()
    {
        Item.width = 42;
        Item.height = 40;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 60, 0, 0);
    }

    public override void Update(ref float gravity, ref float maxFallSpeed)
    {
        if (_seed == -1)
            _seed = Main.rand.NextFloat() * 500;
    }

    public override void SaveData(TagCompound tag)
    {
        base.SaveData(tag);
        tag.Add(nameof(_seed), _seed);
    }

    public override void LoadData(TagCompound tag)
    {
        base.LoadData(tag);

        if (tag.ContainsKey(nameof(_seed)))
            _seed = tag.GetFloat(nameof(_seed));
    }

    public override bool PreDrawJewel(Texture2D texture, Vector2 position, Rectangle frame, Color color, float rotation, Vector2 origin, float scale, bool inInventory)
    {
        var col = inInventory ? Color.White : Lighting.GetColor((position + Main.screenPosition).ToTileCoordinates());
        Main.spriteBatch.Draw(texture, position, frame, col, rotation, origin, scale, SpriteEffects.None, 0);
        return false;
    }

    public override void PostDrawJewel(Vector2 position, Rectangle frame, Color color, float scale, float rotation, Vector2 origin, bool inInventory)
    {
        Main.spriteBatch.End();

        Texture2D palette = ModContent.Request<Texture2D>("PeculiarJewelry/Assets/Effects/RainbowTex").Value;
        Texture2D mask = ModContent.Request<Texture2D>(Texture + "_Mask").Value;

        EffectAssets.RainbowEffect.Parameters["seed"].SetValue(_seed);
        EffectAssets.RainbowEffect.Parameters["palette"].SetValue(palette);
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, EffectAssets.RainbowEffect, !inInventory ? Main.GameViewMatrix.ZoomMatrix : Main.UIScaleMatrix);

        Main.spriteBatch.Draw(mask, position, frame, color, rotation, origin, scale, SpriteEffects.None, 0f);
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, !inInventory ? Main.GameViewMatrix.TransformationMatrix : Main.UIScaleMatrix);
    }
}
