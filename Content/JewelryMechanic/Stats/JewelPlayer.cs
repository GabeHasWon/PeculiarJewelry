using Mono.Cecil.Cil;
using MonoMod.Cil;
using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses;
using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;
using PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers;
using PeculiarJewelry.Content.JewelryMechanic.Syncing;
using System.Collections.Generic;
using Terraria;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats;

/// <summary>
/// Contains logic for jewelry functionality alongside logic for trigger effects being run.
/// </summary>
internal class JewelPlayer : ModPlayer
{
    public List<MajorJewelInfo> TriggerJewelInfos
    {
        get
        {
            List<MajorJewelInfo> infos = [];

            foreach (var item in jewelry)
                foreach (var info in item.Info)
                    if (info is MajorJewelInfo)
                        infos.Add(info as MajorJewelInfo);

            return infos;
        }
    }

    public List<BasicJewelry> jewelry = new();

    internal int timeSinceLastHit = 0;
    internal int landCooldown = 0;

    private bool _jumpFlag = false;

    public override void Load()
    {
        On_Player.DryCollision += CheckLanding;
        IL_Player.ApplyLifeAndOrMana += AddHealHook;
    }

    private void CheckLanding(On_Player.orig_DryCollision orig, Player self, bool fallThrough, bool ignorePlats)
    {
        float velY = self.velocity.Y;

        orig(self, fallThrough, ignorePlats);

        if (self.GetModPlayer<JewelPlayer>().landCooldown <= 0 && velY != self.velocity.Y && self.velocity.Y == 0 && velY > 0.6f)
        {
            foreach (var item in self.GetModPlayer<JewelPlayer>().TriggerJewelInfos)
                item.InstantTrigger(TriggerContext.OnLand, self);

            self.GetModPlayer<JewelPlayer>().landCooldown = 60;

            if (Main.netMode != NetmodeID.SinglePlayer && Main.myPlayer == self.whoAmI)
                new SyncLandModule(Main.myPlayer).Send();
        }
    }

    private void AddHealHook(ILContext il)
    {
        ILCursor c = new(il);
        
        c.GotoNext(x => x.MatchRet());
        c.GotoPrev(x => x.MatchCall<Player>(nameof(Player.SetImmuneTimeForAllTypes)));
        c.GotoNext(MoveType.After, x => x.MatchStfld<Player>(nameof(Player.statMana)));

        c.Emit(OpCodes.Ldarg_0);
        c.Emit(OpCodes.Ldloc_0);
        c.EmitDelegate(OnHeal);
    }

    public static void OnHeal(Player player, int healAmount)
    {
        if (healAmount <= 0)
            return;

        foreach (var item in player.GetModPlayer<JewelPlayer>().TriggerJewelInfos)
            item.InstantTrigger(TriggerContext.OnHeal, player);
    }

    public override void ResetEffects()
    {
        jewelry.Clear();
        timeSinceLastHit++;
    }

    public override void PostUpdateEquips()
    {
        landCooldown--;

        foreach (var item in jewelry)
            item.ApplyConstantTrigger(Player);

        Player.GetModPlayer<MaterialPlayer>().StaticMaterialEffects();
        var hallowedPlayer = Player.GetModPlayer<HallowedBonus.HallowedBonusPlayer>();
        hallowedPlayer.fiveSetPower = 1;

        if (hallowedPlayer.fiveSet)
        {
            foreach (var item in jewelry)
                foreach (var info in item.Info)
                    hallowedPlayer.fiveSetPower += info is MajorJewelInfo ? 0.01f : 0.005f;
        }

        foreach (var item in jewelry)
        {
            item.ApplySingleJewelBonus(Player);
            item.ApplyTo(Player, 0, hallowedPlayer.fiveSetPower * item.PerfectionMultiplier());
            item.ResetSingleJewelBonus(Player);
        }

        if (Player.jump > 0)
        {
            if (!_jumpFlag)
                foreach (var item in TriggerJewelInfos)
                    item.InstantTrigger(TriggerContext.OnJump, Player);

            _jumpFlag = true;
        }
        else
            _jumpFlag = false;
    }

    public override void OnHurt(Player.HurtInfo info)
    {
        foreach (var item in TriggerJewelInfos)
            item.InstantTrigger(TriggerContext.OnTakeDamage, Player);

        timeSinceLastHit = 0;
    }

    private void TriggerOnHit(NPC npc)
    {
        if (npc.immortal)
            return;

        foreach (var item in TriggerJewelInfos)
            item.InstantTrigger(TriggerContext.OnHitEnemy, Player);
    }

    public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) => TriggerOnHit(target);

    public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (proj.GetGlobalProjectile<TriggerGlobalProjectile>().FromTrigger)
            return;

        TriggerOnHit(target);
    }

    private class JewelPlayerItem : GlobalItem
    {
        public override void OnConsumeMana(Item item, Player player, int manaConsumed)
        {
            foreach (var trigger in player.GetModPlayer<JewelPlayer>().TriggerJewelInfos)
                trigger.InstantTrigger(TriggerContext.OnUseMana, player);

            if (player.statMana - manaConsumed <= 0)
                foreach (var trigger in player.GetModPlayer<JewelPlayer>().TriggerJewelInfos)
                    trigger.InstantTrigger(TriggerContext.OnRunOutOfMana, player);
        }
    }
}
