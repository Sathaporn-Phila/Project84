half4 _ColorArray[4] = {half4(0,0,0,1),half4(0,0,0,1),half4(0,0,0,1),half4(0,0,0,1)};
uniform float _IntArray[8];
float4 color;
void mapSticker_float(UnityTexture2D input,UnitySamplerState ss,float2 uv,out half4 color){
    color = SAMPLE_TEXTURE2D(input,ss,uv);
    int colorSize = 4;
    for (int j = 0; j < colorSize; j++){
        if (uv.x > j * (1.0 / colorSize) && uv.x <= (j + 1) * (1.0 / colorSize)){
            color = _ColorArray[j];
        }
    }
}
void mapBaseColor_float(UnityTexture2D input,UnitySamplerState ss,float2 uv,out half4 color){
    color = SAMPLE_TEXTURE2D(input,ss,uv);
}
float InvLerp(float from, float to, float value)
{
    return (value - from) / (to - from);
}
void mapEmissionColor_float(UnityTexture2D _ledInEmissionTexture,UnityTexture2D _ledOutEmissionTexture,UnitySamplerState ss,float2 uv,float4 _ledZeroBitEmissionColor,float4 _ledOneBitEmissionColor,float4 _ledOutEmissionCloseColor,float4 _ledOutEmissionOpenColor,float _ledOutAnim,float _ledOutEmissionIntensity,float _ledInEmissionIntensity,out half4 colorEmission){
    half4 colorLedIn = SAMPLE_TEXTURE2D(_ledInEmissionTexture,ss, uv);
    int colorSize = 8;
    for (int j = 0; j <colorSize; j++)
    {
        if (uv.x > j * (1.0 / colorSize) && uv.x <= (j + 1) * (1.0 / colorSize)){
            if(_IntArray[j] == 0){
                colorLedIn = colorLedIn*_ledZeroBitEmissionColor;
            }else if(_IntArray[j] == 1){
                colorLedIn = colorLedIn*_ledOneBitEmissionColor;
            }else{
                colorLedIn = colorLedIn*-1;
            }
        }

    }
    half4 colorLedOut = SAMPLE_TEXTURE2D(_ledOutEmissionTexture,ss, uv)*lerp(_ledOutEmissionCloseColor,_ledOutEmissionOpenColor,uv.x+_ledOutAnim);
    colorEmission = colorLedIn*_ledInEmissionIntensity + colorLedOut*_ledOutEmissionIntensity;
}