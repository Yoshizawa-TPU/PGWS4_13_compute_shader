Shader "Custom/NewUnlitUniversalRenderPipelineShader"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white"
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

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

            struct Particle
            {
                float3 Position;
                float3 Velocity;
                float3 Color;
            };

            struct Veryings
            {
                float4 positionHCS : SV_POSITION;
                half3 color :COLORO;
            };

            uniform StructuredBuffer<Particle> Packages;
            
            Varyings vert(uint id : SV_VertexID)
            {
                Varyings OUT;
                Particle.particle = Particles[id];
                OUT.positionHCS = TransformObjectToHClip(particle.Position);

                OUT.color = particle.Color;
                return OUT;
            }

            half4 fragg(Varyings IN) : SV_Target
            {
                return half4(IN.color,1);
                }


            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                 return half4(IN.color,1);
                // half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;
                // return color;
            }
            ENDHLSL
        }
    }
}
