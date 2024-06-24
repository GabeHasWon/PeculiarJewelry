using ReLogic.Content;

namespace PeculiarJewelry.Assets.Effects;

internal class EffectAssets : ILoadable
{
    public static Effect RainbowEffect { get; private set; }

    public void Load(Mod mod) => RainbowEffect = ModContent.Request<Effect>("PeculiarJewelry/Assets/Effects/RainbowShader", AssetRequestMode.ImmediateLoad).Value;
    public void Unload() => RainbowEffect = null;
}
