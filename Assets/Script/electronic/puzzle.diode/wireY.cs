using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
public class wireY : MonoBehaviour
{
    public float voltage,baseVoltage=5,cosValue,Count;
    MeshRenderer childRenderer;
    public FlowControlRay controlRay;
    WaveGenerator waveGenerator;
    wireQuery wireQueryGroup;
    MeshFilter meshFilter;
    public float getVoltage(){
        return voltage;
    }

    public class FlowControlRay {
        public List<Ray> input;
        public List<Ray> output;
        public float scale;

        public FlowControlRay(List<Ray> rayInput,List<Ray> rayOutput,float length){
            input = rayInput;
            output = rayOutput;
            scale = length;
        }
        public void Switchflow(GameObject objCompare){
            bool isSwitch = false;
            foreach(Ray ray in this.input){
                
                //Debug.Log(findParentObjectHit(ray,scale).name);
                
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
            Debug.DrawRay(ray.origin,ray.direction*length,Color.magenta,60);
            if(Physics.Raycast(ray,out hit,length)){
                GameObject hitParentObj = hit.collider.gameObject.transform.parent.gameObject;
                return hitParentObj;
            }else{
                return null;
            }
        }
        
    }


    private float calVoltage(List<Ray> ctrlFlowRay){
        float volt = 0;
        int minusVoltTome = 0;
        foreach(Ray ray in ctrlFlowRay){

            GameObject voltInputObj = wireQueryGroup.findParentObjectHit(ray,controlRay.scale,0);
            if(voltInputObj.CompareTag("diodeSlot")){
                wireDiodeSlot wireDiodeSlot = voltInputObj.GetComponent<wireDiodeSlot>();
                Ray raySlot = wireDiodeSlot.toggleRay.getRay();
                float angle = Mathf.Atan2(ray.direction.z,ray.direction.x)*Mathf.Rad2Deg - Mathf.Atan2(raySlot.direction.z,raySlot.direction.x)*Mathf.Rad2Deg;
    
                if(Mathf.Abs(angle)<90){
                    //สามารถมี volt ได้ครั้งเดียว
                    if(minusVoltTome<1 && wireDiodeSlot.getVoltage()<0){
                        volt = wireDiodeSlot.getVoltage();
                        minusVoltTome++;
                    }else{
                        volt = Mathf.Max(volt,wireDiodeSlot.getVoltage());
                    }
                }
            }else if(voltInputObj.CompareTag("wire")){
                wire wireObj = voltInputObj.GetComponent<wire>();
                    //สามารถมี volt ติดลบ ได้ครั้งเดียว
                if(minusVoltTome<1 && wireObj.getVoltage()<0){
                    volt = wireObj.getVoltage();
                    minusVoltTome++;
                }else{
                    volt = Mathf.Max(volt,wireObj.getVoltage());
                }
            }else if(voltInputObj.CompareTag("wire2way")){
                float voltTemp = 0;
                wire2way wireObj = voltInputObj.GetComponent<wire2way>();
                foreach(var item in wireObj.toggleRays){
                    if(item.allGameObject.Contains(this.gameObject)){
                        voltTemp = item.volt;
                        break;
                    }
                }
                    //สามารถมี volt ติดลบได้ครั้งเดียว
                if(minusVoltTome<1 && voltTemp<0){
                    volt = voltTemp;
                    minusVoltTome++;
                }else{
                    volt = Mathf.Max(volt,voltTemp);
                }
            }
            /*if(ctrlFlowRay.Count > 1){
                volt = Mathf.Max(volt,voltInput);
                    //voltage = inputVoltage;
            }else if(Mathf.Abs(voltInput)>0){
                volt = voltInput;
            }*/
        }
        return volt; 
    }
    void Awake(){
        childRenderer = this.gameObject.transform.Find("wire.y").GetComponent<MeshRenderer>();

        waveGenerator = this.gameObject.transform.parent.Find("wire.straight.hole").gameObject.GetComponent<WaveGenerator>();
        wireQueryGroup = this.gameObject.AddComponent<wireQuery>();
        meshFilter = this.gameObject.transform.Find("wire.y").GetComponent<MeshFilter>();

        controlRay = new FlowControlRay(
                new List<Ray>(){new Ray(transform.position,transform.TransformDirection(Vector3.forward))},
                new List<Ray>(){
                    new Ray(transform.position,transform.TransformDirection(new Vector3(-1,0,-1))),
                    new Ray(transform.position,transform.TransformDirection(new Vector3(1,0,-1)))
                },Vector2.Distance(Vector2.zero,new Vector2(meshFilter.mesh.bounds.size.x,meshFilter.mesh.bounds.size.z))
        );
        if(!(this.gameObject.name == "wire.y" || this.gameObject.name == "wire.y.004")){
            List<Ray> raysOut = controlRay.output;
            List<Ray> raysIn = controlRay.input;
            controlRay.input = raysOut;
            controlRay.output = raysIn;
        }
        Count = controlRay.input.Count;
    }

    void FixedUpdate(){
        cosValue = waveGenerator.getCosValue();
        voltage = calVoltage(controlRay.input.Concat(controlRay.output).ToList());
        wireQueryGroup.SetColor(voltage,childRenderer);
    }
    void Update()
    {
        
    }
}
