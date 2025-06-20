using PeculiarJewelry.Content.JewelryMechanic.Stats.Triggers;
using System;
using System.IO;
using Terraria.ModLoader.IO;

namespace PeculiarJewelry.Content.JewelryMechanic.Stats.IO;

internal static class JewelIO
{
    public static TagCompound SaveAs(this JewelInfo info) => SaveInfo(info);

    private static TagCompound SaveInfo(JewelInfo info)
    {
        TagCompound tag = new()
        {
            //Misc info
            { "infoType", info.GetType().AssemblyQualifiedName },
            { "infoTier", (byte)info.tier },
            { "infoExclusivity", (byte)info.exclusivity },
            { "infoCuts", (byte)info.cuts },
            { "infoSuccessfulCuts", (byte)info.successfulCuts },

            //Stats
            { "infoMajor", info.Major.SaveAs() }
        };

        info.SaveData(tag);

        for (int i = 0; i < info.SubStats.Count; i++)
            tag.Add("infoSub" + i, info.SubStats[i].SaveAs());

        return tag;
    }

    public static JewelInfo LoadInfo(TagCompound tag)
    {
        string type = tag.GetString("infoType");
        var info = Activator.CreateInstance(Type.GetType(type)) as JewelInfo;

        info.tier = (JewelTier)tag.GetByte("infoTier");
        info.exclusivity = (StatExclusivity)tag.GetByte("infoExclusivity");
        info.cuts = tag.GetByte("infoCuts");
        info.successfulCuts = tag.GetByte("infoSuccessfulCuts");
        info.SetupFromIO(LoadStat(tag.GetCompound("infoMajor")));
        info.LoadData(tag);

        for (int i = 0; i < info.SubStats.Capacity; i++)
        {
            if (tag.TryGet("infoSub" + i, out TagCompound sub))
                info.SubStats.Add(LoadStat(sub));
            else
                break;
        }

        return info;
    }

    public static TagCompound SaveAs(this JewelStat stat) => SaveStat(stat);

    private static TagCompound SaveStat(JewelStat stat)
    {
        TagCompound tag = new()
        {
            { "statType", (byte)stat.Type },
            { "statStrength", stat.Strength },
            { "negative", stat.Negative }
        };
        return tag;
    }

    public static JewelStat LoadStat(TagCompound tag)
    {
        JewelStat stat = new((StatType)tag.GetByte("statType"))
        {
            Strength = tag.GetFloat("statStrength"),
            Negative = tag.GetBool("negative")
        };

        return stat;
    }

    // Netcode

    public static void SendJewelInfo(JewelInfo info, BinaryWriter writer)
    {
        writer.Write(info.GetType().AssemblyQualifiedName);
        SendStat(info.Major, writer);
        writer.Write((byte)(info.SubStats is null ? 0 : info.SubStats.Count));

        foreach (var item in info.SubStats)
            SendStat(item, writer);

        if (info is MajorJewelInfo major)
        {
            writer.Write(major.effect.GetType().AssemblyQualifiedName);
            writer.Write((byte)major.effect.Context);
        }

        writer.Write((short)info.tier);
        writer.Write((short)info.cuts);
        writer.Write((short)info.successfulCuts);
    }

    private static void SendStat(JewelStat stat, BinaryWriter writer)
    {
        writer.Write((byte)stat.Type);
        writer.Write((Half)stat.Strength);
    }

    public static JewelInfo ReadJewelInfo(BinaryReader reader)
    {
        string type = reader.ReadString();
        JewelInfo info = Activator.CreateInstance(Type.GetType(type)) as JewelInfo;
        info.SetupFromIO(ReadStat(reader));
        int count = reader.ReadByte();

        for (int i = 0; i < count; ++i)
            info.SubStats.Add(ReadStat(reader));

        if (info is MajorJewelInfo major)
        {
            major.effect = Activator.CreateInstance(Type.GetType(reader.ReadString())) as TriggerEffect;
            major.effect.ForceSetContext((TriggerContext)reader.ReadByte());
        }

        info.tier = (JewelTier)reader.ReadInt16();
        info.cuts = reader.ReadInt16();
        info.successfulCuts = reader.ReadInt16();
        return info;
    }

    private static JewelStat ReadStat(BinaryReader reader)
    {
        JewelStat stat = new((StatType)reader.ReadByte())
        {
            Strength = (float)reader.ReadHalf()
        };
        return stat;
    }
}
