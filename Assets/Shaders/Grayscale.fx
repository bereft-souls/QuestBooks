sampler canvas : register(s0);

float4 Desaturate(float2 uv : TEXCOORD0, float4 sampleColor : COLOR0) : COLOR0
{   
    float4 color = tex2D(canvas, uv);
    color = color * sampleColor;
    float sat = (color.r + color.g + color.b) / 3;
    return float4(sat, sat, sat, color.a);

}

technique Technique1
{
    pass ReplacePass
    {
        PixelShader = compile ps_3_0 Desaturate();
    }
}