using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using Newtonsoft.Json;

public class Box: MonoBehaviour
{
    protected Query query;
    List<GameObject> itemSpawn = new List<GameObject>();
    public enum SpawnType {resistor,gate,diode}
    string pattern = @"\bmini-box\.\d*$";
    /*[SerializeField]*/public SpawnType spawnType = new SpawnType();
    public List<GameObject> getSpawnObject(){
        return itemSpawn;
    }

    string dataPath = Directory.GetCurrentDirectory()+"/Assets/data.json";
    GameObject prefab2Spawn(SpawnType type){
        GameObject Prefab;
        switch(type){
            case SpawnType.resistor:
                Prefab = (GameObject)Resources.Load("Prefabs/electronic/resistor");
                Prefab.transform.localScale = new Vector3((float)0.7,(float)0.7,(float)0.7);
                Prefab.transform.rotation = Quaternion.Euler(0,0,0);
                break;
            case SpawnType.diode:
                Prefab = (GameObject)Resources.Load("Prefabs/electronic/diode");
                //Prefab.transform.Rotate(Quaternion.Euler(0,90,0).eulerAngles);
                break;
            default:
                Prefab = null;
                break;
            
        }
        return Prefab;
    }
    private void Awake() {
        query = this.gameObject.AddComponent<Query>();
        Regex regex = new Regex(pattern);
        List<GameObject> slots = query.queryByName(this.gameObject,regex);
        
        if(spawnType == SpawnType.resistor || spawnType == SpawnType.diode){
            GameObject obj2Clone = prefab2Spawn(spawnType);
            foreach(GameObject slot in slots){
                int numSpawn = Random.Range(1,2);
                GameObject cloneObjPrototype = Instantiate(obj2Clone,slot.transform.position+Vector3.up+0.1f*transform.TransformDirection(Vector3.right),slot.transform.rotation);
                cloneObjPrototype.transform.parent = slot.transform;

                if(spawnType == SpawnType.resistor){
                    resistor rProp = cloneObjPrototype.AddComponent<resistor>();
                    for(int i=0;i<numSpawn;i++){
                        GameObject cloneObj= Instantiate(obj2Clone,slot.transform.position+2*i*Vector3.up,slot.transform.rotation);
                        cloneObj.transform.parent = slot.transform;
                        resistor rDup = cloneObj.AddComponent<resistor>();
                        rDup.Prop = rProp.Prop;
                        rDup.SetColor();
                    }
                }
                itemSpawn.Add(cloneObjPrototype);
            }
        }else{
            List<GameObject> GatePrefab = Resources.LoadAll<GameObject>("Prefabs/electronic/gate.machine.module").Where(obj=>Regex.IsMatch(obj.name,@"\bgate")).ToList();
            for(int i=0;i<GatePrefab.Count;i++){
                GameObject cloneObjPrototype = Instantiate(GatePrefab[i],slots[i].transform.position+Vector3.up*0.1f,slots[i].transform.rotation);
                cloneObjPrototype.transform.parent = slots[i].transform;
                cloneObjPrototype.transform.localScale = Vector3.one*0.95f;
            }

        }
                   
    }
    
    private void OnApplicationQuit() {
        if(spawnType==SpawnType.resistor){
            BoxSpawn item = new(){items=itemSpawn.ToDictionary(i=>i.transform.parent,v=>v.GetComponent<resistor>().Prop)};
            string pathname = FindPath(this.transform);            
            /*string jsonInputData = JsonConvert.SerializeObject(new Dictionary<string,BoxSpawn>(){{pathname,item}});*/
            
        }
    }
    
    string FindPath(Transform t){
        string path = t.name;

        while (t.parent != null) {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
}
