using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;
public class rMachine : MonoBehaviour
{
    private Dictionary<GameObject,GameObject> rSlotLed;
    Query query;
    public enum SpawnType {resistor,gate,diode}
    
    private Dictionary<string,string> patterns = new Dictionary<string,string>(){
        {"pair",@"\d*$"},{"slot",@"\b.slot\.\d*$"},{"led",@"\b.led\.\d*$"}}
        ;
    
    Regex reg;
    [SerializeField]SpawnType spawnType = new SpawnType();

    GameObject prefab2Spawn(SpawnType type){
        GameObject Prefab;
        switch(type){
            case SpawnType.resistor:
                Prefab = (GameObject)Resources.Load("Prefabs/electronic/resistor");
                Prefab.transform.localScale = new Vector3((float)0.7,(float)0.7,(float)0.7);
                Prefab.transform.Rotate(Quaternion.Euler(0,0,0).eulerAngles);
                break;
            case SpawnType.gate:
                Prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
                break;
            case SpawnType.diode:
                Prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
                break;
            default:
                Prefab = null;
                break;
            
        }
        return Prefab;
    }
    bool changecheckPattern(Regex rgx,string newPattern,GameObject obj){
        rgx = new Regex(newPattern);
        return rgx.IsMatch(obj.name);
    }
    public GameObject getLedFrom(string key){
        GameObject led = rSlotLed.FirstOrDefault(x => x.Key.name == key).Value;
        return led;
    }
    private void Awake() {
        rSlotLed = new Dictionary<GameObject,GameObject>();
        query = new Query();
        List<GameObject> slots = query.queryByName(this.gameObject,new Regex(patterns["slot"]));
        List<GameObject> led = query.queryByName(this.gameObject,new Regex(patterns["led"]));
        var rEnum = slots.Zip(led,(eachSlot,eachLed)=> new{eachSlot,eachLed});
        GameObject obj2Clone = prefab2Spawn(spawnType);
        foreach(var item in rEnum){
            GameObject cloneObj = Instantiate(obj2Clone,item.eachSlot.transform.position+5*Vector3.up,Quaternion.Euler(0,0,0));
            cloneObj.AddComponent<resistor>();
            rSlotLed.Add(item.eachSlot,item.eachLed);
        }
        /*foreach(var item in rSlotLed){
            Debug.Log(item.Key.name+" "+item.Value.name);
        }*/
        
        //rSlotLed = pair.Where(slot=>changecheckPattern(reg,patterns["slot"],slot))
                    //.Zip(pair.Where(led=>changecheckPattern(reg,patterns["led"],led)),(slot,led)=>new{slot,led}));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
