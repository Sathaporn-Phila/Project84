using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class WaveGenerator : wireProp
{
    public bool m_IsVoltageGenerator;
    public float phaseStart;
    public float baseVoltage=5,cosValue,nextCosValue;
    MeshRenderer childRenderer;
    wireQuery wireQueryGroup;
    Ray ray;
    private string getLedObjectName(){
        name = this.gameObject.transform.Find("wire.straight.hole").name;
        return name;
    }
    public float getCosValue(){
        return cosValue;
    }

    void Awake()
    {
        childRenderer = this.gameObject.transform.Find(getLedObjectName()).gameObject.GetComponent<MeshRenderer>();
        ray = new Ray(transform.position+Vector3.down/2,transform.TransformDirection(Vector3.back));
        wireQueryGroup = this.gameObject.AddComponent<wireQuery>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float m_currentTime = Time.time;
        cosValue = Mathf.Cos(m_currentTime+(Mathf.Deg2Rad*phaseStart));
        if(m_IsVoltageGenerator){
            if(cosValue >= 0){
                nextCosValue = Mathf.Cos(Time.deltaTime+m_currentTime+(Mathf.Deg2Rad*(phaseStart)));
                if(cosValue*nextCosValue >= 0){
                    voltage = baseVoltage*cosValue;    
                }else{
                    voltage = 0;
                }
                

            }else{
                /*GameObject testObj = wireQueryGroup.findParentObjectHit(ray,transform.localScale.z,0);
                Debug.Log(testObj.name);*/
                voltage = wireQueryGroup.findWireHit(ray,transform.localScale.z);
                nextCosValue = 0;
            }
        }else{
            voltage = wireQueryGroup.findWireHit(ray,transform.localScale.z,0);
        }

        
        wireQueryGroup.SetColor(voltage,childRenderer);
    }
}
