using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

internal class SpectroliteCat : SpectroliteProjectile
{
    private class SpectroliteCatPlayer : ModPlayer
    {
        public override void OnHurt(Player.HurtInfo info) 
        {
            foreach (Projectile projectile in Main.ActiveProjectiles)
            {
                if (projectile.active && projectile.ModProjectile is SpectroliteCat cat && cat.Timer >= cat.MaxTimer)
                    cat.Timer = 0;
            }
        }

        public override void UpdateLifeRegen()
        {
            foreach (Projectile projectile in Main.ActiveProjectiles)
            {
                if (projectile.active && projectile.ModProjectile is SpectroliteCat cat && cat.Timer >= cat.MaxTimer)
                    Player.lifeRegen += 20;
            }
        }
    }

    public override float MaxTimer => 10 * 60 / MathHelper.Lerp(Strength, 1, 0.3f);
    public ref float Timer => ref Projectile.ai[0];

    public override StatType StatType => StatType.SpectroliteKitten;

    public override void SetStaticDefaults() => Main.projFrames[Type] = 2;

    public override void SetDefaults()
    {
        Projectile.timeLeft = 2;
        Projectile.friendly = true;
        Projectile.Size = new Vector2(26, 24);
        Projectile.tileCollide = false;
    }

    public override bool? CanDamage() => false;

    public override void AI()
    {
        Timer += Timer >= MaxTimer ? 0.03f : 0.01f;
        Projectile.Center = Vector2.Lerp(Projectile.Center, Owner.Center - new Vector2(0, 60) + (new Vector2(60, 0).RotatedBy(Timer) * new Vector2(1, 0.25f)), 0.2f);
        Projectile.timeLeft = 2;
        Projectile.rotation = MathF.Sin(Timer) * 0.3f;

        if (Timer > MaxTimer)
        {
            if (Main.rand.NextBool(20))
            {
                var vel = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-6, -1));
                Gore.NewGore(Projectile.GetSource_FromThis(), Main.rand.NextVector2FromRectangle(Projectile.Hitbox), vel, 331);
            }
        }
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        if (Projectile.hide)
            overPlayers.Add(index);
    }
}
