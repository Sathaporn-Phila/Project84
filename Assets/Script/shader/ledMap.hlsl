float3 _ledplatform[3] = {float3(0,0,0),float3(0,0,0),float3(0,0,0)};
void mapPlatformBase_float(UnityTexture2D base,float2 uv,out float4 colorBase,out float3 colorEmission){
    colorBase = SAMPLE_TEXTURE2D(base, base.samplerstate, uv);
    colorEmission = SAMPLE_TEXTURE2D(base, base.samplerstate, uv);
    for(int i=1;i<4;i++){
        if(uv.y>1-i*0.1f &&  uv.y<1-(i-1)*0.1f && (_ledplatform[i-1].x != 0 || _ledplatform[i-1].y != 0 || _ledplatform[i-1].z != 0)){
            colorBase = float4(1,1,1,1);
            colorEmission = float3(_ledplatform[i-1].x,_ledplatform[i-1].y,_ledplatform[i-1].z);
        }else{
            colorEmission = float3(0,0,0);
        }
    }
}