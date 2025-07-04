using System;

namespace PeculiarJewelry.Content.NPCs;

internal class Earthfish : ModNPC
{
    public virtual float MaxSpeed => 7f;
    public virtual float Acceleration => 1f;

    public ref float Speed => ref NPC.ai[0];

    public override void SetStaticDefaults() => Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Goldfish];

    public override void SetDefaults()
    {
        NPC.aiStyle = -1;
        NPC.hide = true;
        NPC.lifeMax = 120;
        NPC.damage = 30;
        NPC.noTileCollide = true;
        NPC.noGravity = true;
        NPC.Size = new(38, 28);
        NPC.knockBackResist = 0.5f;
    }

    public override void DrawBehind(int index) => Main.instance.DrawCacheNPCsMoonMoon.Add(index);

    public override void AI()
    {
        Player target = Main.player[NPC.target];

        if (Speed == 0)
        {
            Speed = Main.rand.NextFloat(0.9f, 1.2f);
            NPC.netUpdate = true;
        }

        NPC.TargetClosest();
        NPC.velocity += NPC.DirectionTo(target.Center) * 0.2f * Speed * Acceleration;
        NPC.direction = NPC.spriteDirection = Math.Sign(target.Center.X - NPC.Center.X);
        NPC.rotation = NPC.velocity.X * 0.02f;

        if (NPC.velocity.LengthSquared() > MaxSpeed * MaxSpeed)
            NPC.velocity = Vector2.Normalize(NPC.velocity) * MaxSpeed;
    }

    public override void FindFrame(int frameHeight)
    {
        NPC.frameCounter++;
        NPC.frame.Y = (int)(NPC.frameCounter / 7f) % 4 * frameHeight;
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        if (Main.dedServ)
            return;

        for (int i = 0; i < 5; ++i)
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);

        if (NPC.life <= 0)
        {
            for (int i = 0; i < 3; ++i)
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity * Main.rand.NextFloat(0.5f, 0.9f), Mod.Find<ModGore>(GetType().Name + "_" + i).Type);

            for (int i = 0; i < 15; ++i)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
        }
    }
}
