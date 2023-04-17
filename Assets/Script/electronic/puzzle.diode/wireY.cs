using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
public class wireY : wireProp
{
    public float prevVolt,baseVoltage=5,cosValue;
    MeshRenderer childRenderer;
    public FlowControlRay controlRay;
    WaveGenerator waveGenerator;
    wireQuery wireQueryGroup;
    MeshFilter meshFilter;
    public bool diodeVolt;
    

    public class FlowControlRay {
        public List<Ray> input;
        public List<Ray> output;
        public float scale;

        public FlowControlRay(List<Ray> rayInput,List<Ray> rayOutput,float length){
            input = rayInput;
            output = rayOutput;
            scale = length;
        }

        
    }
    private float calVoltage(List<Ray> ctrlFlowRay){
        prevVolt = voltage;
        float volt = 0;
        float distance = 0;
        bool isDiodeOutputWay = false;
        //int minusVoltTome = 0;
        foreach(Ray ray in ctrlFlowRay){
            
            GameObject voltInputObj = wireQueryGroup.findParentObjectHit(ray,controlRay.scale,0);
            
            if(voltInputObj.CompareTag("diodeSlot")){

                wireDiodeSlot wireDiodeSlot = voltInputObj.GetComponent<wireDiodeSlot>();
                Ray raySlot = wireDiodeSlot.toggleRay.getRay();
                float angle = Mathf.Atan2(ray.direction.z,ray.direction.x)*Mathf.Rad2Deg - Mathf.Atan2(raySlot.direction.z,raySlot.direction.x)*Mathf.Rad2Deg;
                
                if(Mathf.Abs(angle)<90 && wireDiodeSlot.diodeSlot.collideGameObject){
                    isDiodeOutputWay = true;
                    if(Mathf.Abs(wireDiodeSlot.getVoltage())>distance){
                        
                        distance = Mathf.Abs(wireDiodeSlot.getVoltage());
                        volt = wireDiodeSlot.getVoltage();
    
                    }
                }
            }else if(voltInputObj.CompareTag("wire")){
                wire wireObj = voltInputObj.GetComponent<wire>();
                if(!isDiodeOutputWay || wireObj.m_isNearGenerator){
                    /*if(this.gameObject.name == "wire.y.001"){
                        Debug.Log(wireObj.getVoltage());
                    }*/
                    if(Mathf.Abs(wireObj.getVoltage())>distance){
                        distance = Mathf.Abs(wireObj.getVoltage());
                        volt = wireObj.getVoltage();
                    }
                }

            }else if(voltInputObj.CompareTag("wire2way")){
                float voltTemp = 0;
                wire2way wireObj = voltInputObj.GetComponent<wire2way>();
                /*if(this.gameObject.name == "wire.y.002"){
                    Debug.Log(!isDiodeOutputWay);
                }*/
                foreach(var item in wireObj.toggleRays){
                    if(item.allGameObject.Contains(this.gameObject) && (!isDiodeOutputWay || item.m_isNearGenerator)){
                        voltTemp = item.volt;
                        break;
                    }
                }
                

                   
                if(Mathf.Abs(voltTemp)>distance){
                    distance = Mathf.Abs(voltTemp);
                    volt = voltTemp;
                }
            }
        }
        
        diodeVolt = isDiodeOutputWay;
        return volt; 
    }
    void Awake(){
        childRenderer = this.gameObject.transform.Find("wire.y").GetComponent<MeshRenderer>();

        waveGenerator = this.gameObject.transform.parent.Find("wire.straight.hole").gameObject.GetComponent<WaveGenerator>();
        wireQueryGroup = this.gameObject.AddComponent<wireQuery>();
        meshFilter = this.gameObject.transform.Find("wire.y").GetComponent<MeshFilter>();

        controlRay = new FlowControlRay(
                new List<Ray>(){new Ray(transform.position+Vector3.down*0.1f,transform.TransformDirection(Vector3.forward))},
                new List<Ray>(){
                    new Ray(transform.position+Vector3.down*0.1f,transform.TransformDirection(new Vector3(-1,0,-1))),
                    new Ray(transform.position+Vector3.down*0.1f,transform.TransformDirection(new Vector3(1,0,-1)))
                },Vector2.Distance(Vector2.zero,new Vector2(meshFilter.mesh.bounds.size.x,meshFilter.mesh.bounds.size.z))
        );
        
    }

    void FixedUpdate(){
        cosValue = waveGenerator.getCosValue();
        if(this.gameObject.name == "wire.y.001"){
            /*RaycastHit hit;
            if(Physics.Raycast(controlRay.input[0],out hit,Mathf.Infinity)){
                Debug.DrawLine(controlRay.input[0].origin,controlRay.input[0].origin+hit.distance*controlRay.input[0].direction,Color.red);
                Debug.Log(hit.collider.transform.parent.gameObject.name);
            }*/
        }
        voltage = calVoltage(controlRay.output.Concat(controlRay.input).ToList());
        wireQueryGroup.SetColor(voltage,childRenderer);
    }
    
}
