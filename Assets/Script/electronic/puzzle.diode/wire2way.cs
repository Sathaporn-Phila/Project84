using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
public class wire2way : MonoBehaviour
{
    public float cosValue;
    public class ToggleRay {
        Ray m_ray;
        float prevCosValue,voltage,scale;
        public float volt;
        public List<GameObject> allGameObject = new List<GameObject>();
        public ToggleRay(Ray ray,float val,float length){
            m_ray = ray;
            prevCosValue = val;
            scale = length;
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,scale)){
                allGameObject.Add(hit.collider.gameObject.transform.parent.gameObject);
            }
        }
        public void toggle(float val){
            if(prevCosValue*val<0){
                m_ray.direction = -m_ray.direction;
                RaycastHit hit;
                if(Physics.Raycast(m_ray,out hit,scale)){
                    if(!allGameObject.Contains(hit.collider.gameObject.transform.parent.gameObject)){
                        allGameObject.Add(hit.collider.gameObject.transform.parent.gameObject);
                    }
                }
            }
            prevCosValue = val;
        }
        public Ray getRay(){
            return m_ray;
        }

    }
    public List<ToggleRay> toggleRays = new List<ToggleRay>();
    List<MeshRenderer> renderers = new List<MeshRenderer>();
    Dictionary<ToggleRay,MeshRenderer> lines;
    WaveGenerator waveGenerator;
    private void SetColor(float voltageInput,MeshRenderer renderer){
        float baseVoltage = 5;
        float colorRange = Mathf.InverseLerp(baseVoltage*Mathf.Cos(Mathf.PI),baseVoltage*Mathf.Cos(0),voltageInput);
        Color color = new Color(1-colorRange,colorRange,0,1);
        renderer.material.SetColor("_BaseColor",color);
        renderer.material.EnableKeyword("_EMISSION");
        renderer.material.SetColor("_EmissionColor",color);
    }
    private float findWireHit(Ray ray,float length){
        float volt = 0;
        //Debug.DrawRay(ray.origin,ray.direction,Color.magenta);
        GameObject hitParentObj = findParentObjectHit(ray,length);
        if(hitParentObj.GetComponent<wire>()!=null){
            volt = hitParentObj.GetComponent<wire>().getVoltage();
        }else if(hitParentObj.GetComponent<WaveGenerator>()!=null){
            volt = hitParentObj.GetComponent<WaveGenerator>().getVoltage();
        }else if(hitParentObj.GetComponent<wire2way>()!=null){
            foreach(var item in hitParentObj.GetComponent<wire2way>().toggleRays){
                        if(item.allGameObject.Contains(this.gameObject)){
                            volt = item.volt;
                        }
            }
        }
        return volt;
    }
    private GameObject findParentObjectHit(Ray ray,float length){
        RaycastHit hit;
        GameObject hitParentObj = new GameObject();
        if(Physics.Raycast(ray,out hit,transform.localScale.z+(float)0.01)){
            hitParentObj = hit.collider.gameObject.transform.parent.gameObject;
        }
        return hitParentObj;
    }
    void Awake()
    {
        renderers.AddRange(new List<MeshRenderer>(){
            this.gameObject.transform.Find("wire.up").gameObject.GetComponent<MeshRenderer>(),
            this.gameObject.transform.Find("wire.down").gameObject.GetComponent<MeshRenderer>(),
        });
        
        toggleRays.AddRange(new List<ToggleRay>(){
            new ToggleRay(new Ray(transform.position,transform.TransformDirection(Vector3.back)),0,transform.localScale.z+(float)0.01),
            new ToggleRay(new Ray(transform.position,transform.TransformDirection(Vector3.left)),0,transform.localScale.z+(float)0.01),
        });
        lines = toggleRays.Zip(renderers,(ray,renderMesh)=>new {ray,renderMesh}).ToDictionary(val=>val.ray,val=>val.renderMesh);
        waveGenerator = this.gameObject.transform.parent.Find("wire.straight.hole").gameObject.GetComponent<WaveGenerator>();
    }
    void FixedUpdate(){
        cosValue = waveGenerator.getCosValue();
        foreach(ToggleRay toggleRay in lines.Keys){
            float voltage = findWireHit(toggleRay.getRay(),transform.localScale.z+(float)0.01);
            toggleRay.toggle(cosValue);
            toggleRay.volt = voltage;
            SetColor(voltage,lines[toggleRay]);
        }

    }
    // Update is called once per frame
    
}
