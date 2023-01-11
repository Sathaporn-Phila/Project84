using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using System.Linq;
using TMPro;
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
    public class Sticker {
        public string colorName;
        public Color color;
        public float Value;
        public Sticker(string name,Color col,float val ){
            colorName = name;
            color = col;
            Value = val;
        }    
    }
    public class Attribute {
        public List<Sticker> allSticker;
        public float val = 0;
        private int width=1024,height=1024;
        private Dictionary<string,Sticker> oneOfthreeBar,fourthBar;
        public Attribute(Dictionary<string,Sticker> stickers){//ค่า r
            allSticker = new List<Sticker>();
            //allSticker = stickers;
            oneOfthreeBar = stickers.Where(x=>x.Key!="gold"&&x.Key!="silver").ToDictionary(c => c.Key, c => c.Value);
            fourthBar = stickers.Where(x=>!oneOfthreeBar.Contains(x)).ToDictionary(c => c.Key, c => c.Value);
            Debug.Log(oneOfthreeBar.Count);

            //NativeArray<JobHandle> jobs = new NativeArray<JobHandle>();
            for(int bar=0;bar<4;bar++){
                if(bar<3){
                    Sticker stickerProp = oneOfthreeBar.ElementAt(Random.Range(0,oneOfthreeBar.Count-1)).Value;   
                    val = (bar<2)? (val+stickerProp.Value*Mathf.Pow(10,bar)) : (val*Mathf.Pow(10,stickerProp.Value));
                    allSticker.Add(stickerProp);
                }else{
                    Sticker stickerProp = fourthBar.ElementAt(Random.Range(0,fourthBar.Count-1)).Value;
                    allSticker.Add(stickerProp);   
                }
            }
            
        }
        public List<Vector4> getAllColor(){
            return allSticker.Select(obj=>new Vector4(obj.color.r,obj.color.g,obj.color.b,obj.color.a)).ToList();
        }
    }
    Attribute prop;
    private Dictionary<string,Sticker> stickerData = new Dictionary<string, Sticker>(){
        {"black",new Sticker("black",Color.black,0)},
        {"brown",new Sticker("brown",new Color((float)210/255,(float)105/255,(float)30/255),1)},
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
    void Awake(){
        prop = new Attribute(stickerData);
        /*foreach(Sticker attr in prop.allSticker){
            Debug.Log(attr.colorName+new Vector4(attr.color.r,attr.color.g,attr.color.b));
        }*/
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Mpb.SetVectorArray("_ColorArray",prop.getAllColor());
        for(int i=0;i<renderer.materials.Length;i++){
            //if(renderer.materials[i].HasProperty("_ColorArray")){
                renderer.SetPropertyBlock(Mpb,i);
            //}
            //}
        }

    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
