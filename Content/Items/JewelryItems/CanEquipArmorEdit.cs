using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.UI;

namespace PeculiarJewelry.Content.Items.JewelryItems;

internal class CanEquipArmorEdit : ModSystem
{
    public override void Load()
    {
        IL_ItemSlot.LeftClick_ItemArray_int_int += AddCanEquipArmor;
    }

    private void AddCanEquipArmor(ILContext il)
    {
        ILCursor c = new(il);

        for (int i = 0; i < 2; ++i) 
        {
            if (!c.TryGotoNext(x => x.MatchCall(typeof(ItemLoader).GetMethod(nameof(ItemLoader.CanEquipAccessory)))))
                return;
        }

        ILLabel label = null;

        if (!c.TryGotoNext(x => x.MatchBrfalse(out label)))
        {
            return;
        }

        if (!c.TryGotoNext(MoveType.After, x => x.MatchLdelema<Item>()))
        {
            return;
        }

        c.Emit(OpCodes.Ldarg_1);
        c.EmitDelegate(CanEquipItem);

        c.Emit(OpCodes.Brfalse, label);

        c.Emit(OpCodes.Ldarg_0);
        c.Emit(OpCodes.Ldarg_2);
        c.EmitLdelema(typeof(Item));
    }

    private static bool CanEquipItem(ref Item item, int context)//, int slot)
    {
        return context != ItemSlot.Context.EquipArmor || item.ModItem is not BasicJewelry jewel || jewel.CanEquipAccessory(Main.LocalPlayer, -1, false);
    }
}
