using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public class wireY : MonoBehaviour
{
    public float voltage,baseVoltage=5,cosValue;
    public float getVoltage(){
        return voltage;
    }
    public class FlowControlRay {
        public List<Ray> input;
        public List<Ray> output;
        float scale;
        public FlowControlRay(List<Ray> rayInput,List<Ray> rayOutput,float length){
            input = rayInput;
            output = rayOutput;
            scale = length;
        }
        public void Switchflow(GameObject objCompare){
            bool isSwitch = false;
            foreach(Ray ray in this.input){
                if(findParentObjectHit(ray,scale).name.Equals(objCompare.name)){
                    input.Remove(ray);
                    output.Add(ray);
                    isSwitch = true;
                    break;
                }
            }
            if(!isSwitch){
                foreach(Ray ray in this.output){
                if(findParentObjectHit(ray,scale).name.Equals(objCompare.name)){
                    output.Remove(ray);
                    input.Add(ray);
                    isSwitch = true;
                    break;
                }
                }
            }
        }
        private GameObject findParentObjectHit(Ray ray,float length){
            RaycastHit hit;
            GameObject hitParentObj = new GameObject();
            Debug.DrawRay(ray.origin,ray.direction*scale,Color.magenta,60);
            if(Physics.Raycast(ray,out hit,scale+(float)0.01)){
                hitParentObj = hit.collider.gameObject.transform.parent.gameObject;
            }
            return hitParentObj;
        }
        
    }
    MeshRenderer childRenderer;
    public FlowControlRay controlRay;
    WaveGenerator waveGenerator;
    private void SetColor(float voltageInput){
        float colorRange = Mathf.InverseLerp(baseVoltage*Mathf.Cos(Mathf.PI),baseVoltage*Mathf.Cos(0),voltageInput);
        Color color = new Color(1-colorRange,colorRange,0,1);
        childRenderer.material.SetColor("_BaseColor",color);
        childRenderer.material.EnableKeyword("_EMISSION");
        childRenderer.material.SetColor("_EmissionColor",color);
    }
    void Awake(){
        childRenderer = this.gameObject.transform.Find("wire.y").GetComponent<MeshRenderer>();
        controlRay = new FlowControlRay(
                new List<Ray>(){new Ray(transform.position,transform.TransformDirection(Vector3.forward))},
                new List<Ray>(){
                    new Ray(transform.position,transform.TransformDirection(new Vector3(-1,0,-1))),
                    new Ray(transform.position,transform.TransformDirection(new Vector3(1,0,-1)))
                },transform.localScale.z);
        if(!(this.gameObject.name == "wire.y" || this.gameObject.name == "wire.y.004")){
            List<Ray> raysOut = controlRay.output;
            List<Ray> raysIn = controlRay.input;
            controlRay.input = raysOut;
            controlRay.output = raysIn;
        }
        waveGenerator = this.gameObject.transform.parent.Find("wire.straight.hole").gameObject.GetComponent<WaveGenerator>();
    }
    private float findWireHit(Ray ray,float length){
        float volt = 0;
        //Debug.DrawRay(ray.origin,ray.direction,Color.magenta);
        GameObject hitParentObj = findParentObjectHit(ray,length);
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
    private GameObject findParentObjectHit(Ray ray,float length){
        RaycastHit hit;
        GameObject hitParentObj = new GameObject();
        if(Physics.Raycast(ray,out hit,length+(float)0.01)){
            hitParentObj = hit.collider.gameObject.transform.parent.gameObject;
        }
        return hitParentObj;
    }

    void FixedUpdate(){
        cosValue = waveGenerator.getCosValue();
        if(cosValue>=0){
            foreach(Ray ray in controlRay.input){
                float voltInput = findWireHit(ray,transform.localScale.z);
                if(Mathf.Abs(voltInput)>0 && controlRay.input.Count > 1){
                    voltage = Mathf.Max(voltage,voltInput);
                    //voltage = inputVoltage;
                }else if(Mathf.Abs(voltInput)>0){
                    voltage = voltInput;
                }
            }
        }else{
            foreach(Ray ray in controlRay.output){
                float voltInput = findWireHit(ray,transform.localScale.z);
                if(Mathf.Abs(voltInput)>0 && controlRay.output.Count > 1){
                    voltage = Mathf.Max(voltage,voltInput);
                    //voltage = inputVoltage;
                }else if(Mathf.Abs(voltInput)>0){
                    voltage = voltInput;
                }
            }
        }
        SetColor(voltage);
    }
    void Update()
    {
        
    }
}
