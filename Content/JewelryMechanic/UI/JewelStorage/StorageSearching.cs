using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.Items.Jewels;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PeculiarJewelry.Content.JewelryMechanic.UI.JewelStorage;

internal class StorageSearching
{
    internal static bool JewelMatches(string searchTerm, JewelInfo info, Jewel associatedJewel)
    {
        string[] terms = searchTerm.Trim().Split(' ');

        if (terms.Length == 0)
            return true;

        foreach (string term in terms)
        {
            if (term == string.Empty)
                continue;

            if (!term.Contains(':'))
            {
                if (associatedJewel is not null && !MatchName(term, associatedJewel))
                    return false;
            }
            else
            {
                string[] split = term.Split(':');

                if (split.Length is <= 1 or > 2)
                {
                    if (associatedJewel is not null && !MatchName(term, associatedJewel))
                        return false;
                }
                else
                {
                    split[0] = split[0].ToLower();
                    split[1] = split[1].ToLower();

                    switch (split[0])
                    {
                        case "major":
                            if (!JewelHasMajor(info, split[1]))
                                return false;

                            break;

                        case "minor" or "sub":
                            if (!JewelHasSub(info, split[1]))
                                return false;

                            break;

                        case "cuts" or "cut":
                            if (!int.TryParse(split[1], out int result) || info.cuts != result)
                                return false;

                            break;

                        case "successfulcuts":
                            if (!int.TryParse(split[1], out int scResult) || info.successfulCuts != scResult)
                                return false;

                            break;

                        case "tier":
                            if (!Enum.TryParse(split[1], out JewelTier tierResult) || info.tier != tierResult)
                                return false;

                            break;

                        case "type":
                            if (associatedJewel is null)
                                break;

                            if (split[1].Equals("major", StringComparison.OrdinalIgnoreCase) && associatedJewel is not MajorJewel)
                                return false;

                            if (split[1].Equals("minor", StringComparison.OrdinalIgnoreCase) && associatedJewel is not MinorJewel)
                                return false;

                            break;
                    }
                }
            }
        }

        return true;
    }

    private static bool JewelHasSub(JewelInfo info, string subName)
    {
        foreach (var sub in info.SubStats)
            if (sub.GetName().Value.Contains(subName, StringComparison.OrdinalIgnoreCase))
                return true;

        return false;
    }

    private static bool JewelHasMajor(JewelInfo info, string major) => info.Major.GetName().Value.Contains(major, StringComparison.OrdinalIgnoreCase);

    internal static bool JewelryMatches(string searchTerm, BasicJewelry jewelry)
    {
        string[] terms = searchTerm.Trim().Split(' ');
        bool hasJewelTerm = false;

        if (terms.Length == 0)
            return true;

        foreach (string term in terms)
        {
            if (term == string.Empty)
                continue;

            if (!term.Contains(':'))
            {
                if (!MatchName(term, jewelry))
                    return false;
            }
            else
            {
                string[] split = term.Split(':');

                if (split.Length is <= 1 or > 2)
                {
                    if (!MatchName(term, jewelry))
                        return false;
                }
                else
                {
                    split[0] = split[0].ToLower();
                    split[1] = split[1].ToLower();

                    if (!hasJewelTerm && split[0] is "major" or "sub" or "minor" or "tier" or "cuts" or "cut" or "successfulcuts" or "type")
                        hasJewelTerm = true;

                    switch (split[0])
                    {
                        case "material":
                            if (!jewelry.MaterialCategory.Contains(split[1], StringComparison.OrdinalIgnoreCase))
                                return false;

                            break;

                        case "jewelrytier":
                            if (Enum.TryParse(split[1], out BasicJewelry.JewelryTier tierResult) && tierResult != jewelry.tier)
                                return false;

                            break;
                    }
                }
            }
        }

        if (hasJewelTerm && !jewelry.Info.Any(x => JewelMatches(searchTerm, x, null)))
            return false;

        return true;
    }

    private static bool MatchName(string term, ModItem item)
    {
        var tooltips = new List<TooltipLine>() { new(ModLoader.GetMod("PeculiarJewelry"), "ItemName", item.DisplayName.Value) };
        item.ModifyTooltips(tooltips);
        return tooltips.First(x => x.Name == "ItemName").Text.Contains(term, StringComparison.OrdinalIgnoreCase);
    }
}
