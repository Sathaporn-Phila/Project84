using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class WaveGenerator : MonoBehaviour
{
    public bool m_IsVoltageGenerator;
    public float phaseStart;
    public float voltage=0,baseVoltage = 5,cosValue;
    MeshRenderer childRenderer;
    Ray ray;
    private string getLedObjectName(){
        name = this.gameObject.transform.Find("wire.straight.hole").name;
        return name;
    }
    public float getCosValue(){
        return cosValue;
    }
    private void SetColor(float voltageInput){
        float colorRange = Mathf.InverseLerp(baseVoltage*Mathf.Cos(Mathf.PI),baseVoltage*Mathf.Cos(0),voltageInput);
        Color color = new Color(1-colorRange,colorRange,0,1);
        childRenderer.material.SetColor("_BaseColor",color);
        childRenderer.material.EnableKeyword("_EMISSION");
        childRenderer.material.SetColor("_EmissionColor",color);
    }
    public float getVoltage(){
        return voltage;
    }
    void Awake()
    {
        childRenderer = this.gameObject.transform.Find(getLedObjectName()).gameObject.GetComponent<MeshRenderer>();
        Vector3 origin = transform.position + new Vector3((float)0,(float)-0.5,(float)0);
        Ray ray = new Ray(origin,transform.TransformDirection(Vector3.forward));
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float m_currentTime = Time.time;
        cosValue = Mathf.Cos(m_currentTime+(Mathf.Deg2Rad*phaseStart));
        //Debug.Log(this.gameObject.transform.parent.Find("wire.straight.hole").gameObject.GetComponent<wire>().getCosValue()>=0);
        //ใช่จุดสัญญาณไหม
        if(m_IsVoltageGenerator){
            if(cosValue >= 0){
                voltage = baseVoltage*cosValue;
            }else{
                RaycastHit hit;
                if(Physics.Raycast(ray,out hit,1)){
                    voltage = hit.collider.gameObject.transform.parent.gameObject.GetComponent<wire>().getVoltage();
                }
            }
        }
        //ดูว่าสัญญาณมาจากทางไหน
        /*else{
            if(this.gameObject.transform.parent.Find("wire.straight.hole").gameObject.GetComponent<WaveGenerator>().getCosValue()>=0){
            //ใช้สัญญาณทิศทางจากสายไฟก่อนหน้า   
                if(!Regex.IsMatch(prevGameObject.name,@"\bwire.y")){
                    voltage = prevGameObject.GetComponent<wire>().getVoltage();
                }
                else if(Regex.IsMatch(prevGameObject.name,@"\bwire.y")){
                    voltage = prevGameObject.GetComponent<wireY>().getVoltage();
                }
                //voltage = prevGameObject.GetComponent<wire>().getVoltage();
            }
            else{
            //ใช้สัญญาณทิศทางจากสายไฟถัดไป
                if(!Regex.IsMatch(nextGameObject.name,@"\bwire.y")){
                    voltage = nextGameObject.GetComponent<wire>().getVoltage();
                }
                else if(Regex.IsMatch(nextGameObject.name,@"\bwire.y")){
                    voltage = nextGameObject.GetComponent<wireY>().getVoltage();
                }
            }
        }*/
        SetColor(voltage);
    }
}
