using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.XR.Interaction.Toolkit;



public class resistor : MonoBehaviour
{
    Query query;
    MaterialPropertyBlock mpb;
    public MaterialPropertyBlock Mpb {
        get {
            mpb = mpb == null? new MaterialPropertyBlock():mpb;
            return mpb;
        }
    }
    
    private Attribute prop;
    public Attribute Prop{
        get{return prop;}
        set{prop=value;}
    }
    private Dictionary<string,Sticker> stickerData = new Dictionary<string, Sticker>(){
        {"black",new Sticker("black",Color.black,0)},
        {"brown",new Sticker("brown",new Color((float)220/255,(float)105/255,(float)30/255),1)},
        {"red",new Sticker("red",Color.red,2)},
        {"orange",new Sticker("orange",new Color((float)255/255,(float)165/255,0),3)},
        {"yellow",new Sticker("yellow",Color.yellow,4)},
        {"green",new Sticker("green",Color.green,5)},
        {"blue",new Sticker("blue",Color.blue,6)},
        {"magenta",new Sticker("magenta",Color.magenta,7)},
        {"grey",new Sticker("grey",Color.grey,8)},
        {"white",new Sticker("white",Color.white,9)},
        {"gold",new Sticker("gold",new Color((float)255/255,(float)215/255,0),0.05f)},
        {"silver",new Sticker("silver",new Color((float)192/255,(float)192/255,(float)192/255),0.1f)},
    };
    public void SetColor(){
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Mpb.SetVectorArray("_ColorArray",prop.getAllColor());

        for(int i=0;i<renderer.materials.Length;i++){
            renderer.SetPropertyBlock(Mpb,i);
            
        }
    }
    void Awake(){
        prop = new Attribute(stickerData);
        this.gameObject.GetComponent<XRGrabInteractable>().interactionManager = GameObject.Find("XR Interaction Manager").GetComponent<XRInteractionManager>();
        SetColor();
        
        

    }
}
