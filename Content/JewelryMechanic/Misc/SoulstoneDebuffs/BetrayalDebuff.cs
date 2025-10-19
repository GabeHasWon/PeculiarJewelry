using Terraria.GameContent;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SoulstoneDebuffs;

internal class BetrayalDebuff : ModBuff
{
    public override void SetStaticDefaults() => Main.debuff[Type] = true;
}

internal class BetrayalNPC : GlobalNPC
{
    public override void OnKill(NPC npc)
    {
        if (npc.HasBuff<BetrayalDebuff>())
        {
            byte owner = Player.FindClosest(npc.position, npc.width, npc.height);
            int type = ModContent.ProjectileType<BetrayalExplosion>();
            int proj = Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, npc.velocity, type, (int)(npc.lifeMax * 0.33f), 12f, owner);

            if (Main.dedServ)
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
        }
    }
}

internal class BetrayalExplosion : ModProjectile
{
    public override void SetStaticDefaults() => Main.projFrames[Type] = 6;

    public override void SetDefaults()
    {
        Projectile.Size = new(1);
        Projectile.scale = 1f;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.timeLeft = 180;
        Projectile.tileCollide = false;
    }

    public override bool? CanCutTiles() => false;

    public override void AI()
    {
        Projectile.Resize(Projectile.width + 18, Projectile.height + 18);

        Projectile.frameCounter++;
        Projectile.frame = (int)(Projectile.frameCounter / 5f);
        Projectile.velocity *= 0.94f;

        if (Projectile.frame > 5)
        {
            Projectile.Kill();
        }
    }

    public override void OnHitNPC(NPC npc, NPC.HitInfo hit, int damageDone)
    {
        byte owner = Player.FindClosest(npc.position, npc.width, npc.height);
        Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, npc.velocity, ModContent.ProjectileType<BetrayalExplosion>(), (int)(npc.lifeMax * 0.1f), 12f, owner);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D tex = TextureAssets.Projectile[Type].Value;
        Rectangle src = new(0, Projectile.frame * 302, 300, 300);

        Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, src, lightColor, 0f, src.Size() / 2f, 1f, SpriteEffects.None, 0);
        return false;
    }
}