sampler uImage0 : register(s0);

texture palette;

sampler2D paletteSampler = sampler_state
{
    Filter = MIN_MAG_MIP_LINEAR;
    Texture = <palette>;
    AddressU = wrap;
    AddressV = wrap;
};

float seed;

float4 Main(float2 coords : TEXCOORD0, float4 originalColor : COLOR0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float4 rainbow = tex2D(paletteSampler, coords * 2 + float2(seed, seed * 3.14));
    return color * rainbow * originalColor.a;
}

technique BasicColorDrawing
{
    pass WhiteSprite
    {
        PixelShader = compile ps_2_0 Main();
    }
};