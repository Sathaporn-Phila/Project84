using UnityEngine;
using UnityEngine.UI;

 
public class GraphDisplay : MonoBehaviour
{
    public Image originGraph,yourGraph;
    public doorRoom doortrigger;
    
    public virtual void Awake() {
        setGraph();
        setEnemy();
    }
    public void setGraph(){
        originGraph = this.transform.Find("wave.origin").GetComponent<Image>();
        yourGraph = this.transform.Find("wave.your").GetComponent<Image>();
        Material mat = Instantiate(originGraph.material);
        Material mat2 = Instantiate(yourGraph.material);
        mat.SetFloat("_Amplitude",Random.Range(-5,5));

        float pos = Random.Range(0,360);
        mat.SetFloat("_Position",pos-pos%15f);

        originGraph.material = mat;
        yourGraph.material = mat2;
    }
    public virtual void setEnemy(){}
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

        checkSameVal();
    } 
    public virtual void checkSameVal(){
        if(originGraph.material.GetFloat("_Amplitude") == yourGraph.material.GetFloat("_Amplitude") && originGraph.material.GetFloat("_Position") == yourGraph.material.GetFloat("_Position")){
            doortrigger.Open();
        }
    }    
}
