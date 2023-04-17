using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public class wireQuery : MonoBehaviour
{
    // Start is called before the first frame update
    float baseVoltage = 5;
    public float findWireHit(Ray ray,float length,int layer=0){
        float volt = 0;
        GameObject hitParentObj = findParentObjectHit(ray,length,layer);
        /*if(hitParentObj){
            if(hitParentObj.name=="wire") Debug.Log(hitParentObj);
        }*/
        volt = getVoltFromHitObj(hitParentObj);
        return volt;
    }
    public GameObject findParentObjectHit(Ray ray,float length,int layer){
        RaycastHit hit;
        GameObject hitParentObj = null;
        if(Physics.Raycast(ray,out hit,length+(float)0.01,1<<layer)){
            //if(Regex.IsMatch(this.gameObject.name,@"\bwire.curve.001")){Debug.Log(hit.collider.gameObject.name);}
            /*if(Regex.IsMatch(this.gameObject.name,@"\bgate")){
                Debug.Log(hitParentObj.name);
            }*/
            //Debug.DrawLine(ray.origin,ray.origin+hit.distance*ray.direction,Color.red);
            if(hit.transform.parent!=null){
                //if(Regex.IsMatch(this.gameObject.name,@"\bwire.curve")&&Regex.IsMatch(hit.collider.gameObject.transform.parent.gameObject.name,@"\bpuzzle.slot")){Debug.Log("hit");}
                if(Regex.IsMatch(hit.collider.gameObject.name,@"\bgate")){
                    hitParentObj = hit.collider.gameObject;
                }else{
                    hitParentObj = hit.collider.gameObject.transform.parent.gameObject;
                }
            }
            
            return hitParentObj;
        }else{
            return null;
        }
        
    }
    public float getVoltFromHitObj(GameObject hitObj){
        float volt = 0;

        if(hitObj!=null){
            
            if(hitObj.CompareTag("wire2way")){

                foreach(var item in hitObj.GetComponent<wire2way>().toggleRays){
                    if(item.allGameObject.Contains(this.gameObject)){
                        volt = item.volt;
                        break;
                    }
                }
            }else{
                
                if(hitObj.TryGetComponent<wireProp>(out wireProp prop)){
                    //if(Regex.IsMatch(this.gameObject.name,@"\bgate")){Debug.Log(prop.name);}
                    
                    volt = prop.getVoltage();
                }
                else if(hitObj.TryGetComponent<Gate>(out Gate prop1)){
                    volt = prop1.getVoltage();
                }
                
            }
        }
        return volt;
    }
    public void SetColor(float voltageInput,MeshRenderer childRenderer){
        float colorRange = Mathf.InverseLerp(baseVoltage*Mathf.Cos(Mathf.PI),baseVoltage*Mathf.Cos(0),voltageInput);
        Color color = new Color(1-colorRange,colorRange,0,1);
        childRenderer.material.SetColor("_BaseColor",color);
        childRenderer.material.EnableKeyword("_EMISSION");
        childRenderer.material.SetColor("_EmissionColor",color);
    }
}
