using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using TMPro;

public class slot : MonoBehaviour
{
    public int lightIntensity = 5;
    Regex rgx = new Regex(@"\bresistor");
    rMachine resistorMachine;
    MaterialPropertyBlock mpb;
    public MaterialPropertyBlock Mpb {
        get {
            mpb = mpb == null? new MaterialPropertyBlock():mpb;
            return mpb;
        }
    }
    void setLight(GameObject led){
        MeshRenderer renderer = led.GetComponent<MeshRenderer>();
        renderer.material.EnableKeyword("_EMISSION");
        Mpb.SetColor("_BaseColor",Color.green);
        Mpb.SetColor("_EmissionColor",Color.green*Mathf.Pow(2,lightIntensity));
        renderer.SetPropertyBlock(Mpb);
        //Debug.Log(renderer.material.GetColor("_Color"));
    }
    private void Awake() {
        resistorMachine = this.gameObject.transform.parent.gameObject.GetComponent<rMachine>();
        
    }
    private void OnTriggerEnter(Collider other) {
        if(rgx.IsMatch(other.gameObject.name)){
            GameObject led = resistorMachine.getLedFrom(this.gameObject.name);
            if(other.gameObject.GetComponent<resistor>().Prop.val.ToString() == resistorMachine.getSlotGroup().Find(x=>x.slotObj==this.gameObject).textObj.GetComponent<TextMeshPro>().text){
                setLight(led);
            }
        }
    }

    
}