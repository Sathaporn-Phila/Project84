using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Text.RegularExpressions;


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
            case SpawnType.gate:
                Prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
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
        GameObject obj2Clone = prefab2Spawn(spawnType);
        List<GameObject> slots = query.queryByName(this.gameObject,regex);
        foreach(GameObject slot in slots){
            GameObject cloneObj = Instantiate(obj2Clone,slot.transform.position+Vector3.up,obj2Clone.transform.rotation);
            cloneObj.transform.parent = slot.transform;
            cloneObj.AddComponent<resistor>();
            itemSpawn.Add(cloneObj);
        }
    } 
    private void Start(){
        
    }

}
