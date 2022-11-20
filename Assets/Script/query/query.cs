using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class Query : MonoBehaviour{
    private static Query _Instance;

    public static Query Instance { get { return _Instance; } }

    void Awake(){
    if(_Instance != null && _Instance != this){
        Destroy(this.gameObject);
            
    }
    else{
        _Instance = this;
    }
    }
    public List<GameObject> queryByName(GameObject root,Regex pattern){
        List<GameObject> result = new List<GameObject>();
        if (root.transform.childCount > 0)
        {
            foreach (Transform VARIABLE in root.transform)
            {
                Searcher(result,VARIABLE.gameObject,pattern);
            }
        }
        return result;
    }

    public void Searcher(List<GameObject> list,GameObject root,Regex pattern)
    {

        if (root.transform.childCount > 0)
        {
            foreach (Transform VARIABLE in root.transform)
            {
                Searcher(list,VARIABLE.gameObject,pattern);
            }
        }
        else{

            if(pattern.IsMatch(root.name)){
                list.Add(root);
            }
        }
    }
}
