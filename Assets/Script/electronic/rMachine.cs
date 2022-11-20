using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;
public class rMachine : MonoBehaviour
{
    private Dictionary<GameObject,GameObject> rSlotLed;
    Query query;
    
    private Dictionary<string,string> patterns = new Dictionary<string,string>(){
        {"pair",@"\d*$"},{"slot",@"\b.slot\.\d*$"},{"led",@"\b.led\.\d*$"}}
        ;
    
    Regex reg;
    bool changecheckPattern(Regex rgx,string newPattern,GameObject obj){
        rgx = new Regex(newPattern);
        return rgx.IsMatch(obj.name);
    }
    private void Awake() {
        rSlotLed = new Dictionary<GameObject,GameObject>();
        query = this.gameObject.AddComponent<Query>();
        List<GameObject> slots = query.queryByName(this.gameObject,new Regex(patterns["slot"]));
        List<GameObject> led = query.queryByName(this.gameObject,new Regex(patterns["led"]));
        var rEnum = slots.Zip(led,(eachSlot,eachLed)=> new{eachSlot,eachLed});
        foreach(var item in rEnum){
            rSlotLed.Add(item.eachSlot,item.eachLed);
        }
        foreach(var item in rSlotLed){
            Debug.Log(item.Key.name+" "+item.Value.name);
        }
        
        //rSlotLed = pair.Where(slot=>changecheckPattern(reg,patterns["slot"],slot))
                    //.Zip(pair.Where(led=>changecheckPattern(reg,patterns["led"],led)),(slot,led)=>new{slot,led}));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
