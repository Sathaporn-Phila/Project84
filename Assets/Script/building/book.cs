using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class book : MonoBehaviour
{
    private Texture pageTexture;
    private List<GameObject> pages;
    string pattern = @"\bpage\.\d*$|\bpage$";
    Regex rgx;
    private List<GameObject> queryByName(GameObject root,Regex pattern){
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
 
    private void Searcher(List<GameObject> list,GameObject root,Regex pattern)
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
    private void SetMaterial(GameObject obj){
        Renderer meshRenderer = obj.GetComponent<Renderer>();
        meshRenderer.material.SetTexture("_BaseMap",pageTexture);
    }
    
    void Awake(){
        pageTexture = Resources.Load<Texture>("furniture/book/page");
        rgx = new Regex(pattern);
        pages = queryByName(this.gameObject,rgx);
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
