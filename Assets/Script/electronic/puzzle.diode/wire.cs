using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
public class wire : wireProp
{   
    public bool m_isNearGenerator,needWaveGen;
    public float cosValue;
    float scale;
    public int current,waitTime=-1;
    List<MeshRenderer> childRenderer = new List<MeshRenderer>();
    public List<toggleRay> toggleRay = new List<toggleRay>();

    
    WaveGenerator waveGen;
    wireQuery wireQueryGroup;
    MeshFilter meshFilter;
    private void Awake() {

        
        needWaveGen = Regex.IsMatch(this.transform.parent.name,@"\bdiode.machine")?true:false;
        
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
        Vector3 origin;
        if(transform.parent.name=="diode.machine"){
            origin = transform.position + Vector3.down*0.1f;
        }else{
            origin = transform.position + Vector3.down*0.5f;
            
        }
        if(Regex.IsMatch(this.gameObject.name,@"\bwire.straight") || Regex.IsMatch(this.gameObject.name,@"\bwire.resistor.slot")){
            Ray m_InputRay1 = new Ray(origin,transform.TransformDirection(Vector3.forward));
            Ray m_InputRay2 = new Ray(origin,transform.TransformDirection(Vector3.back));
            toggleRay.AddRange(new List<toggleRay>{new toggleRay(m_InputRay1,0),new toggleRay(m_InputRay2,0)});
        }else if(Regex.IsMatch(this.gameObject.name,@"\bwire.curve")){
            Ray m_InputRay1 = new Ray(origin,transform.TransformDirection(Vector3.back));
            Ray m_InputRay2 = new Ray(origin,transform.TransformDirection(Vector3.left));
            //Debug.DrawLine(m_InputRay1.origin,m_InputRay1.origin + scale*m_InputRay1.direction,Color.magenta,60);
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
        GameObject wireLineHit = wireQueryGroup.findParentObjectHit(toggleRay[current].getRay(),scale,0);
        GameObject wireYHit = wireQueryGroup.findParentObjectHit(toggleRay[current].getRay(),scale,7);
        /*if(this.gameObject.name == "wire.curve.001"){
            GameObject gameObj = wireQueryGroup.findParentObjectHit(toggleRay[current].getRay(), scale, 7);
            Debug.Log(gameObj.name);
        }*/
        
        if(!Regex.IsMatch(this.gameObject.name,@"\bwire.resistor.slot")){
            if (wireLineHit)
            {
                voltage = wireQueryGroup.findWireHit(toggleRay[current].getRay(), scale, 0);
            }
            else if (wireYHit)
            {
                voltage = wireQueryGroup.findWireHit(toggleRay[current].getRay(), scale, 7);
            }
            else
            {
                voltage = 0;
            }
        }

        else {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(toggleRay[current].getRay(), scale);
            RaycastHit hit = hits[1]; //skip first object
            voltage = -wireQueryGroup.getVoltFromHitObj(hit.collider.transform.parent.gameObject);
        }

        //โค้ดส่วนนี้ทำงานเมื่อเป็น diode machine 
        if(needWaveGen && !Regex.IsMatch(this.gameObject.name,@"\bwire.resistor.slot")){
            float nextCosValue = waveGen.getCosValue();   
            if(m_isNearGenerator){
                if(cosValue*nextCosValue<0){
                    current = (current + 1)%toggleRay.Count;
                    voltage = 0;
                }
            
               
            }else{
                float voltTemp = 0;
                if(wireYHit){
                    voltTemp = voltage;
                    //Debug.Log(voltTemp);
                    if(cosValue*nextCosValue<0){
                        
                        if(Mathf.Abs(voltTemp)>0){
                            voltage = 0;

                            if(this.gameObject.name=="wire.curve.001"){
                                this.transform.parent.BroadcastMessage("setDirectionVoltRead",0,SendMessageOptions.DontRequireReceiver);
                            }
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
                    if(this.gameObject.name=="wire.curve.001"){
                        this.transform.parent.BroadcastMessage("setDirectionVoltRead",1,SendMessageOptions.DontRequireReceiver);
                    }

                }
                waitTime--;
            }   

            cosValue = nextCosValue;
        }
        foreach(MeshRenderer renderer in childRenderer){
            if(childRenderer.Count <= 1){
                wireQueryGroup.SetColor(voltage,renderer);
            }else{
                //เมื่อตำแหน่งสายไฟมีทิศเดียวกับทิศทางที่ถูก raycast ไป
                if((toggleRay[current].getRay().origin-renderer.transform.position).normalized.z*toggleRay[current].getRay().direction.z>=0){
                    wireQueryGroup.SetColor(voltage,renderer);
                }else{
                    wireQueryGroup.SetColor(-voltage,renderer);
                }
            }
        }
        
        /*if(this.transform.root.name=="puzzle.gate"){
            GameObject hitObj = wireQueryGroup.findParentObjectHit(toggleRay[current].getRay(),scale,0);
            if(hitObj){
                if(!Regex.IsMatch(hitObj.name,@"\bwire.curve")){
                Debug.Log(wireQueryGroup.getVoltFromHitObj(hitObj));
                }
            }else{
                Debug.Log(wireQueryGroup.getVoltFromHitObj(hitObj));
            }
        }*/
    }
    
}