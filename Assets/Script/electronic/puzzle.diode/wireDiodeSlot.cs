using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
public class wireDiodeSlot : MonoBehaviour
{
    // Start is called before the first frames update
    public float voltage,baseVoltage=5,scale;
    public toggleRay toggleRay;
    WaveGenerator waveGenerator;
    diodeSlot diodeSlot;
    wireQuery wireQueryGroup;
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
        toggleRay = new toggleRay(new Ray(transform.position,transform.TransformDirection(-1,0,-1)),0);
        Debug.DrawRay(toggleRay.m_ray.origin,toggleRay.m_ray.direction,Color.blue,60);
        Mesh mesh = this.gameObject.transform.Find("wire.slot.diode").GetComponent<MeshFilter>().mesh;
        scale = Vector2.Distance(Vector2.zero,new Vector2(mesh.bounds.size.x,mesh.bounds.size.z));
        waveGenerator = this.gameObject.transform.parent.Find("wire.straight.hole").gameObject.GetComponent<WaveGenerator>();
        diodeSlot = this.gameObject.transform.Find("collider").gameObject.GetComponent<diodeSlot>();
        wireQueryGroup = this.gameObject.AddComponent<wireQuery>();
    }

    void FixedUpdate(){
        float cosValue = waveGenerator.getCosValue();
        if(diodeSlot.gameObject.name!=null){
            voltage = wireQueryGroup.findWireHit(toggleRay.getRay(),scale,7);
            //Debug.Log(voltage);
            //Debug.Log(wireQueryGroup.findParentObjectHit(toggleRay.getRay(),scale,7).name);
        }
    }
}
