using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
public class wire2way : MonoBehaviour
{
    public float cosValue;
    public class ToggleRay : toggleRay {
        float voltage,scale;
        public float volt;
        public List<GameObject> allGameObject = new List<GameObject>();
        public ToggleRay(Ray ray,float val,float length):base(ray,val){
            scale = length;
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,scale)){
                allGameObject.Add(hit.collider.gameObject.transform.parent.gameObject);
            }
        }
        public override void toggle(float val){
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

    }
    public List<ToggleRay> toggleRays = new List<ToggleRay>();
    List<MeshRenderer> renderers = new List<MeshRenderer>();
    Dictionary<ToggleRay,MeshRenderer> lines;
    WaveGenerator waveGenerator;
    wireQuery wireQueryGroup;

    
    
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
        wireQueryGroup = this.gameObject.AddComponent<wireQuery>();
    }
    void FixedUpdate(){
        cosValue = waveGenerator.getCosValue();
        foreach(ToggleRay toggleRay in lines.Keys){
            float voltage = wireQueryGroup.findWireHit(toggleRay.getRay(),transform.localScale.z+(float)0.01);
            toggleRay.toggle(cosValue);
            toggleRay.volt = voltage;
            wireQueryGroup.SetColor(voltage,lines[toggleRay]);
        }

    }
    // Update is called once per frame
    
}
