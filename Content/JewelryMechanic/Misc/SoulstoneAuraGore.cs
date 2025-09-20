using Terraria.DataStructures;
using Terraria.GameContent;

namespace PeculiarJewelry.Content.JewelryMechanic.Misc;

internal class SoulstoneAuraGore : ModGore
{
    public override string Texture => base.Texture.Replace("Gore", "");

    public override void OnSpawn(Gore gore, IEntitySource source)
    {
        gore.alpha = Main.rand.Next(60, 225);
        gore.drawOffset = -TextureAssets.Gore[Type].Value.Size() / 4f;
    }

    public override bool Update(Gore gore)
    {
        gore.position += gore.velocity;
        gore.velocity *= 0.98f;
        gore.scale -= 0.01f;

        if (gore.scale < 0.01f)
            gore.active = false;

        return false;
    }
}
