using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
public class wire : MonoBehaviour
{   
    public bool m_isNearGenerator,needWaveGen;
    public float voltage=0,cosValue;
    float scale,waitTime;
    public int current;
    List<MeshRenderer> childRenderer = new List<MeshRenderer>();
    public List<toggleRay> toggleRay = new List<toggleRay>();
    WaveGenerator waveGen;
    wireQuery wireQueryGroup;
    MeshFilter meshFilter;
    private void Awake() {

        if(Regex.IsMatch(this.transform.parent.name,@"\puzzle.diode")){
            needWaveGen = true;
        }else{
            
        }

        if(Regex.IsMatch(this.gameObject.name,@"\bwire.resistor.slot")){
            meshFilter = this.gameObject.transform.Find("wire.resistor.slot").gameObject.GetComponent<MeshFilter>(); 
            scale = meshFilter.mesh.bounds.size.z;
            childRenderer.AddRange(this.gameObject.GetComponentsInChildren<MeshRenderer>().Where(obj => obj.gameObject.name != this.gameObject.name && Regex.IsMatch(obj.gameObject.name,@"\bwire.straight")));

        }else if(Regex.IsMatch(this.gameObject.name,@"\bwire.curve")){
            meshFilter = this.gameObject.transform.Find("cover").gameObject.GetComponent<MeshFilter>(); 
            scale = meshFilter.mesh.bounds.size.x;
            childRenderer.Add(this.gameObject.transform.Find(getLedObjectName()).gameObject.GetComponent<MeshRenderer>());
            
            
        }else{
            scale = transform.localScale.z;
            childRenderer.Add(this.gameObject.transform.Find(getLedObjectName()).gameObject.GetComponent<MeshRenderer>());
        }

        if(needWaveGen){
            waveGen = this.gameObject.transform.parent.Find("wire.straight.hole").gameObject.GetComponent<WaveGenerator>();
        }
        
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
            Ray m_InputRay2 = new Ray(origin,transform.TransformDirection(Vector3.left));
            
            toggleRay.AddRange(new List<toggleRay>{
                new toggleRay(m_InputRay1,0),new toggleRay(m_InputRay2,0)
            });
        }
        else if(Regex.IsMatch(this.gameObject.name,@"\bwire.t")){
            Ray m_InputRay1 = new Ray(origin,transform.TransformDirection(Vector3.forward));
            Ray m_InputRay2 = new Ray(origin,transform.TransformDirection(Vector3.left));
            
            toggleRay.AddRange(new List<toggleRay>{
                new toggleRay(m_InputRay1,0),new toggleRay(m_InputRay2,0)
            });
            if(transform.localRotation.y==0){
                toggleRay temp = toggleRay[0];
                temp.m_ray.direction = -temp.m_ray.direction;
                toggleRay[0] = toggleRay[1];
                toggleRay[1] = temp;
            }
        }

    }
    public void setDirectionVoltRead(int i){
        if(!m_isNearGenerator){
            current = i;
        }
    }
    private void FixedUpdate() {
        float nextCosValue = waveGen.getCosValue();

        if(wireQueryGroup.findParentObjectHit(toggleRay[current].getRay(),scale,0)){
            voltage = wireQueryGroup.findWireHit(toggleRay[current].getRay(),scale,0);
        }else if(wireQueryGroup.findParentObjectHit(toggleRay[current].getRay(),scale,7)){
            voltage = wireQueryGroup.findWireHit(toggleRay[current].getRay(),scale,7);
        }   
        if(m_isNearGenerator){
            

            if(cosValue*nextCosValue<0){
                current = (current + 1)%toggleRay.Count;
                voltage = 0;
            }
            
               
        }else{
            float voltTemp = 0;
            GameObject hitObjLeft = wireQueryGroup.findParentObjectHit(toggleRay[current].getRay(),scale,7);
            if(hitObjLeft){
                voltTemp = wireQueryGroup.findWireHit(toggleRay[current].getRay(),scale,7);
                    //Debug.Log(voltTemp);
                if(cosValue*nextCosValue<0){
                    Debug.Log(this.gameObject.name);
                    if(Mathf.Abs(voltTemp)>0){
                        voltage = 0;
                        this.transform.parent.BroadcastMessage("setDirectionVoltRead",current,SendMessageOptions.DontRequireReceiver);
                        //List<wire2way.ToggleRay> toggleRays = this.gameObject.transform.parent.Find("wire.dive").GetComponent<wire2way>().toggleRays;
                    }else{
                        waitTime = 8;
                        //this.transform.parent.BroadcastMessage("setDirectionVoltRead",(current+1)%toggleRay.Count,SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            
        }
        if(waitTime >= 0){
            float voltTemp = wireQueryGroup.findWireHit(toggleRay[current].getRay(),scale,7);
            if(Mathf.Abs(voltTemp)==0&&waitTime==0){
                this.transform.parent.BroadcastMessage("setDirectionVoltRead",(current+1)%toggleRay.Count,SendMessageOptions.DontRequireReceiver);
            }
            waitTime--;
        }
        if(Regex.IsMatch(this.gameObject.name,@"\bwire.resistor.slot")){
            voltage = -voltage;
        }
            
        foreach(MeshRenderer renderer in childRenderer){
            if(childRenderer.Count <= 1){
                wireQueryGroup.SetColor(voltage,renderer);
            }else{
                if((toggleRay[current].getRay().origin-renderer.transform.position).normalized.z*toggleRay[current].getRay().direction.z>=0){
                    wireQueryGroup.SetColor(voltage,renderer);
                }else{
                    wireQueryGroup.SetColor(-voltage,renderer);
                }
            }
        }
        cosValue = nextCosValue;
    }
    
}