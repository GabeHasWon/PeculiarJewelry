using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;
using System.Linq;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Utilities;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class GoldBonus : BaseMaterialBonus
{
    private static SoundStyle Sound => SoundID.Tink;

    public override string MaterialKey => "Gold";

    public override float EffectBonus(Player player, StatType statType)
    {
        float count = player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);
        return count >= 1 ? 1.15f : 1f;
    }

    public override void StaticBonus(Player player, bool firstSet)
    {
        player.luck += 0.5f;

        float count = player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);

        if (count >= 3)
            player.GetModPlayer<GoldBonusPlayer>().threeSet = true;

        if (count >= 5)
            player.GetModPlayer<GoldBonusPlayer>().fiveSet = true;
    }

    class GoldBonusPlayer : ModPlayer
    {
        private static readonly int[] Woods = new int[] { ItemID.Wood, ItemID.AshWood, ItemID.BorealWood, ItemID.RichMahogany, ItemID.BorealWood, ItemID.Ebonwood, 
            ItemID.PalmWood, ItemID.Shadewood };

        private static readonly int[] Ore = new int[] { ItemID.GoldOre, ItemID.CopperOre, ItemID.TinOre, ItemID.IronOre, ItemID.LeadOre, ItemID.SilverOre,
            ItemID.TungstenOre, ItemID.PlatinumOre };

        private static bool SkipCheck = false;

        internal bool threeSet = false;
        internal bool fiveSet = false;

        public override void Load()
        {
            On_Recipe.Create += HijackCreate;
            On_NPC.GetWereThereAnyInteractions += HijackAnyInteractions;
            On_Player.PayCurrency += HijackShopPricePayment;
            On_NPC.NPCLoot_DropItems += RemoveDropsIfUnlucky;
        }

        private void RemoveDropsIfUnlucky(On_NPC.orig_NPCLoot_DropItems orig, NPC self, Player closestPlayer)
        {
            if (closestPlayer.GetModPlayer<GoldBonusPlayer>().fiveSet && Main.rand.NextFloat() < closestPlayer.luck * 0.03f)
                return;

            orig(self, closestPlayer);
        }

        private bool HijackShopPricePayment(On_Player.orig_PayCurrency orig, Player self, long price, int customCurrency)
        {
            if (!self.GetModPlayer<GoldBonusPlayer>().fiveSet)
                return orig(self, price, customCurrency);

            if (self.luck < 0 && Main.rand.NextFloat() < Math.Abs(self.luck) * 0.03f)
                price *= 2;
            else if (Main.rand.NextFloat() < self.luck * 0.03f)
            {
                price = 0;
                SoundEngine.PlaySound(Sound, self.Center);
            }

            return orig(self, price, customCurrency);
        }

        private void HijackCreate(On_Recipe.orig_Create orig, Recipe self)
        {
            if (!Main.LocalPlayer.GetModPlayer<GoldBonusPlayer>().fiveSet)
            {
                orig(self);
                return;
            }

            if (Main.LocalPlayer.luck < 0 && Main.rand.NextFloat() < -Main.LocalPlayer.luck * 0.03f)
            {
                orig(self); // Consume double material
                orig(self);
                return;
            }

            if (Main.rand.NextFloat() < Main.LocalPlayer.luck * 0.03f)
            {
                SoundEngine.PlaySound(Sound, Main.LocalPlayer.Center);
                return;
            }

            orig(self);
        }

        public override void ResetEffects() => threeSet = fiveSet = false;

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!fiveSet)
                return;

            if (Player.luck < 0 && Main.rand.NextFloat() < Player.luck * 0.03f)
            {
                modifiers.FinalDamage *= 0;
                return;
            }

            if (Main.rand.NextFloat() < Player.luck * 0.03f)
            {
                modifiers.FinalDamage *= 2;
                SoundEngine.PlaySound(Sound, Main.LocalPlayer.Center);
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (threeSet)
            {
                if (item.pick > 0 && Main.rand.NextBool(3))
                    Item.NewItem(Player.GetSource_OnHit(target), target.Hitbox, Main.rand.Next(Ore));

                if (item.axe > 0 && Main.rand.NextBool(3))
                    Item.NewItem(Player.GetSource_OnHit(target), target.Hitbox, Main.rand.Next(Woods));
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (fiveSet && Player.luck * 0.03f > Main.rand.NextFloat())
                TryDoubleLoot(target);

            if (threeSet)
            {
                if (!Main.rand.NextBool(10) || target.life > 0)
                    return;

                 WeightedRandom<int> type = new();

                if (Player.ZoneJungle)
                    type.Add(ItemID.Moonglow);

                if (Player.ZoneCorrupt || Player.ZoneCrimson)
                    type.Add(ItemID.Deathweed);

                if (Player.ZoneDesert)
                    type.Add(ItemID.Waterleaf);

                if (Player.ZoneRockLayerHeight)
                    type.Add(ItemID.Blinkroot);

                if (Player.ZoneUnderworldHeight)
                    type.Add(ItemID.Fireblossom);

                if (Player.ZoneSnow)
                    type.Add(ItemID.Shiverthorn);

                if (type.elements.Any())
                    Item.NewItem(Player.GetSource_OnHit(target), target.Hitbox, type);
            }
        }

        private static void TryDoubleLoot(NPC target)
        {
            if (target.life > 0)
                return;

            SkipCheck = true;
            target.NPCLoot();
            SkipCheck = false;

            SoundEngine.PlaySound(Sound, Main.LocalPlayer.Center);
        }

        private bool HijackAnyInteractions(On_NPC.orig_GetWereThereAnyInteractions orig, NPC self)
        {
            if (SkipCheck)
                return false;

            return orig(self);
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            if (fiveSet)
            {
                bool survive = Main.rand.NextFloat() > Player.luck / 100f;

                if (survive)
                    SoundEngine.PlaySound(Sound, Main.LocalPlayer.Center);

                return survive;
            }

            return true;
        }

        public override bool FreeDodge(Player.HurtInfo info)
        {
            bool dodge = fiveSet && Main.rand.NextFloat() < Player.luck * 0.02f;

            if (dodge)
            {
                Player.AddImmuneTime(ImmunityCooldownID.General, 60);
                Player.immune = true;
                SoundEngine.PlaySound(Sound, Main.LocalPlayer.Center);
            }

            return dodge;
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (!Player.GetModPlayer<GoldBonusPlayer>().fiveSet)
                return;

            if (Player.luck < 0 && Main.rand.NextFloat() < -Player.luck * 0.02f)
                modifiers.FinalDamage *= 2;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref Player.HurtModifiers modifiers)
        {
            if (!Player.GetModPlayer<GoldBonusPlayer>().fiveSet)
                return;

            if (Player.luck < 0 && Main.rand.NextFloat() < -Player.luck * 0.02f)
                modifiers.FinalDamage *= 2;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (!Player.GetModPlayer<GoldBonusPlayer>().fiveSet)
                return;

            if (Player.luck < 0 && Main.rand.NextFloat() < -Player.luck * 0.01f)
                Player.KillMe(hurtInfo.DamageSource, hurtInfo.Damage, hurtInfo.HitDirection, false);
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (!Player.GetModPlayer<GoldBonusPlayer>().fiveSet)
                return;

            if (Player.luck < 0 && Main.rand.NextFloat() < -Player.luck * 0.01f)
                Player.KillMe(hurtInfo.DamageSource, hurtInfo.Damage, hurtInfo.HitDirection, false);
        }
    }
}
