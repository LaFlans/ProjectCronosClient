Shader "Hidden/Shader/OutLine"
{
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #define RECOLOR_EDGE_COLOR
            #include "OutLine.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}
