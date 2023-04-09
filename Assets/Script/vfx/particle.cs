using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GradientEffect : Gradient
{
    public Gradient gradient;
    public GradientColorKey[] colorKey;
    public GradientAlphaKey[] alphaKey;

    public GradientEffect(Color mainColor,Color subColor){
        gradient = new Gradient();
        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[2];
        colorKey[0].color = mainColor;
        colorKey[0].time = 0.0f;
        colorKey[1].color = subColor;
        colorKey[1].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);
    }
    public GradientEffect(List<Color> listColor){
        gradient = new Gradient();
        colorKey = new GradientColorKey[listColor.Count];
        alphaKey = new GradientAlphaKey[listColor.Count];
        foreach(var it in listColor.Select((Col,Index)=> new {Col,Index})){
            colorKey[it.Index].color = it.Col;
            colorKey[it.Index].time = it.Index/listColor.Count;
            alphaKey[it.Index].alpha = 1;
            alphaKey[it.Index].time = it.Index/listColor.Count;
        }
        gradient.SetKeys(colorKey, alphaKey);
    }
}
