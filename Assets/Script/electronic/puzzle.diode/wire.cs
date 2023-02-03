using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class wire : MonoBehaviour
{   
    public bool m_isNearGenerator;
    public float voltage=0,cosValue;
    float scale;
    int current;
    MeshRenderer childRenderer;
    public List<toggleRay> toggleRay = new List<toggleRay>();
    WaveGenerator waveGen;
    wireQuery wireQueryGroup;
    MeshFilter meshFilter;
    private void Awake() {
        //Debug.Log(this.gameObject.transform.position);
        childRenderer = this.gameObject.transform.Find(getLedObjectName()).gameObject.GetComponent<MeshRenderer>();
        waveGen = this.gameObject.transform.parent.Find("wire.straight.hole").gameObject.GetComponent<WaveGenerator>();
        wireQueryGroup = this.gameObject.AddComponent<wireQuery>();
        InitialRay();
        if(Regex.IsMatch(this.gameObject.name,@"\bwire.resistor.slot")){
            meshFilter = this.gameObject.transform.Find("wire.resistor.slot").gameObject.GetComponent<MeshFilter>(); 
            scale = meshFilter.mesh.bounds.size.z;
        }else{
            scale = transform.localScale.z;
        }
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
        }else if(Regex.IsMatch(this.gameObject.name,@"\bwire.t")){
            name = this.gameObject.transform.Find("wire.t").name;
        }else if(Regex.IsMatch(this.gameObject.name,@"\bwire.resistor.slot")){
            name = this.gameObject.transform.Find("wire.straight.002").name;
        }
        return name;
    }
    public float getCosValue(){
        return cosValue;
    }
    
    private void InitialRay(){
        Vector3 origin = transform.position + new Vector3((float)0,(float)-0.5,(float)0);
        if(Regex.IsMatch(this.gameObject.name,@"\bwire.straight") || Regex.IsMatch(this.gameObject.name,@"\bwire.resistor.slot")){
            Ray m_InputRay1 = new Ray(origin,transform.TransformDirection(Vector3.forward));
            Ray m_InputRay2 = new Ray(origin,transform.TransformDirection(Vector3.back));
            toggleRay.AddRange(new List<toggleRay>{new toggleRay(m_InputRay1,0),new toggleRay(m_InputRay2,0)});
        }else if(Regex.IsMatch(this.gameObject.name,@"\bwire.curve")){
            Ray m_InputRay1 = new Ray(origin,transform.TransformDirection(Vector3.back));
            Ray m_InputRay2 = new Ray(origin,transform.TransformDirection(Vector3.right));
            
            toggleRay.AddRange(new List<toggleRay>{
                new toggleRay(m_InputRay1,0),new toggleRay(m_InputRay2,0)
            });
        }else if(Regex.IsMatch(this.gameObject.name,@"\bwire.t")){
            Ray m_InputRay1 = new Ray(origin,transform.TransformDirection(Vector3.forward));
            Ray m_InputRay2 = new Ray(origin,transform.TransformDirection(Vector3.back));
            
            toggleRay.AddRange(new List<toggleRay>{
                new toggleRay(m_InputRay1,0),new toggleRay(m_InputRay2,0)
            });
        }

    }

    private void FixedUpdate() {
        if(m_isNearGenerator){
            if(wireQueryGroup.findParentObjectHit(toggleRay[current].getRay(),scale,0)){
                voltage = wireQueryGroup.findWireHit(toggleRay[current].getRay(),scale,0);
            }else if(wireQueryGroup.findParentObjectHit(toggleRay[current].getRay(),scale,7)){
                voltage = wireQueryGroup.findWireHit(toggleRay[current].getRay(),scale,7);
            }

            float nextCosValue = waveGen.getCosValue();   
            if(cosValue*nextCosValue<0){
                current = (current + 1)%toggleRay.Count;
            }
            cosValue = nextCosValue;
               
        }else{
            float voltTemp = 0;
            float distance = 0;
            foreach(toggleRay ray in toggleRay){
                if(wireQueryGroup.findParentObjectHit(ray.getRay(),scale,0)){
                    voltTemp  = wireQueryGroup.findWireHit(ray.getRay(),scale,0);
                }else if(wireQueryGroup.findParentObjectHit(ray.getRay(),scale,7)){
                    voltTemp  = wireQueryGroup.findWireHit(ray.getRay(),scale,7);
                }

                if(Mathf.Abs(voltTemp) > distance){
                    distance = voltTemp;
                    voltage = Regex.IsMatch(this.gameObject.name,@"\bwire.resistor.slot")?-voltTemp:voltTemp;                        
                    break;
                }
            }
        }
        wireQueryGroup.SetColor(voltage,childRenderer);
        
    }
    
}