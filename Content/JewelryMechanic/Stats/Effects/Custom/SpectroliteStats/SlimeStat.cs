using PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;
using PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SpectroliteStats;

internal class SlimeStat : SpectroliteStatEffect
{
    internal class SlimePlayer : ModPlayer
    {
        public override bool ConsumableDodge(Player.HurtInfo info) 
        {
            foreach (Projectile projectile in Main.ActiveProjectiles)
            {
                if (projectile.owner != Player.whoAmI || projectile.ModProjectile is not SpectroliteHeartSlime heart || heart.Timer < heart.MaxTimer || heart.Health < 0)
                    continue;

                bool skipDamage = false;

                if (heart.Health >= info.Damage)
                {
                    skipDamage = true;
                    heart.Health -= info.Damage;
                }
                else
                {
                    info.Damage -= (int)heart.Health;
                    heart.Health = 0;
                }

                if (heart.Health <= 0)
                    heart.Timer = 0;

                int count = heart.Health < 0 ? 14 : 4;

                for (int i = 0; i < count; ++i)
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.PinkSlime);

                if (skipDamage)
                {
                    Player.SetImmuneTimeForAllTypes(60);
                    return true;
                }
            }

            return false;
        }
    }

    public override StatType Type => StatType.SpectroliteSlime;
    public override Color Color => new(255, 132, 195);
    public override int ProjId => ModContent.ProjectileType<SpectroliteHeartSlime>();

    protected override float InternalEffectBonus(float multiplier, Player player) => 1f;
}
