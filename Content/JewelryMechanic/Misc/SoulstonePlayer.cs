using PeculiarJewelry.Content.JewelryMechanic.Stats;
using PeculiarJewelry.Content.JewelryMechanic.Stats.JewelInfos.Rares;
using System.Collections.Generic;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc;

internal class SoulstonePlayer : ModPlayer
{
    public class ActiveSoulstone
    {
        public bool HasProjectile => ProjWho != -1;

        public int ProjWho = -1;
    }

    public class SoulstoneInfo
    {
        public float TotalStrength = 0;
        public JewelTier MaxTier = JewelTier.Natural;
        public MajorSoulstoneInfo.ClassEnum Class = MajorSoulstoneInfo.ClassEnum.Melee;
        public List<ActiveSoulstone> Soulstones = [];
        public List<ActiveSoulstone> LastFrameSoulstones = [];

        public void Reset()
        {
            TotalStrength = 0;
            MaxTier = JewelTier.Natural;
            Class = MajorSoulstoneInfo.ClassEnum.Melee;

            foreach (var soulstone in Soulstones)
            {
                Main.NewText(soulstone.GetHashCode());

                if (!soulstone.HasProjectile)
                    continue;

                Projectile projectile = Main.projectile[soulstone.ProjWho];

                if (!projectile.active || projectile.type != ModContent.ProjectileType<SoulstoneWisp>())
                {
                    soulstone.ProjWho = -1;
                }
            }

            // Clear all items that weren't re-buffed
            foreach (var item in LastFrameSoulstones)
            {
                if (!Soulstones.Contains(item))
                {
                    if (item.HasProjectile)
                    {
                        Main.projectile[item.ProjWho].active = false;
                        item.ProjWho = -1;
                    }
                }
            }

            LastFrameSoulstones.Clear();

            // Add all items that were added to LastFrame, for the above functionality
            foreach (var item in Soulstones)
                LastFrameSoulstones.Add(item);

            Soulstones.Clear();
        }

        public override string ToString() => $"STR: {TotalStrength} - INST. COUNT: {Soulstones.Count} - TIER: {MaxTier} - CLASS: {Class}";
    }

    public readonly Dictionary<StatType, SoulstoneInfo> InfoByInfo = new() { { StatType.SoulAgony, new() }, { StatType.SoulGrief, new() }, { StatType.SoulBetrayal, new() },
        { StatType.SoulPlague, new() }, { StatType.SoulTorture, new() }, { StatType.SoulSacrifice, new() } };

    public override void PostUpdateEquips()
    {
        foreach (var key in InfoByInfo.Keys)
        {
            SoulstoneInfo info = InfoByInfo[key];

            if (info.TotalStrength == 0)
                continue;

            foreach (ActiveSoulstone stone in info.Soulstones)
            {
                if (stone.HasProjectile)
                    continue;

                if (Player.whoAmI == Main.myPlayer)
                {
                    Vector2 position;

                    do
                    {
                        position = Player.Center + Main.rand.NextVector2CircularEdge(400, 400) * Main.rand.NextFloat(0.75f, 1f);
                    } while (Collision.SolidCollision(position, 32, 32));

                    int projType = ModContent.ProjectileType<SoulstoneWisp>();
                    stone.ProjWho = Projectile.NewProjectile(Player.GetSource_FromThis(), position, Vector2.Zero, projType, 0, 0, Player.whoAmI, (float)info.Class);
                }
            }
        }

        foreach (var key in InfoByInfo.Keys)
            InfoByInfo[key].Reset();
    }
}
