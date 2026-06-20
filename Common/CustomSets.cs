namespace PeculiarJewelry.Common;

[ReinitializeDuringResizeArrays]
internal class CustomSets
{
    public static readonly bool[] BlockableProjectile = ItemID.Sets.Factory.CreateNamedSet(nameof(BlockableProjectile)).Description("Whether this projectile can be blocked")
        .RegisterBoolSet(ProjectileID.WoodenArrowHostile, ProjectileID.FireArrow, ProjectileID.FlamingArrow, ProjectileID.FrostArrow, ProjectileID.IceBolt, ProjectileID.BulletDeadeye,
            ProjectileID.BulletSnowman, ProjectileID.SalamanderSpit, ProjectileID.WebSpit, ProjectileID.DD2OgreSpit, ProjectileID.VortexLightning, ProjectileID.SkeletonBone,
            ProjectileID.Stinger, ProjectileID.QueenBeeStinger, ProjectileID.EyeLaser, ProjectileID.PinkLaser, ProjectileID.CursedFlameHostile, ProjectileID.DeathLaser, 
            ProjectileID.EyeFire, ProjectileID.SnowBallHostile, ProjectileID.UnholyTridentHostile, ProjectileID.FrostBlastHostile, ProjectileID.InfernoHostileBolt, 
            ProjectileID.RuneBlast, ProjectileID.PoisonDartTrap, ProjectileID.SpikyBallTrap, ProjectileID.CannonballHostile, ProjectileID.FrostBeam, ProjectileID.EyeBeam, 
            ProjectileID.Fireball, ProjectileID.RainNimbus, ProjectileID.SeedPlantera, ProjectileID.PoisonSeedPlantera, ProjectileID.ThornBall, ProjectileID.SniperBullet,
            ProjectileID.RocketSkeleton, ProjectileID.FlamingWood, ProjectileID.PineNeedleHostile, ProjectileID.OrnamentHostileShrapnel, ProjectileID.OrnamentHostile,
            ProjectileID.FrostShard, ProjectileID.FrostWave, ProjectileID.Missile, ProjectileID.Spike, ProjectileID.MartianTurretBolt, ProjectileID.BrainScramblerBolt,
            ProjectileID.RayGunnerLaser, ProjectileID.GigaZapperSpear, ProjectileID.SaucerMissile, ProjectileID.SaucerScrap, ProjectileID.SaucerLaser, ProjectileID.PhantasmalEye,
            ProjectileID.PhantasmalSphere, ProjectileID.PhantasmalBolt, ProjectileID.CultistBossFireBall, ProjectileID.CultistBossFireBallClone, ProjectileID.JavelinHostile,
            ProjectileID.Twinkle, ProjectileID.NebulaBolt, ProjectileID.NebulaSphere, ProjectileID.NebulaLaser, ProjectileID.VortexLaser, ProjectileID.VortexAcid,
            ProjectileID.AncientDoomProjectile, ProjectileID.DesertDjinnCurse, ProjectileID.DD2DrakinShot, ProjectileID.DD2LightningBugZap, ProjectileID.DD2GoblinBomb,
            ProjectileID.DD2BetsyFireball, ProjectileID.BloodShot, ProjectileID.HallowBossLastingRainbow, ProjectileID.HallowBossRainbowStreak, ProjectileID.TorchGod,
            ProjectileID.InsanityShadowHostile, ProjectileID.MiniBoulder );
}
