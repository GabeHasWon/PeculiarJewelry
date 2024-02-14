﻿using PeculiarJewelry.Content.Buffs;
using PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers;

internal abstract class TriggerEffect : ModType
{
    private static readonly Dictionary<TriggerContext, float> ConditionCoefficients = new()
    {
        { TriggerContext.OnTakeDamage, 0.5f },
        { TriggerContext.OnHeal, 5f },
        { TriggerContext.OnUseMana, 0.2f },
        { TriggerContext.OnRunOutOfMana, 1 },
        { TriggerContext.OnJump, 0.1f },
        { TriggerContext.OnHitEnemy, 0.1f },
        { TriggerContext.OnLand, 0.2f },
        { TriggerContext.WhenBelowHalfHealth, 0.8f },
        { TriggerContext.WhenAboveHalfHealth, 0.5f },
        { TriggerContext.WhenFullHealth, 1 },
        { TriggerContext.WhenBelowHalfMana, 0.4f },
        { TriggerContext.WhenAboveHalfMana, 0.25f },
        { TriggerContext.WhenFullMana, 0.5f },
        { TriggerContext.WhenHaveDebuff, 0.5f },
        { TriggerContext.WhenOver10Buffs, 0.5f },
        { TriggerContext.WhenPotionSick, 0.2f },
        { TriggerContext.WhenNoBuffs, 1 },
        { TriggerContext.WhenIdle, 1 },
        { TriggerContext.WhenNotHitFor15Seconds, 0.5f },
        { TriggerContext.WhenHitWithinPast5Seconds, 1 }
    };

    public abstract TriggerType Type { get; }
    public virtual bool NeedsCooldown => false;

    public int CooldownBuffType => !NeedsCooldown ? throw new FieldAccessException($"{GetType().Name} has no buff!")
        : ModLoader.GetMod("PeculiarJewelry").Find<ModBuff>(GetType().Name + "Buff").Type;

    public TriggerContext Context { get; protected set; }

    private int _lingerTime = 0;

    public TriggerEffect()
    {
        if (Type != TriggerType.Conditional)
            Context = (TriggerContext)Main.rand.Next((int)TriggerContext.OnTakeDamage, (int)TriggerContext.WhenBelowHalfHealth);
        else
            Context = (TriggerContext)Main.rand.Next((int)TriggerContext.WhenBelowHalfHealth, (int)TriggerContext.Max);
    }

    protected sealed override void Register()
    {
        ModTypeLookup<TriggerEffect>.Register(this);

        if (NeedsCooldown)
        {
            string key = "Mods.PeculiarJewelry.Jewelry.TriggerEffects." + GetType().Name + "Buff.";
            Mod.AddContent(new TriggerCooldownBuff(GetType().Name + "Buff", Language.GetText(key + "BuffName"), Language.GetText(key + "BuffDescription")));
        }
    }

    internal void ForceSetContext(TriggerContext context) => Context = context;

    public static int CooldownTime(JewelTier tier) => (int)Math.Pow(2, 1 - ((float)tier / 10)) * 60;

    public void InstantTrigger(TriggerContext context, Player player, JewelTier tier)
    {
        if (NeedsCooldown && player.HasBuff(CooldownBuffType))
            return;

        if (Type != TriggerType.Conditional)
        {
            if (ReportInstantChance(tier, player) > Main.rand.NextFloat())
            {
                float coefficient = ConditionCoefficients[context];
                float bonus = player.GetModPlayer<MaterialPlayer>().CompoundCoefficientTriggerBonuses();

                if (player.GetModPlayer<MaterialPlayer>().MaterialCount("Hellstone") >= 3 && Main.rand.NextBool(2)) // Chance to double up
                    InternalInstantEffect(context, player, coefficient * bonus, tier);

                InternalInstantEffect(context, player, coefficient * bonus, tier);
            }
        }
    }

    protected virtual void InternalInstantEffect(TriggerContext context, Player player, float coefficient, JewelTier tier) { }

    public void ConstantTrigger(Player player, JewelTier tier, float bonus)
    {
        //if (NeedsCooldown && player.HasBuff(CooldownBuffType))
        //    return;

        _lingerTime--;

        bool condition = ConstantConditionMet(Context, player, tier);
        int meteoriteCount = player.GetModPlayer<MaterialPlayer>().MaterialCount("Meteorite");
        bool canRun = meteoriteCount >= 3 ? (condition || Main.rand.NextBool(4)) : condition;

        if (canRun || _lingerTime > 0)
        {
            float coefficient = ConditionCoefficients[Context] + bonus;
            int hellCount = player.GetModPlayer<MaterialPlayer>().MaterialCount("Hellstone");

            if (hellCount >= 3)
                coefficient *= 1.33f;

            InternalConditionalEffect(Context, player, coefficient, tier);

            if (meteoriteCount >= 1)
                _lingerTime = 180;
        }
    }

    protected static bool ConstantConditionMet(TriggerContext context, Player player, JewelTier tier)
    {
        return context switch 
        {
            TriggerContext.WhenBelowHalfHealth => player.statLife < player.statLifeMax2 / 2,
            TriggerContext.WhenAboveHalfHealth => player.statLife >= player.statLifeMax2 / 2,
            TriggerContext.WhenFullHealth => player.statLife == player.statLifeMax2,
            TriggerContext.WhenBelowHalfMana => player.statMana < player.statManaMax2 / 2,
            TriggerContext.WhenAboveHalfMana => player.statMana >= player.statManaMax2 / 2,
            TriggerContext.WhenFullMana => player.statMana == player.statManaMax2,
            TriggerContext.WhenHaveDebuff => player.buffType.Any(x => x != 0 && Main.debuff[x]),
            TriggerContext.WhenOver10Buffs => player.buffType.Count(x => x != 0) > 10,
            TriggerContext.WhenPotionSick => player.HasBuff(BuffID.PotionSickness),
            TriggerContext.WhenNoBuffs => !player.buffType.Any(x => x != 0 && !BuffSet.TriggerBuffs.Contains(x)),
            TriggerContext.WhenIdle => player.velocity.LengthSquared() <= 0.1f,
            TriggerContext.WhenNotHitFor15Seconds => false, // TBD
            TriggerContext.WhenHitWithinPast5Seconds => false, // TBD
            _ => false,
        };
    }

    /// <summary>
    /// How strong conditionals are using the formula (T/10)*C*E*0.1.
    /// </summary>
    protected float TotalConditionalStrength(float coefficient, JewelTier tier) => coefficient * TriggerPower() * (((float)tier + 1) / 10) * 0.1f;

    protected virtual void InternalConditionalEffect(TriggerContext context, Player player, float coefficient, JewelTier tier) { }

    public virtual string Tooltip(JewelTier tier, Player player)
    {
        const string Prefix = "Mods.PeculiarJewelry.Jewelry.";
        string condition = Language.GetText(Prefix + "TriggerContexts." + Context).Value;
        string chance = Language.GetText(Prefix + "ChanceTo").WithFormatArgs((ReportInstantChance(tier, player) * 100).ToString("#0.##")).Value;
        string effect = Language.GetText(Prefix + "TriggerEffects." + GetType().Name).WithFormatArgs(TotalTriggerPower(player, ConditionCoefficients[Context])).Value;

        return condition + " " + (Type == TriggerType.Conditional ? "" : chance) + effect;
    }

    private static float ReportInstantChance(JewelTier jewelTier, Player player)
    {
        int meteoriteCount = player.GetModPlayer<MaterialPlayer>().MaterialCount("Meteorite");

        if (meteoriteCount >= 3)
            return 1f;

        int tier = (int)jewelTier;
        float chance = (tier + 1f) / (tier + 3f);
        chance += (100 - (chance * 100)) / 100 * (meteoriteCount / (meteoriteCount + 3f));

        return chance;
    }

    public virtual string TooltipArgumentFormat(float coefficient, JewelTier tier) => (TriggerPower() * coefficient).ToString("#0.##");
    public abstract float TriggerPower();

    public float TotalTriggerPower(Player player, float coefficient)
    {
        float hellstoneMultiplier = player.GetModPlayer<MaterialPlayer>().MaterialCount("Hellstone") * 0.5f;
        return coefficient * TriggerPower() * (hellstoneMultiplier + 1);
    } 
}
