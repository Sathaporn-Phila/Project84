Shader "Custom/graph"
{
    Properties
    {
        // Specular vs Metallic workflow
        [HideInInspector] _WorkflowMode("WorkflowMode", Float) = 1.0

        _NegativeColor("Color", Color) = (0.5,0.5,0.5,1)
        _PositiveColor("Color", Color) = (0.5,0.5,0.5,1)
        [MainTexture] _BaseMap("Albedo", 2D) = "white" {}
        //[Header("Resistor Color")]
        _Amplitude("Amplitude",Range(-5,5)) = 0
        _MaxAmplitude("Max Amplitude",Range(-5,5)) =5
        _Frequency("Frequency",float) = 2
        _Position("Position",Range(0,360)) = 0
    }

    SubShader
    {
        // ------------------------------------------------------------------
        // Forward pass. Shades GI, emission, fog and all lights in a single pass.
        // Compared to Builtin pipeline forward renderer, LWRP forward renderer will
        // render a scene with multiple lights with less drawcalls and less overdraw.
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline"}
        Cull Off
        Lighting Off
        
        
        
        Pass
        {
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            float _Amplitude;
            float _MaxAmplitude;
            float _Frequency;
            float _Position;
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv           : TEXCOORD0;
                float4 positionHCS  : SV_POSITION;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            
            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            half4 _NegativeColor;
            half4 _PositiveColor;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }
            float InvLerp(float from, float to, float value)
            {
                return (value - from) / (to - from);
            }
            half4 setGraph(Varyings input){
                half4 color = half4(0, 0, 0, 1);
                float threshold = 0.05; // adjust this to control the thickness of the line
                float pi = 3.14;
                float y = (_Amplitude/_MaxAmplitude)*sin(input.uv.x*2*pi*_Frequency+_Position/(2*pi))+0.5;
                if (abs(input.uv.y - y) < threshold) {
                    color = half4(1, 1, 1, 1);
                }
    
                return color;
            }
            half4 frag(Varyings IN) : SV_Target
            {
                return SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * setGraph(IN) * lerp(_NegativeColor,_PositiveColor,InvLerp(_MaxAmplitude, -_MaxAmplitude, _Amplitude)); ;
                
            }
            ENDHLSL
        
        }

    
        
    }
}
