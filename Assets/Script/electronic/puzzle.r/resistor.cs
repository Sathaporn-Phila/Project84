using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;
using Realms;

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
    public class Sticker : EmbeddedObject  {
        public string colorName{get;set;}
        public Vector4Model color{get;set;}
        public float Value{get;set;}
        public Sticker(string name,Color col,float val ){
            colorName = name;
            color = new Vector4Model(new Vector4(col.r,col.g,col.b,col.a));
            Value = val;
        }
        public Sticker(){}    
    }
    public class Attribute : EmbeddedObject {
        public IList<Sticker> allSticker {get;}
        public double val{get;set;}
        private int width=1024,height=1024;
        private Dictionary<string,Sticker> oneOfthreeBar,fourthBar;
        public Attribute(Dictionary<string,Sticker> stickers){//ค่า r
            //allSticker = stickers;
            oneOfthreeBar = stickers.Where(x=>x.Key!="gold"&&x.Key!="silver").ToDictionary(c => c.Key, c => c.Value);
            fourthBar = stickers.Where(x=>!oneOfthreeBar.Contains(x)).ToDictionary(c => c.Key, c => c.Value);

            //NativeArray<JobHandle> jobs = new NativeArray<JobHandle>();
            for(int bar=0;bar<4;bar++){
                if(bar<3){
                    Sticker stickerProp = oneOfthreeBar.ElementAt(Random.Range(0,oneOfthreeBar.Count-1)).Value;   
                    val = (bar<2)? (val+stickerProp.Value*Mathf.Pow(10,1-bar)) : (val*Mathf.Pow(10,stickerProp.Value));
                    allSticker.Add(stickerProp);
                }else{
                    Sticker stickerProp = fourthBar.ElementAt(Random.Range(0,fourthBar.Count-1)).Value;
                    allSticker.Add(stickerProp);   
                }
            }
            
        }

        public Attribute(){}
        public List<Vector4> getAllColor(){
            return allSticker.Select(obj=>obj.color.ToVector4()).ToList();
        }
        public string findPrefixSymbol(int val){
            if(val==3){
                return "K";
            }else if(val==6){
                return "M";
            }else if(val==9){
                return "G";
            }else{
                return "";
            }
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
            //if(renderer.materials[i].HasProperty("_ColorArray")){
                renderer.SetPropertyBlock(Mpb,i);
            //}
            //}
        }
    }
    void Awake(){
        prop = new Attribute(stickerData);
        this.gameObject.GetComponent<XRGrabInteractable>().interactionManager = GameObject.Find("XR Interaction Manager").GetComponent<XRInteractionManager>();
        SetColor();
        /*foreach(Sticker attr in prop.allSticker){
            Debug.Log(attr.colorName+new Vector4(attr.color.r,attr.color.g,attr.color.b));
        }*/
        

    }
}
