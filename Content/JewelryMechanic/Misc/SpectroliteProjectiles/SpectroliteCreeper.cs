using PeculiarJewelry.Common;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

internal class SpectroliteCreeper : SpectroliteProjectile
{
    public override float MaxTimer => 40 * 60;

    public override StatType StatType => StatType.SpectroliteCreeper;

    private ref float HitsLeft => ref Projectile.localAI[0];

    public override void SetStaticDefaults() => Main.projFrames[Type] = 2;

    public override void SetDefaults()
    {
        Projectile.timeLeft = 2;
        Projectile.friendly = true;
        Projectile.Size = new Vector2(36, 36);
        Projectile.tileCollide = false;
    }

    public override bool? CanDamage() => false;

    public override void AI()
    {
        if (IsBig == 1)
            Projectile.Size = new Vector2(20, 20);

        Timer++;
        Projectile.timeLeft = 2;
        Projectile.rotation += 0.04f;
        Projectile.Center = Owner.Center + new Vector2(70 + Owner.GetModPlayer<SpectroliteProjPlayer>().ProjCountByStatType[StatType] * 20, 0).RotatedBy(Timer * 0.02f);
        Projectile.frame = (int)IsBig;

        if (Projectile.velocity.LengthSquared() > 12 * 12)
            Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 12;

        if (Projectile.DistanceSQ(Owner.Center) > 1800 * 1800)
            Projectile.Center = Owner.Center - new Vector2(0, 120);

        if (Timer > MaxTimer)
        {
            if (Main.rand.NextBool(50))
                SpawnDust();

            foreach (Projectile projectile in Main.ActiveProjectiles)
            {
                if (CustomSets.BlockableProjectile[projectile.type] && projectile.hostile)
                {
                    projectile.Kill();

                    if (!OnHit())
                        break;
                }
            }

            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (NPCID.Sets.ProjectileNPC[npc.type] && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.StrikeInstantKill();

                    if (!OnHit())
                        break;
                }
            }
        }
        else
            HitsLeft = Math.Max(15 * Strength, 2);
    }

    private bool OnHit()
    {
        HitsLeft--;

        for (int i = 0; i < 12; ++i)
            SpawnDust(2);

        if (HitsLeft <= 0)
        {
            Timer = 0;
            return false;
        }

        return true;
    }

    private void SpawnDust(float mod = 1f)
    {
        var vel = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-6, -1));
        Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(Projectile.Hitbox), DustID.Blood, vel * mod);
    }

    public override Color? GetAlpha(Color lightColor) => Timer <= MaxTimer ? lightColor.MultiplyRGB(new Color(150, 150, 150)) : lightColor;
}
