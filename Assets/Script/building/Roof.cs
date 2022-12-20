using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


public class Roof : MonoBehaviour
{
    [ColorUsageAttribute(false, true)] public Color colorPattern;
    Query query;
    Regex rgx;
    private List<GameObject> roofs;
    string pattern = @"\bcyberRoof";
    // Start is called before the first frame update
    private void SetMaterial(GameObject obj){
        Renderer meshRenderer = obj.GetComponent<Renderer>();
        meshRenderer.material.SetColor("_EmissionColor",colorPattern);
    }
    void Awake(){
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        query = this.gameObject.AddComponent<Query>();
        rgx = new Regex(pattern);
        roofs = query.queryByName(this.gameObject,rgx);
        foreach(GameObject item in roofs){
            SetMaterial(item);
        }
    }
}
