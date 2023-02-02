using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class wire : MonoBehaviour
{   
    public float voltage=0,cosValue;
    public int current;
    MeshRenderer childRenderer;
    List<toggleRay> toggleRay = new List<toggleRay>();
    WaveGenerator waveGen;
    wireQuery wireQueryGroup;
    
    private void Awake() {
        //Debug.Log(this.gameObject.transform.position);
        childRenderer = this.gameObject.transform.Find(getLedObjectName()).gameObject.GetComponent<MeshRenderer>();
        waveGen = this.gameObject.transform.parent.Find("wire.straight.hole").gameObject.GetComponent<WaveGenerator>();
        wireQueryGroup = this.gameObject.AddComponent<wireQuery>();
        InitialRay();
    }
    public float getVoltage(){
        return voltage;
    }

    private string getLedObjectName(){ 
        string name = "";
        if(Regex.IsMatch(this.gameObject.name,@"\bwire.straight")){
            name = this.gameObject.transform.Find("wire.straight").name;
        }
        else if(Regex.IsMatch(this.gameObject.name,@"\bwire.curve")){
            name = this.gameObject.transform.Find("line.curve").name;
        }
        return name;
    }
    public float getCosValue(){
        return cosValue;
    }
    
    private void InitialRay(){
        Vector3 origin = transform.position + new Vector3((float)0,(float)-0.5,(float)0);
        if(Regex.IsMatch(this.gameObject.name,@"\bwire.straight")){
            Ray m_InputRay = new Ray(origin,transform.TransformDirection(Vector3.forward));
            toggleRay.Add(new toggleRay(m_InputRay,0));
        }else if(Regex.IsMatch(this.gameObject.name,@"\bwire.curve")){
            Ray m_InputRay1 = new Ray(origin,transform.TransformDirection(Vector3.back));
            Ray m_InputRay2 = new Ray(origin,transform.TransformDirection(Vector3.right));
            
            toggleRay.AddRange(new List<toggleRay>{
                new toggleRay(m_InputRay1,0),new toggleRay(m_InputRay2,0)
            });
        }
    }

    private void FixedUpdate() {
        voltage = wireQueryGroup.findWireHit(toggleRay[current].getRay(),transform.localScale.z,0);
        if(toggleRay.Count.Equals(1)){
            cosValue = waveGen.getCosValue();
            toggleRay[current].toggle(cosValue);
        }
        else{
            float nextCosValue = waveGen.getCosValue();   
            if(cosValue*nextCosValue<0){
                current = (current + 1)%toggleRay.Count;
            }
            cosValue = nextCosValue;
        }
        wireQueryGroup.SetColor(voltage,childRenderer);
        
    }
    
}