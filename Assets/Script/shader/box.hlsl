#pragma once
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"


// Structs
struct Attributes
{
    float3 positionOS   : POSITION;
    half3 normalOS      : NORMAL;
    half4 tangentOS     : TANGENT;
    float2 uv           : TEXCOORD0;
	float2 lightmapUV	: TEXCOORD1;
	float4 color		: COLOR;
};

// all pass will share this Varyings struct (define data needed from our vertex shader to our fragment shader)
struct Varyings
{
    float2 uv                       : TEXCOORD0;
    float4 positionWSAndFogFactor   : TEXCOORD1; // xyz: positionWS, w: vertex fog factor
    half3 normalWS                  : TEXCOORD2;
    float4 positionCS               : SV_POSITION;
	DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 3);
};
struct Surface{
	half3 Albedo;      // base (diffuse or specular) color
    half3 Normal;      // tangent space normal, if written
    half3 Emission;
    half Metallic;      // 0=non-metal, 1=metal
    half Smoothness;    // 0=rough, 1=smooth
    half Occlusion;     // occlusion (default 1)
    half Alpha;
};
struct Lighting{
	half3   normalWS;
    float3  positionWS;
    half3   viewDirectionWS;
    float4  shadowCoord;
};
Surface initSurfaceData(Varyings input){
	Surface output;
	output.Albedo = SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap,input.uv)*_BaseColor.rgb;
	output.Normal = half3(0,0,1);
	output.Emission = SAMPLE_TEXTURE2D(_EmissionMap,sampler_EmissionMap,input.uv).rgb*_EmissionColor.rgb;
	output.Metallic = 0;
	output.Smoothness = _Smoothness;
	output.Occlusion = 1;
	output.Alpha = 1;
	return output;
};
Lighting initLightningData(Varyings input){
	Lighting lightingData;
    lightingData.positionWS = input.positionWSAndFogFactor.xyz;
    lightingData.viewDirectionWS = SafeNormalize(GetCameraPositionWS() - lightingData.positionWS);  
    lightingData.normalWS = normalize(input.normalWS); //interpolated normal is NOT unit vector, we need to normalize it
    return lightingData;
};
Varyings vert(Attributes input)
{
    Varyings output;

    // VertexPositionInputs contains position in multiple spaces (world, view, homogeneous clip space, ndc)
    // Unity compiler will strip all unused references (say you don't use view space).
    // Therefore there is more flexibility at no additional cost with this struct.
    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS);

    // Similar to VertexPositionInputs, VertexNormalInputs will contain normal, tangent and bitangent
    // in world space. If not used it will be stripped.
    VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

    float3 positionWS = vertexInput.positionWS;
	float fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

    // TRANSFORM_TEX is the same as the old shader library.
    output.uv = TRANSFORM_TEX(input.uv,_BaseMap);

    // packing positionWS(xyz) & fog(w) into a vector4
    output.positionWSAndFogFactor = float4(positionWS, fogFactor);
    output.normalWS = vertexNormalInput.normalWS; //normlaized already by GetVertexNormalInputs(...)
	OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
	OUTPUT_SH(output.normalWS.xyz, output.vertexSH);
    output.positionCS = TransformWorldToHClip(positionWS);
	return output;
};

half4 frag(Varyings input):SV_TARGET{
	Surface surfData = initSurfaceData(input);
	Lighting lightingData = initLightningData(input);
	half3 bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, input.normalWS);
	float4 shadowCoord = TransformWorldToShadowCoord(lightingData.positionWS.xyz);
	Light mainLight = GetMainLight(shadowCoord);
	half3 attenuatedLightColor = mainLight.color * (mainLight.distanceAttenuation * mainLight.shadowAttenuation);

				
	MixRealtimeAndBakedGI(mainLight, input.normalWS, bakedGI);

	half3 shading = bakedGI + LightingLambert(attenuatedLightColor, mainLight.direction, input.normalWS);
	half3 color = surfData.Albedo * _BaseColor.rgb;
	return half4(color * shading, 1);	
};