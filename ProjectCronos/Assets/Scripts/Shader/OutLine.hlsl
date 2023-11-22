

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/NormalBuffer.hlsl"

struct Attributes
{
    uint vertexID : SV_VertexID;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS : SV_POSITION;
    float2 texcoord   : TEXCOORD0;
    UNITY_VERTEX_OUTPUT_STEREO
};

Varyings Vertex(Attributes input)
{
    Varyings output;
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
    output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
    output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
    return output;
}

TEXTURE2D_X(_InputTexture);

float4 _EdgeColor;
float2 _EdgeThresholds;
float _FillOpacity;

float4 _ColorKey0;

TEXTURE2D(_DitherTexture);
float _DitherStrength;

float3 LoadWorldNormal(uint2 positionSS)
{
    NormalData data;
    DecodeFromNormalBuffer(positionSS, data);
    return data.normalWS;
}

float4 Fragment(Varyings input) : SV_Target
{

            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        uint2 positionSS = input.texcoord * _ScreenSize.xy;
        float3 outColor = LOAD_TEXTURE2D_X(_InputTexture, positionSS).xyz;

        // ブレンディングはハードウェアブレンドに頼らず、手動で行う
        // これはカラーバッファに以前のポストプロセスエフェクトからの廃物が含まれているため、必要である。
        return float4(lerp(outColor, Luminance(outColor).xxx, 1), 1);

    // UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    // uint2 positionSS = input.texcoord * _ScreenSize.xy;

    // // Source color
    // float4 c0 = LOAD_TEXTURE2D_X(_InputTexture, positionSS);

    // // Four sample points of the roberts cross operator
    // // TL / BR / TR / BL
    // uint2 uv0 = positionSS;
    // uint2 uv1 = min(positionSS + uint2(1, 1), _ScreenSize.xy - 1);
    // uint2 uv2 = uint2(uv1.x, uv0.y);
    // uint2 uv3 = uint2(uv0.x, uv1.y);

    // // Color samples
    // float3 c1 = LOAD_TEXTURE2D_X(_InputTexture, uv1).rgb;
    // float3 c2 = LOAD_TEXTURE2D_X(_InputTexture, uv2).rgb;
    // float3 c3 = LOAD_TEXTURE2D_X(_InputTexture, uv3).rgb;

    // // Roberts cross operator
    // float3 g1 = c1 - c0.rgb;
    // float3 g2 = c3 - c2;
    // float g = sqrt(dot(g1, g1) + dot(g2, g2)) * 10;

    // // Dithering
    // uint tw, th;
    // _DitherTexture.GetDimensions(tw, th);
    // float dither = LOAD_TEXTURE2D(_DitherTexture, positionSS % uint2(tw, th)).x;
    // dither = (dither - 0.5) * _DitherStrength;

    // // Apply fill gradient.
    // float3 fill = _ColorKey0.rgb;
    // float lum = Luminance(c0.rgb) + dither;

    // float edge = smoothstep(_EdgeThresholds.x, _EdgeThresholds.y, g);
    // float3 cb = lerp(c0.rgb, fill, _FillOpacity);
    // float3 co = lerp(cb, _EdgeColor.rgb, edge * _EdgeColor.a);
    // return float4(co, c0.a);
}