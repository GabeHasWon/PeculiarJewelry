using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.Bestiary;
using PeculiarJewelry.Content.JewelryMechanic.UI;
using PeculiarJewelry.Content.Items.Jewels;
using rail;
using PeculiarJewelry.Content.Items.JewelryItems;

namespace PeculiarJewelry.Content.NPCs;

[AutoloadHead]
public class Lapidarist : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 26;

        NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
        NPCID.Sets.AttackFrameCount[NPC.type] = 4;
        NPCID.Sets.DangerDetectRange[NPC.type] = 1500;
        NPCID.Sets.AttackType[NPC.type] = 0;
        NPCID.Sets.AttackTime[NPC.type] = 25;
        NPCID.Sets.AttackAverageChance[NPC.type] = 30;

        NPC.Happiness
            .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Like)
            .SetBiomeAffection<HallowBiome>(AffectionLevel.Like)
            .SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike)
            .SetBiomeAffection<JungleBiome>(AffectionLevel.Hate)
            .SetNPCAffection(NPCID.Clothier, AffectionLevel.Love)
            .SetNPCAffection(NPCID.Painter, AffectionLevel.Like)
            .SetNPCAffection(NPCID.Steampunker, AffectionLevel.Like)
            .SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Dislike)
            .SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Hate);
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Guide);
        NPC.townNPC = true;
        NPC.friendly = true;
        NPC.aiStyle = 7;
        NPC.damage = 14;
        NPC.defense = 30;
        NPC.lifeMax = 250;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.knockBackResist = 0.4f;
        AnimationType = NPCID.Guide;
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
            new FlavorTextBestiaryInfoElement("Mods.PeculiarJewelry.NPCs.Lapidarist.Bestiary"),
        });
    }

    public override bool CanTownNPCSpawn(int numTownNPCs)
    {
        for (int i = 0; i < Main.maxPlayers; ++i)
        {
            Player player = Main.player[i];

            if (player.active)
            {
                for (int j = 0; j < player.inventory.Length; ++j)
                {
                    Item inv = player.inventory[j];

                    if (!inv.IsAir && (inv.ModItem is Jewel || inv.ModItem is BasicJewelry))
                        return true;
                }
            }
        }

        return false;
    }

    public override List<string> SetNPCNameList()
    {
        List<string> names = [];

        for (int i = 0; i < 10; ++i)
            names.Add(Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.Names." + i));

        return names;
    }

    public override string GetChat() => Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.Dialogue." + Main.rand.Next(5));

    public override void SetChatButtons(ref string button, ref string button2)
    {
        button = Language.GetTextValue("LegacyInterface.28");
        button2 = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.MenuName");
    }

    public override void OnChatButtonClicked(bool firstButton, ref string shopName)
    {
        if (firstButton)
            shopName = "Shop";
        else
        {
            Main.npcChatText = Language.GetTextValue("Mods.PeculiarJewelry.NPCs.Lapidarist.UIDialogue.OpenSelectionMenu");
            JewelUISystem.Instance.JewelInterface.SetState(new ChooseJewelMechanicUIState(NPC.whoAmI));
            Main.playerInventory = false;
        }
    }

    public override void TownNPCAttackStrength(ref int damage, ref float knockback)
    {
        damage = 18;
        knockback = 3f;
    }

    public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
    {
        cooldown = 5;
        randExtraCooldown = 5;
    }

    public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
    {
        projType = ProjectileID.DiamondBolt;
        attackDelay = 1;
    }

    public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
    {
        multiplier = 14f;
        randomOffset = 2f;
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        if (NPC.life > 0 || Main.netMode == NetmodeID.Server)
            return;
    }

    public override void FindFrame(int frameHeight)
    {
        if (!NPC.IsABestiaryIconDummy)
            return;

        NPC.frameCounter += 0.25f;
        if (NPC.frameCounter >= 16)
            NPC.frameCounter = 2;
        else if (NPC.frameCounter < 2)
            NPC.frameCounter = 2;

        int frame = (int)NPC.frameCounter;
        NPC.frame.Y = frame * frameHeight;
    }

    public override ITownNPCProfile TownNPCProfile() => new LapidaristProfile();
}

public class LapidaristProfile : ITownNPCProfile
{
    public int RollVariation() => 0;
    public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

    public ReLogic.Content.Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
    {
        var tex = ModContent.GetModNPC(ModContent.NPCType<Lapidarist>()).Texture;

        if (npc.altTexture == 1 && !(npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn))
            return ModContent.Request<Texture2D>(tex + "_Alt_1");

        return ModContent.Request<Texture2D>(tex);
    }

    public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot(ModContent.GetModNPC(ModContent.NPCType<Lapidarist>()).Texture + "_Head");
}