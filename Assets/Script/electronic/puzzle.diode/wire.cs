using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class wire : MonoBehaviour
{   
    public float voltage=0,baseVoltage = 5,cosValue;
    public int current;
    MeshRenderer childRenderer;
    public class ToggleRay {
        Ray m_ray;
        float prevCosValue;
        public ToggleRay(Ray ray,float val){
            m_ray = ray;
            prevCosValue = val;
        }
        public void toggle(float val){
            if(prevCosValue*val<0){
                m_ray.direction = -m_ray.direction;
            }
            prevCosValue = val;
        }
        public Ray getRay(){
            return m_ray;
        }

    }
    List<ToggleRay> toggleRay = new List<ToggleRay>();
    WaveGenerator waveGen;
    //public GameObject prevGameObject,nextGameObject;
    private void Awake() {
        //Debug.Log(this.gameObject.transform.position);
        childRenderer = this.gameObject.transform.Find(getLedObjectName()).gameObject.GetComponent<MeshRenderer>();
        Vector3 origin = transform.position + new Vector3((float)0,(float)-0.5,(float)0);
        if(Regex.IsMatch(this.gameObject.name,@"\bwire.straight")){
            Ray m_InputRay = new Ray(origin,transform.TransformDirection(Vector3.forward));
            toggleRay.Add(new ToggleRay(m_InputRay,0));
        }else if(Regex.IsMatch(this.gameObject.name,@"\bwire.curve")){
            Ray m_InputRay1 = new Ray(origin,transform.TransformDirection(Vector3.back));
            Ray m_InputRay2 = new Ray(origin,transform.TransformDirection(Vector3.right));
            
            toggleRay.AddRange(new List<ToggleRay>{
                new ToggleRay(m_InputRay1,0),new ToggleRay(m_InputRay2,0)
            });
        }
        waveGen = this.gameObject.transform.parent.Find("wire.straight.hole").gameObject.GetComponent<WaveGenerator>();
    }
    private void SetColor(float voltageInput){
        float colorRange = Mathf.InverseLerp(baseVoltage*Mathf.Cos(Mathf.PI),baseVoltage*Mathf.Cos(0),voltageInput);
        Color color = new Color(1-colorRange,colorRange,0,1);
        childRenderer.material.SetColor("_BaseColor",color);
        childRenderer.material.EnableKeyword("_EMISSION");
        childRenderer.material.SetColor("_EmissionColor",color);
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
    private float findWireHit(Ray ray,float length){
        float volt = 0;
        GameObject hitParentObj = findParentObjectHit(ray,length);
        if(hitParentObj.CompareTag("wire")){
            volt = hitParentObj.GetComponent<wire>().getVoltage();
        }else if(hitParentObj.CompareTag("WaveGenerator")){
            volt = hitParentObj.GetComponent<WaveGenerator>().getVoltage();
        }else if(hitParentObj.CompareTag("wire2way")){
            foreach(var item in hitParentObj.GetComponent<wire2way>().toggleRays){
                if(item.allGameObject.Contains(this.gameObject)){
                    volt = item.volt;
                    break;
                }
            }
        }
        return volt;
    }
    private GameObject findParentObjectHit(Ray ray,float length){
        RaycastHit hit;
        GameObject hitParentObj = new GameObject();
        Debug.DrawRay(ray.origin,ray.direction,Color.magenta);
        if(Physics.Raycast(ray,out hit,length+(float)0.01)){
            hitParentObj = hit.collider.gameObject.transform.parent.gameObject;
        }
        return hitParentObj;
    }

    private void FixedUpdate() {
        voltage = findWireHit(toggleRay[current].getRay(),transform.localScale.z);
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
        SetColor(voltage);
        
    }
    
}