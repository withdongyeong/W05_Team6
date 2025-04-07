Shader "Custom/URP-Sprite-Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Thickness", Float) = 1
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100

        Pass
        {
            Name "ForwardLit"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _OutlineColor;
            float _OutlineThickness;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            float4 frag (Varyings IN) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                if (col.a < 0.1)
                {
                    float2 offsets[8] = {
                        float2(-1, -1), float2(-1, 0), float2(-1, 1),
                        float2(0, -1),              float2(0, 1),
                        float2(1, -1), float2(1, 0), float2(1, 1)
                    };

                    float outline = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        float2 uvOffset = IN.uv + offsets[i] * _OutlineThickness * 0.001;
                        float4 sample = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uvOffset);
                        outline += sample.a;
                    }

                    if (outline > 0) return _OutlineColor;
                }
                return col;
            }
            ENDHLSL
        }
    }
}
