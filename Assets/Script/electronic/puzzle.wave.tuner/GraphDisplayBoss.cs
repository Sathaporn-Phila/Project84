using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphDisplayBoss : GraphDisplay
{
    public override void checkSameVal(){
        if(originGraph.material.GetFloat("_Amplitude") == yourGraph.material.GetFloat("_Amplitude") && originGraph.material.GetFloat("_Position") == yourGraph.material.GetFloat("_Position")){
            action();
        }
    }
    private void action(){
        //boss.hp -= 1
    }
}
