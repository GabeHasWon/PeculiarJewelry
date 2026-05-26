using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

internal class SpectroliteHeartSlime : SpectroliteProjectile
{
    public override StatType StatType => StatType.SpectroliteSlime;

    public override float MaxTimer => 50 * 60;

    private Vector2 AttachPosition
    {
        get => new(Projectile.localAI[0], Projectile.localAI[1]);
        set => (Projectile.localAI[0], Projectile.localAI[1]) = (value.X, value.Y);
    }

    internal ref float Health => ref Projectile.localAI[2];

    public override void SetStaticDefaults() => Main.projFrames[Type] = 4;

    public override void SetDefaults()
    {
        Projectile.timeLeft = 2;
        Projectile.friendly = true;
        Projectile.Size = new Vector2(24, 16);
        Projectile.tileCollide = false;
        Projectile.hide = true;
    }

    public override void DrawBehind(int index, List<int> bT, List<int> bN, List<int> bP, List<int> overPlayers, List<int> overWiresUI) => overPlayers.Add(index);
    public override bool? CanDamage() => false;

    public override void AI()
    {
        Projectile.timeLeft++;
        Projectile.frameCounter++;
        Projectile.Opacity = Math.Min(Timer / MaxTimer, 1) * 0.5f + 0.5f;
        Projectile.direction = Projectile.spriteDirection = MathF.Sign(Owner.Center.X - Projectile.Center.X);

        if (IsBig == 1)
            Projectile.frame = 2 + (int)(Projectile.frameCounter / 14f % 2);
        else
            Projectile.frame = (int)(Projectile.frameCounter / 14f % 2);

        Timer++;

        if (Timer > MaxTimer)
        {
            if (AttachPosition == Vector2.Zero || Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Y))
                AttachPosition = Main.rand.NextVector2FromRectangle(Owner.Hitbox with { X = 0, Y = 0 });

            float adjTimer = Timer - MaxTimer;
            Projectile.Center = Vector2.Lerp(Projectile.Center, Owner.Center + AttachPosition - new Vector2(16, 32), MathF.Min(adjTimer / 60f, 0.5f));
        }
        else
        {
            AttachPosition = Vector2.Zero;
            Projectile.Center = Vector2.Lerp(Projectile.Center, Owner.Center - new Vector2(0, 50), 0.06f);
            Health = (IsBig == 1 ? 120 : 60) * Strength;
        }
    }
}
