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

            //if insert correct slot and correct value
            if(other.TryGetComponent<resistor>(out resistor r)){
                float nearDivider = (r.Prop.val.ToString().Length-1) - ((r.Prop.val.ToString().Length-1) % 3);
                if(string.Join(" ",r.Prop.val/Mathf.Pow(10,nearDivider),string.Join("",r.Prop.findPrefixSymbol((int)nearDivider),"\u2126")) == resistorMachine.getSlotGroup().Find(x=>x.slotObj==this.gameObject).textObj.GetComponent<TextMeshPro>().text){
                    setLight(led);
                }
            }
        }
    }

    
}
