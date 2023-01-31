using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
public class wireDiodeSlot : MonoBehaviour
{
    // Start is called before the first frames update
    public float voltage,baseVoltage=5,scale;
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
        public void setDirection(Vector3 val){
            m_ray.direction = val;
        }
        public Ray getRay(){
            return m_ray;
        }
    }
    public ToggleRay toggleRay;
    WaveGenerator waveGenerator;
    diodeSlot diodeSlot;
    public float getVoltage(){
        return voltage;
    }
    private void SetColor(float voltageInput,MeshRenderer childRenderer){
        float colorRange = Mathf.InverseLerp(baseVoltage*Mathf.Cos(Mathf.PI),baseVoltage*Mathf.Cos(0),voltageInput);
        Color color = new Color(1-colorRange,colorRange,0,1);
        childRenderer.material.SetColor("_BaseColor",color);
        childRenderer.material.EnableKeyword("_EMISSION");
        childRenderer.material.SetColor("_EmissionColor",color);
    }
    private void Awake() {
        toggleRay = new ToggleRay(new Ray(transform.position,transform.TransformDirection(1,0,1)),0);
        Mesh mesh = this.gameObject.transform.Find("wire.slot.diode").GetComponent<MeshFilter>().mesh;
        scale = mesh.bounds.size.z;
        waveGenerator = this.gameObject.transform.parent.Find("wire.straight.hole").gameObject.GetComponent<WaveGenerator>();
        diodeSlot = this.gameObject.transform.Find("collider").gameObject.GetComponent<diodeSlot>();
    }
    private float findWireHit(Ray ray,float length){
        float volt = 0;
        //Debug.DrawRay(ray.origin,ray.direction,Color.magenta);
        GameObject hitParentObj = findParentObjectHit(ray,length,7);
        if(hitParentObj.CompareTag("wire")){
            volt = hitParentObj.GetComponent<wire>().getVoltage();
        }else if(hitParentObj.CompareTag("WaveGenerator")){
            volt = hitParentObj.GetComponent<WaveGenerator>().getVoltage();
        }else if(hitParentObj.CompareTag("wire2way")){
            foreach(var item in hitParentObj.GetComponent<wire2way>().toggleRays){
                if(item.allGameObject.Contains(this.gameObject)){
                    volt = item.volt;
                }
            }
        }
        return volt;
    }
    public GameObject findParentObjectHit(Ray ray,float length,int layer){
        RaycastHit hit;
        GameObject hitParentObj = new GameObject();
        //Debug.DrawRay(ray.origin,ray.direction*length,Color.magenta,60);
        if(Physics.Raycast(ray,out hit,length+(float)0.01,1<<layer)){
            hitParentObj = hit.collider.gameObject.transform.parent.gameObject;
        }
        return hitParentObj;
    }
    void FixedUpdate(){
        float cosValue = waveGenerator.getCosValue();
        if(cosValue>=0&& diodeSlot.gameObject.name!=null){
            voltage = findWireHit(toggleRay.getRay(),scale);
        }
    }
}
