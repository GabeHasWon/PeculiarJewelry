using System;
using Terraria.GameContent;

namespace PeculiarJewelry.Content.NPCs;

internal class Earthbubble : ModNPC
{
    private ref float Timer => ref NPC.ai[0];

    public override void SetDefaults()
    {
        NPC.Size = new(32);
        NPC.lifeMax = 3000;
        NPC.defense = 300;
        NPC.aiStyle = -1;
    }

    public override void AI()
    {
        Timer++;
        NPC.TargetClosest();
        NPC.velocity = NPC.DirectionTo(Main.player[NPC.target].Center) * MathF.Min(MathHelper.Lerp(0, 16, Timer / 200f), 16);

        if (NPC.DistanceSQ(Main.player[NPC.target].Center) < 32 * 32)
        {
            Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ProjectileID.Cthulunado, 60, 0, Main.myPlayer, 10, Main.rand.Next(20, 31));
            NPC.netUpdate = true;
            NPC.active = false;
        }
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        float xScale = 1 + MathF.Sin(Timer * 0.3f) * 0.2f;
        float yScale = 1 + MathF.Cos(Timer * 0.3f) * 0.2f;
        Texture2D tex = TextureAssets.Npc[Type].Value;
        spriteBatch.Draw(tex, NPC.Center - screenPos, null, Color.White, NPC.rotation, NPC.Size / 2f, new Vector2(xScale, yScale), SpriteEffects.None, 0);
        return false;
    }
}
