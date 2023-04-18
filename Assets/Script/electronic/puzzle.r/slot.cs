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
    public void TurnLight(GameObject led,bool on){
        MeshRenderer renderer = led.GetComponent<MeshRenderer>();
        renderer.material.EnableKeyword("_EMISSION");
        if(on){
            Mpb.SetColor("_BaseColor",Color.green);
            Mpb.SetColor("_EmissionColor",Color.green*Mathf.Pow(2,lightIntensity));
        }else{
            Mpb.SetColor("_BaseColor",Color.red);
            Mpb.SetColor("_EmissionColor",Color.red*Mathf.Pow(2,lightIntensity));
        }
        renderer.SetPropertyBlock(Mpb);
    
    }
    private void Awake() {
        resistorMachine = this.gameObject.transform.parent.gameObject.GetComponent<rMachine>();
        
    }
    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent<resistor>(out resistor r)){
            rMachine.SlotGroup ledGroup = resistorMachine.getSlotFrom(this.gameObject.name);
            //if insert correct slot and correct value
                float nearDivider = (r.Prop.val.ToString().Length-1) - ((r.Prop.val.ToString().Length-1) % 3);
                if(string.Join(" ",r.Prop.val/Mathf.Pow(10,nearDivider),string.Join("",r.Prop.findPrefixSymbol((int)nearDivider),"\u2126")) == resistorMachine.getSlotGroup().Find(x=>x.slotObj==this.gameObject).textObj.GetComponent<TextMeshPro>().text){
                    TurnLight(ledGroup.led,true);
                    ledGroup.setLedActive(true);
                    if(resistorMachine.checkAllLed()){
                        resistorMachine.action();
                    }
                }
            }
        }
    
    private void OnTriggerExit(Collider other) {
        if(other.TryGetComponent<resistor>(out resistor r)){
            rMachine.SlotGroup ledGroup = resistorMachine.getSlotFrom(this.gameObject.name);

            TurnLight(ledGroup.led,false);
        }
    }

    
}
