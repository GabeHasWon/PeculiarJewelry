using PeculiarJewelry.Content.Items.Jewels;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using ReLogic.Content;
using System;
using Terraria.GameContent;

namespace PeculiarJewelry.Content.Items;

public static class JewelDrawing
{
    private static readonly Vector2[] DefaultSparkLocations = [new Vector2(0.25f, 0.75f), new Vector2(0), new Vector2(1f, 0.67f), new Vector2(0.67f, 0.33f)];

    public static void DrawJewel(Jewel jewel, Asset<Texture2D> tex, Vector2 position, Vector2 origin, Color color, float rotation, float scale, 
        int frmWidth, int frmHeight, JewelInfo info, int variation, bool inInventory)
    {
        float cutAmount = MathF.Floor(info.successfulCuts / (float)info.MaxCuts / 5f * 25);

        if (cutAmount == 5)
            cutAmount--;

        var frame = new Rectangle(variation * (frmWidth + 2), (int)(frmHeight * cutAmount), frmWidth, frmHeight - 1);

        if (jewel.PreDrawJewel(tex.Value, position, frame, color, rotation, origin, scale, inInventory))
            Main.spriteBatch.Draw(tex.Value, position, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

        jewel.PostDrawJewel(position, frame, color, scale, rotation, origin, inInventory);
        DrawSparkles(position - origin * scale, new Vector2(frmWidth, frmHeight) * scale, info, color, null);
    }

    public static void DrawSparkles(Vector2 position, Vector2 size, JewelInfo info, Color color, Vector2[] locations = null)
        => DrawSparks(position, size, info.tier.DisplayValue(), color, info.cuts / (float)info.MaxCuts, locations);

    public static void DrawSparks(Vector2 position, Vector2 size, int count, Color color, float alpha, Vector2[] locations = null)
    {
        locations ??= DefaultSparkLocations;

        if (locations.Length > 4)
            throw new ArgumentException("Too many spark locations! Max is 4.");

        if (locations.Length < count)
            throw new ArgumentException("Too few spark locations! Expected " + count + ".");

        for (int i = 0; i < count; i++)
            DrawSparkle(position, size, locations[i], color, alpha, i);
    }

    private static void DrawSparkle(Vector2 position, Vector2 size, Vector2 placement, Color color, float alpha, int sparkleNumber)
    {
        color = Color.Lerp(color, Color.White, 0.33f) * alpha;
        Texture2D tex = TextureAssets.Extra[ExtrasID.SharpTears].Value;
        float time = MathF.Sin(Main.GameUpdateCount * 0.02f + (MathF.Tau / (sparkleNumber + 1)));
        Main.spriteBatch.Draw(tex, position + (placement * size), null, color, 0f, tex.Size() / 2f, new Vector2(0.5f, 0.25f) * time, SpriteEffects.None, 0);
    }
}
