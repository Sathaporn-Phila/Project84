using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

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
                int numSpawn = Random.Range(1,5);
                GameObject cloneObjPrototype = Instantiate(obj2Clone,slot.transform.position+Vector3.up,slot.transform.rotation);
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
            }
        }else{
            List<GameObject> GatePrefab = Resources.LoadAll<GameObject>("Prefabs/electronic/gate.machine.module").Where(obj=>Regex.IsMatch(obj.name,@"\bgate")).ToList();
            Debug.Log(GatePrefab.Count);
            for(int i=0;i<GatePrefab.Count;i++){
                GameObject cloneObjPrototype = Instantiate(GatePrefab[i],slots[i].transform.position+Vector3.up,slots[i].transform.rotation);
                cloneObjPrototype.transform.parent = slots[i].transform;
            }

        }
            
            
    }
    
}
