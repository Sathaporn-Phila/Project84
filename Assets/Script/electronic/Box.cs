using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using Realms;


public class Box: MonoBehaviour
{
    protected Query query;
    private Realm _realm;

    private resistorBox resistorBox;
    private GateBox gateBox;

    List<GameObject> itemSpawn = new List<GameObject>();
    
    public enum SpawnType {resistor,gate,diode}
    string pattern = @"\bmini-box\.\d*$",path;
    /*[SerializeField]*/public SpawnType spawnType = new SpawnType();
    public List<GameObject> getSpawnObject(){
        return itemSpawn;
    }
    bool WanttoQuit(){
        this.saveObjectPosition();
        return true;
    }
    bool hasSpawned;
    GameObject prefab2Spawn(SpawnType type){
        GameObject Prefab;
        switch(type){
            case SpawnType.resistor:
                Prefab = (GameObject)Resources.Load("Prefabs/electronic/resistor");
                Prefab.transform.localScale = new Vector3((float)0.7,(float)0.7,(float)0.7);
                Prefab.transform.rotation = Quaternion.Euler(0,0,0);

                resistorBox = _realm.Find<resistorBox>(path);
                if(resistorBox is null){
                    hasSpawned = false;
                    _realm.Write(()=>{
                        resistorBox = _realm.Add(new resistorBox(path));
                    });
                }else{
                    hasSpawned = true;
                }
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
        Application.wantsToQuit += WanttoQuit;
        _realm = Realm.GetInstance();

        path = this.FindPath(this.transform);
        query = this.gameObject.AddComponent<Query>();
        
        Regex regex = new Regex(pattern);
        List<GameObject> slots = query.queryByName(this.gameObject,regex);
        

        if(spawnType == SpawnType.resistor || spawnType == SpawnType.diode){
            GameObject obj2Clone = prefab2Spawn(spawnType);
            

            foreach(GameObject slot in slots){

                int numSpawn = Random.Range(1,2);
                
                GameObject cloneObjPrototype = Instantiate(obj2Clone,slot.transform.position+Vector3.up+0.1f*transform.TransformDirection(Vector3.right),slot.transform.rotation);
                cloneObjPrototype.transform.parent = slot.transform;

                string parentPath = this.FindPath(slot.transform);

                
                if(spawnType == SpawnType.resistor){
                    resistor rProp = cloneObjPrototype.AddComponent<resistor>();

                    //เคย spawn ครั้งแรกไปแล้ว
                    if(!hasSpawned){
                        for(int i=0;i<numSpawn;i++){

                            GameObject cloneObj= Instantiate(obj2Clone,slot.transform.position+2*i*Vector3.up,slot.transform.rotation);
                            cloneObj.transform.parent = slot.transform;
                            resistor rDup = cloneObj.AddComponent<resistor>();
                            rDup.Prop = rProp.Prop;
                            rDup.SetColor();

                            _realm.Write(()=>{    
                                resistorBox.inside.Add(new resistorData(parentPath,cloneObj.transform,parentPath,rDup.Prop));
                            });
                              
                        }
                    }else{ 
                        resistorBox.inside.Where(r=>r.BoxPath==parentPath).ToList().ForEach((r)=>{
                            GameObject parentObj = GameObject.Find(r.curPath);
                            GameObject cloneObj = Instantiate(obj2Clone,r.transformModel.Position,r.transformModel.Rotation);
                            cloneObj.transform.parent = parentObj.transform;
                            resistor rDup = cloneObj.AddComponent<resistor>();
                            rDup.Prop = r.attribute;
                            rProp.Prop = r.attribute;
                            rDup.SetColor();
                    });
                        
                    }
                }
                itemSpawn.Add(cloneObjPrototype);
                cloneObjPrototype.SetActive(false);
            }

            
            
            

        }else{

            List<GameObject> GatePrefab = Resources.LoadAll<GameObject>("Prefabs/electronic/gate.machine.module").Where(obj=>Regex.IsMatch(obj.name,@"\bgate")).ToList();

            gateBox = _realm.Find<GateBox>(path);

            if(gateBox is null){
                _realm.Write(()=>{
                    gateBox = _realm.Add(new GateBox(path));
                });
            }
                
            for(int i=0;i<GatePrefab.Count;i++){
                GameObject cloneObjPrototype = Instantiate(GatePrefab[i],slots[i].transform.position+Vector3.up*0.1f,slots[i].transform.rotation);
                if(gateBox.allGate.Filter($"GateData.name == {cloneObjPrototype.name}").ToList().Count == 0){
                    _realm.Write(()=>{
                        gateBox.allGate.Add(new GateData(cloneObjPrototype.name,cloneObjPrototype.transform));
                        });
                }
                cloneObjPrototype.transform.parent = slots[i].transform;
                cloneObjPrototype.transform.localScale = Vector3.one*0.95f;
            }

        }
                   
    }
    
    
    private void saveObjectPosition(){
        if(spawnType == SpawnType.resistor){
            _realm.Write(()=>{
                resistorBox.inside.ToList().ForEach((r)=>{
                    r.transformModel.Position = this.transform.position;
                    r.transformModel.Rotation = this.transform.rotation;
                });
            });
        }else if(spawnType == SpawnType.gate){
            _realm.Write(()=>{
                
            });
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
