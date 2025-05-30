using Mono.Cecil.Cil;
using MonoMod.Cil;
using PeculiarJewelry.Content.Items.JewelryItems;
using PeculiarJewelry.Content.JewelryMechanic.Stats;
using Terraria.GameContent;

namespace PeculiarJewelry.Content.JewelryMechanic.MaterialBonuses.Bonuses;

internal class PlatinumBonus : BaseMaterialBonus
{
    public override string MaterialKey => "Platinum";

    private float bonus = 1f;

    public override void Load()
    {
        On_Player.HasUnityPotion += HijackWormholeCheck;
        On_TeleportPylonsSystem.IsPlayerNearAPylon += HijackProximityCheck;
        IL_TeleportPylonsSystem.HandleTeleportRequest += HijackPylonTeleport;
    }

    private void HijackPylonTeleport(ILContext il)
    {
        ILCursor c = new(il);

        if (!c.TryGotoNext(MoveType.After, x => x.MatchCallvirt<Player>(nameof(Player.InInteractionRange))))
            return;

        c.Emit(OpCodes.Ldloc_0);
        c.EmitDelegate((bool vanillaValue, Player player) =>
        {
            if (CountMaterial(player) >= 5)
                return true;

            return vanillaValue;
        });
    }

    private bool HijackProximityCheck(On_TeleportPylonsSystem.orig_IsPlayerNearAPylon orig, Player player)
    {
        if (CountMaterial(player) >= 5)
            return true;

        return orig(player);
    }

    private bool HijackWormholeCheck(On_Player.orig_HasUnityPotion orig, Player self)
    {
        if (CountMaterial(self) >= 5)
            return true;

        return orig(self);
    }

    public override bool AppliesToStat(Player player, StatType type) =>
        type == StatType.Diligence || type == StatType.Tolerance || type == StatType.Allure;

    public override void SingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1.25f;
    public override void ResetSingleJewelBonus(Player player, BasicJewelry jewel) => bonus = 1f;

    public override float EffectBonus(Player player, StatType statType)
    {
        float count = player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);
        return count >= 1 ? bonus : 1f;
    }

    public override void StaticBonus(Player player, bool firstSet)
    {
        float count = player.GetModPlayer<MaterialPlayer>().MaterialCount(MaterialKey);

        if (count >= 3)
        {
            player.tileSpeed *= 2;
            player.sonarPotion = true;
        }

        if (count >= 5)
            player.cratePotion = true;
    }
}
