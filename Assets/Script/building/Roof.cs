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
    MaterialPropertyBlock mpb;
    string pattern = @"\bcyberRoof";
    // Start is called before the first frame update
    static readonly int shaderProperties = Shader.PropertyToID("_EmissionColor");
    public MaterialPropertyBlock Mpb {
        get {
            mpb = mpb == null? new MaterialPropertyBlock():mpb;
            return mpb;
        }
    }
    private void SetMaterial(GameObject obj){
        Debug.Log(obj.name);
        Renderer meshRenderer = obj.GetComponent<Renderer>();
        
        Mpb.SetColor(shaderProperties,colorPattern);
        meshRenderer.SetPropertyBlock(Mpb);
    }
    void Awake(){
        query = this.gameObject.AddComponent<Query>();
        rgx = new Regex(pattern);
        roofs = query.queryByName(this.gameObject,rgx);
        foreach(GameObject item in roofs){
            SetMaterial(item);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
}
