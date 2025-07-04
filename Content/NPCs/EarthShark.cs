namespace PeculiarJewelry.Content.NPCs;

internal class EarthShark : Earthfish
{
    public override float MaxSpeed => 12;
    public override float Acceleration => 2f;

    public override void SetStaticDefaults() => Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Shark];

    public override void SetDefaults()
    {
        NPC.aiStyle = -1;
        NPC.hide = true;
        NPC.lifeMax = 800;
        NPC.damage = 70;
        NPC.noTileCollide = true;
        NPC.noGravity = true;
        NPC.Size = new(120, 46);
        NPC.knockBackResist = 0f;
    }
}
