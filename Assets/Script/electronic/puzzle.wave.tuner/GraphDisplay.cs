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
        if(buttonBehaviour == DirectionButton.ButtonBehaviour.Amplitude){
            float amplitude = yourGraph.material.GetFloat("_Amplitude");
            if(dir == DirectionButton.Direction.Left){
                yourGraph.material.SetFloat("_Amplitude",amplitude-0.5f);
            }else{
                yourGraph.material.SetFloat("_Amplitude",amplitude+0.5f);
            }

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
