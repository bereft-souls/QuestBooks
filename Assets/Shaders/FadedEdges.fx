sampler canvas : register(s0);

bool FadeTop;
bool FadeBottom;
bool FadeRight;
bool FadeLeft;

float FadeDesignation;

float4 Fade(float2 uv : TEXCOORD0) : COLOR0
{
    float alpha = 1;
    float fade = 0;
    
    if (FadeLeft)
    {
        fade = FadeDesignation - uv.x;
        if (fade > 0)
            alpha = alpha * ((FadeDesignation - fade) / FadeDesignation);
    }
    
    if (FadeRight)
    {
        fade = uv.x - (1 - FadeDesignation);
        if (fade > 0)
            alpha = alpha * ((FadeDesignation - fade) / FadeDesignation);
    }
    
    if (FadeTop)
    {
        fade = FadeDesignation - uv.y;
        if (fade > 0)
            alpha = alpha * ((FadeDesignation - fade) / FadeDesignation);
    }
    
    if (FadeBottom)
    {
        fade = uv.y - (1 - FadeDesignation);
        if (fade > 0)
            alpha = alpha * ((FadeDesignation - fade) / FadeDesignation);
    }
    
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