using PeculiarJewelry.Content.Items.Jewels.Rares.Spectrolite;
using PeculiarJewelry.Content.JewelryMechanic.Misc;
using PeculiarJewelry.Content.JewelryMechanic.Misc.SpectroliteProjectiles;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Effects.Custom.SpectroliteStats;

#nullable enable

internal class CactusStat : SpectroliteStatEffect
{
    private class CactusFunctionalityPlayer : ModPlayer
    {
        public bool Protected = false;
        public NPC? Target = null;

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            Protected = false;
            Target = null;

            if (modifiers.DamageSource.SourceNPCIndex == -1)
                return;

            Target = Main.npc[modifiers.DamageSource.SourceNPCIndex];

            foreach (Projectile projectile in Main.ActiveProjectiles)
            {
                if (projectile.owner != Player.whoAmI || projectile.ModProjectile is not SpectroliteCactus cactus || cactus.UsedTimer > 0)
                    continue;

                cactus.UsedTimer = (int)(50 * 60 / cactus.Strength);
                Protected = true;
                break;
            }

            if (Protected)
                modifiers.FinalDamage -= 0.15f;
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (Protected)
            {
                int damage = (int)(info.Damage / 0.85 * 0.15f);
                Player.Heal(damage);

                Target!.SimpleStrikeNPC(damage + Target.defense, 0, false, 0);
            }
        }
    }

    public override StatType Type => StatType.SpectroliteCactus;
    public override Color Color => new(73, 120, 17);

    public override void Apply(Player player, float strength)
    {
        player.GetModPlayer<SpectrolitePlayer>().Stats[Type] += InternalEffectBonus(strength, player);

        if (player.ownedProjectileCounts[ModContent.ProjectileType<SpectroliteCactus>()] < player.GetModPlayer<SpectrolitePlayer>().Stats[Type])
        {
            Vector2 position = player.Center - new Vector2(0, 60);
            Projectile.NewProjectile(player.GetSource_FromThis(), position, Vector2.Zero, ModContent.ProjectileType<SpectroliteCactus>(), 0, 0, player.whoAmI, 0, strength);
        }
    }

    protected override float InternalEffectBonus(float multiplier, Player player) => 1f;
}
