using PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;
using PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;
using Terraria.Audio;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SpectroliteStats;

internal class BeetleStat : SpectroliteStatEffect
{
    internal class BeetleProjPlayer : ModPlayer
    {
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            foreach (Projectile projectile in Main.ActiveProjectiles)
            {
                if (projectile.owner != Player.whoAmI || projectile.ModProjectile is not SpectroliteBeetle beetle || beetle.Timer < beetle.MaxTimer)
                    continue;

                beetle.Timer = 0;
                modifiers.FinalDamage *= 0.85f;

                SoundEngine.PlaySound(SoundID.Tink with { Volume = 0.8f, PitchRange = (-0.3f, 0.5f) });
                break;
            }
        }
    }

    public override StatType Type => StatType.SpectroliteBeetle;
    public override Color Color => new(213, 150, 255);
    public override int ProjId => ModContent.ProjectileType<SpectroliteBeetle>();

    protected override float InternalEffectBonus(float multiplier, Player player) => 1f;
}
