sampler canvas : register(s0);

texture target;
sampler2D targetView = sampler_state { Texture = <target>; };

float4 Fade(float2 uv : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(canvas, uv);
    return float4(color.r, color.g, color.b, color.a * alpha);
}

technique Technique1
{
    pass ReplacePass
    {
        PixelShader = compile ps_3_0 Fade();
    }
}