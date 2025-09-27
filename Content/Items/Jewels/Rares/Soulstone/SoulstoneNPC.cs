using PeculiarJewelry.Content.JewelryMechanic.Misc.SoulstoneDebuffs;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.GameContent;

namespace PeculiarJewelry.Content.Items.Jewels.Rares.Soulstone;

internal class SoulstoneNPC : GlobalNPC
{
    private class SoulstoneStack
    {
        public const float MaxResetTime = 240 * 60;

        public float Factor = 0;
        public float TimeToReset = 0;

        public override string ToString() => $"{Factor} {TimeToReset}";
    }

    const float TimeMax = 10 * 60;

    public override bool InstancePerEntity => true;

    private readonly Dictionary<StatType, SoulstoneStack> _statTypeStacks = new()
    {
        { StatType.SoulAgony, new() },
        { StatType.SoulGrief, new() },
        { StatType.SoulBetrayal, new() },
        { StatType.SoulPlague, new() },
        { StatType.SoulSacrifice, new() },
        { StatType.SoulTorture, new() },
    };

    private int _timer = 0;

    public override void ResetEffects(NPC npc)
    {
        foreach (var value in _statTypeStacks.Values)
        {
            value.TimeToReset--;

            if (value.TimeToReset <= 0)
                value.Factor = 0;
        }
    }

    public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        int activeCount = 0;
        int offset = 0;

        _timer++;

        foreach (var type in _statTypeStacks.Keys)
        {
            SoulstoneStack stack = _statTypeStacks[type];

            if (stack.Factor > 0 || npc.HasBuff(GetBuffType(type)))
                activeCount++;
        }

        foreach (var type in _statTypeStacks.Keys)
        {
            SoulstoneStack stack = _statTypeStacks[type];
            bool hasBuff = npc.HasBuff(GetBuffType(type));

            if (stack.Factor == 0 && !hasBuff)
                continue;

            Texture2D tex = TextureAssets.Item[ModContent.ItemType<MinorSoulstone>()].Value;
            int frameHeight = 24;
            float factor = hasBuff ? 1 : stack.Factor;
            Rectangle src = new((type - StatType.SoulAgony) * 26, 4 * frameHeight, 24, (int)((frameHeight - 2) * factor));
            Vector2 rotationVector = new Vector2((activeCount - 1) * 20, 0).RotatedBy(offset * (MathHelper.TwoPi / activeCount) + _timer * 0.02f);
            Vector2 pos = npc.Top - Main.screenPosition - new Vector2(0, 30 - npc.gfxOffY) + rotationVector;
            float opacity = hasBuff ? npc.buffTime[npc.FindBuffIndex(GetBuffType(type))] / TimeMax : stack.TimeToReset / SoulstoneStack.MaxResetTime;
            Color color = Lighting.GetColor(npc.Center.ToTileCoordinates(), hasBuff ? Color.Red : Color.White) * opacity;
            Main.EntitySpriteDraw(tex, pos, src, color, 0f, src.Size() / 2f, 1f, SpriteEffects.None, 0);

            offset++;
        }
    }

    public static void AddStack(NPC npc, StatType type, float factorAdd)
    {
        ref float factor = ref npc.GetGlobalNPC<SoulstoneNPC>()._statTypeStacks[type].Factor;
        ref float time = ref npc.GetGlobalNPC<SoulstoneNPC>()._statTypeStacks[type].TimeToReset;
        factor += factorAdd;
        time = SoulstoneStack.MaxResetTime;

        if (factor >= 1)
        {
            npc.AddBuff(GetBuffType(type), (int)TimeMax);
            factor = 0;
        }
    }

    private static int GetBuffType(StatType type) => type switch
    {
        StatType.SoulAgony => ModContent.BuffType<AgonyDebuff>(),
        StatType.SoulTorture => ModContent.BuffType<TortureDebuff>(),
        StatType.SoulSacrifice => ModContent.BuffType<SacrificeDebuff>(),
        StatType.SoulPlague => ModContent.BuffType<PlagueDebuff>(),
        StatType.SoulGrief => ModContent.BuffType<GriefDebuff>(),
        StatType.SoulBetrayal => ModContent.BuffType<BetrayalDebuff>(),
        _ => throw new ArgumentException("StatType isn't valid for Soul Stone."),
    };
}
