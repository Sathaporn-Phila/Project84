using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wireQuery : MonoBehaviour
{
    // Start is called before the first frame update
    float baseVoltage = 5;
    public float findWireHit(Ray ray,float length,int layer=0){
        float volt = 0;
        GameObject hitParentObj = findParentObjectHit(ray,length,layer);
        if(hitParentObj!=null){
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
            }else if(hitParentObj.CompareTag("wireY")){
                volt = hitParentObj.GetComponent<wireY>().getVoltage();
            }
        }
        return volt;
    }
    public GameObject findParentObjectHit(Ray ray,float length,int layer){
        RaycastHit hit;
        Debug.DrawRay(ray.origin,ray.direction,Color.magenta);
        if(Physics.Raycast(ray,out hit,length+(float)0.01,1<<layer)){
            GameObject hitParentObj = hit.collider.gameObject.transform.parent.gameObject;
            if(this.gameObject.name == "wire.slot.diode.004"){
                Debug.Log(hitParentObj.name);
            }
            return hitParentObj;
        }else{
            return null;
        }
    }
    public void SetColor(float voltageInput,MeshRenderer childRenderer){
        float colorRange = Mathf.InverseLerp(baseVoltage*Mathf.Cos(Mathf.PI),baseVoltage*Mathf.Cos(0),voltageInput);
        Color color = new Color(1-colorRange,colorRange,0,1);
        childRenderer.material.SetColor("_BaseColor",color);
        childRenderer.material.EnableKeyword("_EMISSION");
        childRenderer.material.SetColor("_EmissionColor",color);
    }
}
