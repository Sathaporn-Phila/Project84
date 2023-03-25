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
    card card;
    public class SlotGroup {
        public GameObject slotObj;
        public GameObject led;
        public GameObject textObj;
        public bool ledActive;
        public SlotGroup(GameObject eachSlot,GameObject eachLed,GameObject eachText){
            slotObj = eachSlot;
            led = eachLed;
            textObj = eachText;
            ledActive = false;
        }
        public void setLedActive(bool val){
            ledActive = val;
        }
    }
    //private Dictionary<GameObject,GameObject> rSlotLed =  new Dictionary<GameObject,GameObject>();
    protected Query query = new Query();
    
    private Dictionary<string,string> patterns = new Dictionary<string,string>(){
        {"pair",@"\d*$"},{"slot",@"\b.slot\.\d*$"},{"led",@"\b.led\.\d*$"}
    };
    public SlotGroup getSlotFrom(string key){
        SlotGroup led = slotGroups.Find(x => x.slotObj.name == key);
        return led;
    }
    public bool checkAllLed(){
        bool val = true;
        foreach(SlotGroup slot in slotGroups){
            val = val && slot.ledActive;
            if(!val){
                break;
            }
        }
        return val;
    }
    public void unlockCard(){
        card.Animated();
    }
    private void matchSlotGroup(){
        List<GameObject> resistor2match = this.transform.parent.Find("box").GetComponent<Box>().getSpawnObject().OrderBy(item=>Guid.NewGuid()).ToList();
        List<Attribute> attributes = new List<Attribute>();

        //property in resistor such as ohm value
        foreach(GameObject r in resistor2match){
            attributes.Add(r.GetComponent<resistor>().Prop);
        }
        
        List<GameObject> slots = query.queryByName(this.gameObject,new Regex(patterns["slot"]));
        List<GameObject> led = query.queryByName(this.gameObject,new Regex(patterns["led"]));
        List<GameObject> text = (this.gameObject.transform.Find("ohm.sticker/Canvas").GetComponentsInChildren<Transform>()).Skip(1).Select(t=>t.gameObject).ToList();
        Debug.Log(slots.Count);
        //set property for rMachine
        for(int i=0;i<slots.Count;i++){
            float nearDivider = (attributes[i].val.ToString().Length-1) - ((attributes[i].val.ToString().Length-1) % 3);
            //Debug.Log(string.Join(" ",attributes[i].val,attributes[i].val.ToString().Length,nearDivider));
            text[i].GetComponent<TextMeshPro>().text = string.Join(" ",attributes[i].val/Math.Pow(10,nearDivider),string.Join("",attributes[i].findPrefixSymbol((int)nearDivider),"\u2126"));
            SlotGroup slotGroup = new SlotGroup(slots[i],led[i],text[i]);
            slotGroups.Add(slotGroup);
        }

    }
    
    public List<SlotGroup> getSlotGroup(){
        return slotGroups;
    }
    private void Start() {
        matchSlotGroup();
        card = this.transform.Find("card").GetComponent<card>();
        
        unlockCard();
    }
    
}
