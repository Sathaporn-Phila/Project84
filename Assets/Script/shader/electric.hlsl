half4 _ColorArray[4] = {half4(0,0,0,1),half4(0,0,0,1),half4(0,0,0,1),half4(0,0,0,1)};
const static int colorSize = 4;
void mapSticker_float(UnityTexture2D input,UnitySamplerState ss,float2 uv,out half4 color){
    color = SAMPLE_TEXTURE2D(input,ss,uv);
    for (int j = 0; j < colorSize; j++){
        if (uv.x > j * (1.0 / colorSize) && uv.x <= (j + 1) * (1.0 / colorSize)){
            color = _ColorArray[j];
        }
    }
}