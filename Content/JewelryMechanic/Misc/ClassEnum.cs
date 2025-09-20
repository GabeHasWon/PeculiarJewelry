using System;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc;

public enum ClassEnum : sbyte
{
    Invalid = -1,
    Melee,
    Ranged,
    Magic,
    Summoner,
    Generic
}

public static class ClassEnumExtensions
{
    public static ref StatModifier GetDamageClass(this ClassEnum type, Player player) => ref player.GetDamage(type switch
    {
        ClassEnum.Melee => DamageClass.Melee,
        ClassEnum.Ranged => DamageClass.Ranged,
        ClassEnum.Magic => DamageClass.Magic,
        ClassEnum.Summoner => DamageClass.Summon,
        ClassEnum.Generic => DamageClass.Generic,
        _ => throw null,
    });

    public static Color GetColor(this ClassEnum classType, float adjustment) => classType switch
    {
        ClassEnum.Melee => Color.Lerp(Color.Red, Color.IndianRed, adjustment),
        ClassEnum.Ranged => Color.Lerp(Color.CadetBlue, Color.DarkSlateBlue, adjustment),
        ClassEnum.Magic => Color.Lerp(Color.Yellow, Color.DarkOrange, adjustment),
        ClassEnum.Summoner => Color.Lerp(Color.Green, Color.DarkOliveGreen, adjustment),
        ClassEnum.Generic => Color.Lerp(Color.White, Color.DarkSlateGray, adjustment),
        _ => throw new ArgumentException("ClassType exceeds the options for some reason. (MajorSoulstoneInfo OverrideDisplayColor)")
    };
}