using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
 
public class GraphDisplay : MonoBehaviour
{
    Image originGraph,yourGraph;
    
    private void Awake() {
        originGraph = this.transform.Find("wave.origin").GetComponent<Image>();
        yourGraph = this.transform.Find("wave.your").GetComponent<Image>();
        
    }
    public void ChangeVal(DirectionButton.Direction dir,DirectionButton.ButtonBehaviour buttonBehaviour){
        float maxAmp = 5;
        float minAmp = 5;
        if(buttonBehaviour == DirectionButton.ButtonBehaviour.Amplitude){
            float nextAmplitude  = 0;
            float amplitude = yourGraph.material.GetFloat("_Amplitude");
            if(dir == DirectionButton.Direction.Left){
                nextAmplitude = amplitude-0.5f;
                yourGraph.material.SetFloat("_Amplitude",nextAmplitude);
            }else{
                nextAmplitude = amplitude+0.5f;
                yourGraph.material.SetFloat("_Amplitude",nextAmplitude);
            }
            yourGraph.material.SetColor("_BaseColor",Color.Lerp(new Color32(255, 192, 203,1),new Color32(255, 87, 51,1),Mathf.InverseLerp(minAmp,maxAmp,nextAmplitude)));
            

        }else{
            float pos = yourGraph.material.GetFloat("_Position");
            if(dir == DirectionButton.Direction.Left){
                yourGraph.material.SetFloat("_Position",pos-15f);
            }else{
                yourGraph.material.SetFloat("_Position",pos+15f);
            }
        }
    } 

       
    
    
    
}
