using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
public class wire2way : MonoBehaviour
{
    public float cosValue;
    public class ToggleRay : toggleRay {

        public float volt,scale;
        public bool m_isNearGenerator = false;
        public List<GameObject> allGameObject = new List<GameObject>();
        public ToggleRay(Ray ray,float val,float length):base(ray,val){
            scale = length;
            Ray inverseRay = new Ray(ray.origin,-ray.direction);
            setBoolNearGenerator(new List<Ray>{ray,inverseRay});
        }
        
        private void setBoolNearGenerator(List<Ray> item){
            foreach(Ray ray in item){
                RaycastHit hit;
                if(Physics.Raycast(ray,out hit,scale)){
                    GameObject hitObj = hit.collider.gameObject.transform.parent.gameObject;
                    //Debug.Log(hitObj.name);
                    allGameObject.Add(hitObj);
                    if(!m_isNearGenerator && hitObj.CompareTag("wire")){
                        bool isFromGenerator = hitObj.GetComponent<wire>().m_isNearGenerator;
                        if(isFromGenerator){
                            m_isNearGenerator = true;
                        }
                    }
                }
            }
        }

    }
    public List<ToggleRay> toggleRays = new List<ToggleRay>();
    List<MeshRenderer> renderers = new List<MeshRenderer>();
    Dictionary<ToggleRay,MeshRenderer> lines;
    WaveGenerator waveGenerator;
    wireQuery wireQueryGroup;

    public void setDirectionVoltRead(int i){
        foreach(ToggleRay item in toggleRays){
            if(!item.m_isNearGenerator){
                Debug.Log(i);
                item.setDirection(i==0?transform.TransformDirection(Vector3.left):transform.TransformDirection(Vector3.right));
            }
        }
    }
    
    void Awake()
    {
        renderers.AddRange(new List<MeshRenderer>(){
            this.gameObject.transform.Find("wire.up").gameObject.GetComponent<MeshRenderer>(),
            this.gameObject.transform.Find("wire.down").gameObject.GetComponent<MeshRenderer>(),
        });
        
        toggleRays.AddRange(new List<ToggleRay>(){
            new ToggleRay(new Ray(transform.position,transform.TransformDirection(Vector3.back)),0,transform.localScale.z+(float)1),
            new ToggleRay(new Ray(transform.position+Vector3.down*0.45f,transform.TransformDirection(Vector3.left)),0,transform.localScale.z+(float)1),
        });
        lines = toggleRays.Zip(renderers,(ray,renderMesh)=>new {ray,renderMesh}).ToDictionary(val=>val.ray,val=>val.renderMesh);
        waveGenerator = this.gameObject.transform.parent.Find("wire.straight.hole").gameObject.GetComponent<WaveGenerator>();
        wireQueryGroup = this.gameObject.AddComponent<wireQuery>();
    }

    void FixedUpdate(){
        float nextCosValue = waveGenerator.getCosValue();
        int i=0;
        foreach(ToggleRay toggleRay in lines.Keys){
            float voltTemp = 0;
            //Debug.Log(toggleRay.m_isNearGenerator);
            if(toggleRay.m_isNearGenerator){
                toggleRay.toggle(cosValue);    
            }

            /*if(i==1){
                RaycastHit hit;
                if(Physics.Raycast(toggleRay.getRay(),out hit,Mathf.Infinity,1<<0)){
                    Debug.Log(hit.collider.transform.name);
                    Debug.DrawLine(toggleRay.getRay().origin,toggleRay.getRay().origin+toggleRay.getRay().direction*hit.distance,Color.magenta);
                }
            }*/
            GameObject wireLineHit = wireQueryGroup.findParentObjectHit(toggleRay.getRay(),toggleRay.scale,0);
            GameObject wireYHit = wireQueryGroup.findParentObjectHit(toggleRay.getRay(),toggleRay.scale,7);
            if(wireLineHit){
                //Debug.Log(wireLineHit.transform.name);
                voltTemp  = wireQueryGroup.getVoltFromHitObj(wireLineHit);
            }else if(wireYHit){
                voltTemp  = wireQueryGroup.getVoltFromHitObj(wireYHit);
            }

            toggleRay.volt = voltTemp;
            wireQueryGroup.SetColor(toggleRay.volt,lines[toggleRay]);
            cosValue = nextCosValue;
            i++;
        }
    // Update is called once per frame
    
    }
}