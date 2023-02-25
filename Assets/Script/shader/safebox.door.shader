Shader "Custom/safebox.door"
{
    Properties
    {
        // Specular vs Metallic workflow
        [HideInInspector] _WorkflowMode("WorkflowMode", Float) = 1.0

        [Header(BaseTexture)]
        [MainColor] _BaseColor("Color", Color) = (0.5,0.5,0.5,1)
        [MainTexture] _BaseMap("Base Texture", 2D) = "white" {}

        [Header(ledIn)]
        [HDR]_ledInEmissionColor("Led in Color", Color) = (0.5,0.5,0.5,1)
        _ledInEmissionTexture("Led in texture", 2D) = "white" {}
        _ledInEmissionIntensity("Led in strength",float) = 1

        [Header(ledOut)]
        [HDR]_ledOutEmissionColor("Led out Color", Color) = (0.5,0.5,0.5,1)
        _ledOutEmissionTexture("Led out texture", 2D) = "white" {}
        _ledOutEmissionIntensity("Led Out strength",float) = 1


        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        _GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0
        _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0

        [Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _MetallicGlossMap("Metallic", 2D) = "white" {}

        _SpecColor("Specular", Color) = (0.2, 0.2, 0.2)
        _SpecGlossMap("Specular", 2D) = "white" {}

        [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        [ToggleOff] _EnvironmentReflections("Environment Reflections", Float) = 1.0

        _BumpScale("Scale", Float) = 1.0
        _BumpMap("Normal Map", 2D) = "bump" {}

        _OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
        _OcclusionMap("Occlusion", 2D) = "white" {}

        _EmissionColor("Color", Color) = (0,0,0)
        _EmissionMap("Emission", 2D) = "white" {}

        // Blending state
        [HideInInspector] _Surface("__surface", Float) = 0.0
        [HideInInspector] _Blend("__blend", Float) = 0.0
        [HideInInspector] _AlphaClip("__clip", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0
        [HideInInspector] _Cull("__cull", Float) = 2.0
        
        _ReceiveShadows("Receive Shadows", Float) = 1.0

            // Editmode props
        [HideInInspector] _QueueOffset("Queue offset", Float) = 0.0
    }

    SubShader
    {
        // With SRP we introduce a new "RenderPipeline" tag in Subshader. This allows to create shaders
        // that can match multiple render pipelines. If a RenderPipeline tag is not set it will match
        // any render pipeline. In case you want your subshader to only run in LWRP set the tag to
        // "UniversalRenderPipeline"
        Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True"}
        LOD 300
        HLSLINCLUDE
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            TEXTURE2D(_ledInEmissionTexture);
            TEXTURE2D(_ledOutEmissionTexture);
            SAMPLER(sampler_ledInEmissionTexture);
            SAMPLER(sampler_ledOutEmissionTexture);

            half4 _ledInEmissionColor;
            half4 _ledOutEmissionColor;
            float _ledInEmissionIntensity;
            float _ledOutEmissionIntensity;
            


            CBUFFER_START(UnityPerMaterial)
                float4 _ledInEmissionTexture_ST;
                float4 _ledOutEmissionTexture_ST;               
            CBUFFER_END
        ENDHLSL
        // ------------------------------------------------------------------
        // Forward pass. Shades GI, emission, fog and all lights in a single pass.
        // Compared to Builtin pipeline forward renderer, LWRP forward renderer will
        // render a scene with multiple lights with less drawcalls and less overdraw.
        Pass
        {
            // "Lightmode" tag must be "UniversalForward" or not be defined in order for
            // to render objects.
            Name "StandardLit"
            Tags{"LightMode" = "UniversalForward"}

            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_Cull]

            HLSLPROGRAM

            #pragma vertex LitPassVertex
            #pragma fragment LitPassFragment

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            
            const static int colorSize = 8;
            float _IntArray[8] = {0,0,0,0,0,0,0,0};
            
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
                float2 uv           : TEXCOORD0;
                float2 uvLM         : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv                       : TEXCOORD0;
                float2 uvLM                     : TEXCOORD1;
                float4 positionWSAndFogFactor   : TEXCOORD2; // xyz: positionWS, w: vertex fog factor
                half3  normalWS                 : TEXCOORD3;
            #if _NORMALMAP
                half3 tangentWS                 : TEXCOORD4;
                half3 bitangentWS               : TEXCOORD5;
            #endif

            #ifdef _MAIN_LIGHT_SHADOWS
                float4 shadowCoord              : TEXCOORD6; // compute shadow coord per-vertex for the main light
            #endif
                float4 positionCS               : SV_POSITION;
            };

            Varyings LitPassVertex(Attributes input)
            {
                Varyings output;

                // VertexPositionInputs contains position in multiple spaces (world, view, homogeneous clip space)
                // Our compiler will strip all unused references (say you don't use view space).
                // Therefore there is more flexibility at no additional cost with this struct.
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

                // Similar to VertexPositionInputs, VertexNormalInputs will contain normal, tangent and bitangent
                // in world space. If not used it will be stripped.
                VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                // Computes fog factor per-vertex.
                float fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

                // TRANSFORM_TEX is the same as the old shader library.
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.uvLM = input.uvLM.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                output.positionWSAndFogFactor = float4(vertexInput.positionWS, fogFactor);
                output.normalWS = vertexNormalInput.normalWS;

                // Here comes the flexibility of the input structs.
                // In the variants that don't have normal map defined
                // tangentWS and bitangentWS will not be referenced and
                // GetVertexNormalInputs is only converting normal
                // from object to world space
                #ifdef _NORMALMAP
                    output.tangentWS = vertexNormalInput.tangentWS;
                    output.bitangentWS = vertexNormalInput.bitangentWS;
                #endif

                #ifdef _MAIN_LIGHT_SHADOWS
                // shadow coord for the main light is computed in vertex.
                // If cascades are enabled, LWRP will resolve shadows in screen space
                // and this coord will be the uv coord of the screen space shadow texture.
                // Otherwise LWRP will resolve shadows in light space (no depth pre-pass and shadow collect pass)
                // In this case shadowCoord will be the position in light space.
                    output.shadowCoord = GetShadowCoord(vertexInput);
                #endif
                // We just use the homogeneous clip position from the vertex input
                output.positionCS = vertexInput.positionCS;
                return output;
            }
            half4 setInitialTexture(Varyings input){
                half4 colorBase = SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap, input.uv)*_BaseColor;
                half4 color = colorBase;
                return color;
            }
            half4 setFinalEmission(Varyings input){
                half4 colorLedIn = SAMPLE_TEXTURE2D(_ledInEmissionTexture,sampler_ledInEmissionTexture, input.uv)*_ledInEmissionColor;
                for (int j = 0; j <colorSize; j++)
                {
                    if (input.uv.x > j * (1.0 / colorSize) && input.uv.x <= (j + 1) * (1.0 / colorSize))
                        colorLedIn = colorLedIn*_IntArray[j];
                }
                half4 colorLedOut = SAMPLE_TEXTURE2D(_ledOutEmissionTexture,sampler_ledOutEmissionTexture, input.uv)*_ledOutEmissionColor;
                half4 color = colorLedIn*_ledInEmissionIntensity + colorLedOut*_ledOutEmissionIntensity;
                return color;
            }
            half4 LitPassFragment(Varyings input) : SV_Target
            {
                // Surface data contains albedo, metallic, specular, smoothness, occlusion, emission and alpha
                // InitializeStandarLitSurfaceData initializes based on the rules for standard shader.
                // You can write your own function to initialize the surface data of your shader.
                SurfaceData surfaceData;
                InitializeStandardLitSurfaceData(input.uv, surfaceData);

                #if _NORMALMAP
                half3 normalWS = TransformTangentToWorld(surfaceData.normalTS,
                    half3x3(input.tangentWS, input.bitangentWS, input.normalWS));
                #else
                half3 normalWS = input.normalWS;
                #endif
                normalWS = normalize(normalWS);

                #ifdef LIGHTMAP_ON
                // Normal is required in case Directional lightmaps are baked
                half3 bakedGI = SampleLightmap(input.uvLM, normalWS);
                #else
                // Samples SH fully per-pixel. SampleSHVertex and SampleSHPixel functions
                // are also defined in case you want to sample some terms per-vertex.
                half3 bakedGI = SampleSH(normalWS);
                #endif

                float3 positionWS = input.positionWSAndFogFactor.xyz;
                half3 viewDirectionWS = SafeNormalize(GetCameraPositionWS() - positionWS);

                // BRDFData holds energy conserving diffuse and specular material reflections and its roughness.
                // It's easy to plugin your own shading fuction. You just need replace LightingPhysicallyBased function
                // below with your own.
                BRDFData brdfData;
                surfaceData.albedo = setInitialTexture(input);
                surfaceData.emission = setFinalEmission(input);
                InitializeBRDFData(surfaceData.albedo, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness, surfaceData.alpha, brdfData);

                #ifdef _MAIN_LIGHT_SHADOWS
                // Main light is the brightest directional light.
                // It is shaded outside the light loop and it has a specific set of variables and shading path
                // so we can be as fast as possible in the case when there's only a single directional light
                // You can pass optionally a shadowCoord (computed per-vertex). If so, shadowAttenuation will be
                // computed.
                Light mainLight = GetMainLight(input.shadowCoord);
                #else
                Light mainLight = GetMainLight();
                #endif

                // Mix diffuse GI with environment reflections.
                half3 color = GlobalIllumination(brdfData, bakedGI, surfaceData.occlusion, normalWS, viewDirectionWS);

                // LightingPhysicallyBased computes direct light contribution.
                color += LightingPhysicallyBased(brdfData, mainLight, normalWS, viewDirectionWS);

                // Additional lights loop
                #ifdef _ADDITIONAL_LIGHTS

                // Returns the amount of lights affecting the object being renderer.
                // These lights are culled per-object in the forward renderer
                int additionalLightsCount = GetAdditionalLightsCount();
                for (int i = 0; i < additionalLightsCount; ++i)
                {
                    // Similar to GetMainLight, but it takes a for-loop index. This figures out the
                    // per-object light index and samples the light buffer accordingly to initialized the
                    // Light struct. If _ADDITIONAL_LIGHT_SHADOWS is defined it will also compute shadows.
                    Light light = GetAdditionalLight(i, positionWS);

                    // Same functions used to shade the main light.
                    color += LightingPhysicallyBased(brdfData, light, normalWS, viewDirectionWS);
                }
                #endif
                // Emission
                color += surfaceData.emission;

                float fogFactor = input.positionWSAndFogFactor.w;

                // Mix the pixel color with fogColor. You can optionaly use MixFogColor to override the fogColor
                // with a custom one.
                color = MixFog(color, fogFactor);
                return half4(color, surfaceData.alpha);
            }
            ENDHLSL
        }

        // Used for rendering shadowmaps
        UsePass "Universal Render Pipeline/Lit/ShadowCaster"

        // Used for depth prepass
        // If shadows cascade are enabled we need to perform a depth prepass. 
        // We also need to use a depth prepass in some cases camera require depth texture
        // (e.g, MSAA is enabled and we can't resolve with Texture2DMS
        UsePass "Universal Render Pipeline/Lit/DepthOnly"

        // Used for Baking GI. This pass is stripped from build.
        UsePass "Universal Render Pipeline/Lit/Meta"
    }

    // Uses a custom shader GUI to display settings. Re-use the same from Lit shader as they have the
    // same properties.
}
