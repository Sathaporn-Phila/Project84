using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class book : MonoBehaviour
{
    Query query;
    private Texture pageTexture;
    private List<GameObject> pages;
    string pattern = @"\bpage\.\d*$|\bpage$";
    Regex rgx;
 
    
    private void SetMaterial(GameObject obj){
        Renderer meshRenderer = obj.GetComponent<Renderer>();
        meshRenderer.material.SetTexture("_BaseMap",pageTexture);
    }
    
    void Awake(){
        query = this.gameObject.AddComponent<Query>();
        pageTexture = Resources.Load<Texture>("furniture/book/page");
        rgx = new Regex(pattern);
        pages = query.queryByName(this.gameObject,rgx);
        foreach(GameObject item in pages){
            SetMaterial(item);
        }
        
    }
    void Start(){
    }
    void Update()
    {
        
    }
}
