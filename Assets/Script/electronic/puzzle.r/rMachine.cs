using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Linq;
using TMPro;
public class rMachine : MonoBehaviour
{
    List<SlotGroup> slotGroups = new List<SlotGroup>();
    
    public class SlotGroup {
        public GameObject slotObj;
        public GameObject led;
        public GameObject textObj;
        public SlotGroup(GameObject eachSlot,GameObject eachLed,GameObject eachText){
            slotObj = eachSlot;
            led = eachLed;
            textObj = eachText;
        }
    }
    //private Dictionary<GameObject,GameObject> rSlotLed =  new Dictionary<GameObject,GameObject>();
    protected Query query = new Query();
    
    private Dictionary<string,string> patterns = new Dictionary<string,string>(){
        {"pair",@"\d*$"},{"slot",@"\b.slot\.\d*$"},{"led",@"\b.led\.\d*$"}
    };
    public GameObject getLedFrom(string key){
        GameObject led = slotGroups.Find(x => x.slotObj.name == key).led;
        return led;
    }
    
    private void matchSlotGroup(){
        List<resistor.Attribute> resistor2match = new List<resistor.Attribute>();
        
        List<GameObject> slots = query.queryByName(this.gameObject,new Regex(patterns["slot"]));
        List<GameObject> led = query.queryByName(this.gameObject,new Regex(patterns["led"]));
        List<GameObject> text = (this.gameObject.transform.Find("ohm.sticker/Canvas").GetComponentsInChildren<Transform>()).Skip(1).Select(t=>t.gameObject).ToList();
        for(int i=0;i<slots.Count;i++){

            SlotGroup slotGroup = new SlotGroup(slots[i],led[i],text[i]);
            slotGroups.Add(slotGroup);
        }

    }
    /*private void spawnResistor(List<GameObject> resistorList){
        int i=0;
        foreach(SlotGroup slotGroup in slotGroups){
            GameObject cloneObj = Instantiate(resistorList[i],slotGroup.slotObj.transform.position+7*Vector3.up,resistorList[i].transform.rotation);
            if(i>1){
                cloneObj.GetComponent<resistor>().Prop = resistorList[i].GetComponent<resistor>().Prop; 
                cloneObj.GetComponent<resistor>().SetColor();
            }
            slotGroup.textObj.GetComponent<TextMeshPro>().text = resistorList[i].GetComponent<resistor>().Prop.val.ToString();
            /*resistorList[i].transform.position = slotGroup.slotObj.transform.position + Vector3.up*5;
            resistorList[i].transform.rotation = Quaternion.Euler(0,0,0);
            i++;
        }
        
    }*/
    public List<SlotGroup> getSlotGroup(){
        return slotGroups;
    }
    private void Awake() {
        matchSlotGroup();    
    }
    
}
