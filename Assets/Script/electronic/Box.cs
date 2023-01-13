using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

public class Box: MonoBehaviour
{
    Query query;
    public enum SpawnType {
        resistor,gate,diode
    }
    string pattern = @"\bmini-box\.\d*$";
    [SerializeField]SpawnType spawnType = new SpawnType();
    private void Awake() {
        query = this.gameObject.AddComponent<Query>() as Query;
        if(spawnType == SpawnType.resistor){
            Regex regex = new Regex(pattern);
            GameObject resistorPrefab = (GameObject)Resources.Load("Prefabs/electronic/resistor");
            resistorPrefab.transform.localScale = new Vector3((float)0.7,(float)0.7,(float)0.7);
            resistorPrefab.transform.Rotate(Quaternion.Euler(0,90,0).eulerAngles);
            List<GameObject> slots = query.queryByName(this.gameObject,regex);
            foreach(GameObject slot in slots){
                GameObject resitorClone = Instantiate(resistorPrefab,slot.transform.position+Vector3.up,slot.transform.rotation);
            }
        }
    } 
    private void Start(){
        
    }

}
