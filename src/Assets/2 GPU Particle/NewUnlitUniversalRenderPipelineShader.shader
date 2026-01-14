Shader "Custom/ParticleUnlitUniversalRenderPipelineShader"
{
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Particle
            {
                float3 Position;
                float3 Velocity;
                float3 Color;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                half3 color : COLOR0;
            };

            uniform StructuredBuffer<Particle> Particles;

           Varyings vert(uint id : SV_VertexID)
            {
                Varyings OUT;

                Particle particle = Particles[id];
                OUT.positionHCS = TransformObjectToHClip(particle.Position);

                OUT.color = particle.Color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return half4(IN.color, 1);
            }
            ENDHLSL
        }
    }
}