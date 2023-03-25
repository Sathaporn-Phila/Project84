using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;
using System.Linq;
using MongoDB.Bson;

class resistorBox : RealmObject
{
    [PrimaryKey]
    private string path{get;set;}
    public IList<resistorData> inside{get;}
    public resistorBox(string pathname){
        path = pathname;
        inside = new List<resistorData>();
    }
    public resistorBox(){}
}
class resistorData:EmbeddedObject{
    
    public string BoxPath{get;set;}
    public string curPath{get;set;}
    public TransformModel transformModel{get;set;}
    public Attribute attribute{get;set;}
    public resistorData(string parentPath,Transform transform,string currentPath){
        BoxPath = parentPath;
        transformModel = new TransformModel(){
            Position = transform.position,Rotation = transform.rotation,Scale = transform.localScale
        };
        curPath = currentPath;
    }
    public resistorData(){}

}
class GateBox : RealmObject {
    [PrimaryKey]
    public string path {get;set;}
    public IList<GateData> allGate{get;}

    public GateBox(string pathName){
        path = pathName;
    }
    private GateBox(){}
}
class GateData : EmbeddedObject {
    public string name{get;set;}
    public TransformModel transformModel{get;set;}
    public GateData(string objName,Transform transform){
        name = objName;
        transformModel = new TransformModel(){
            Position = transform.position,Rotation = transform.rotation,Scale = transform.localScale
        };
    }
    private GateData(){}
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
        public IList<Sticker> allSticker{get;}
        public double val{get;set;}
        private int width=1024,height=1024;
        private Dictionary<string,Sticker> oneOfthreeBar,fourthBar;
        public Attribute(Dictionary<string,Sticker> stickers){//ค่า r
            
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

class CardData : RealmObject{
    [PrimaryKey]
    public string path{get;set;}
    public bool isAnimated{get;set;}
    public TransformModel transformModel{get;set;}
    public CardData(string pathName,Transform transform){
        path = pathName;
        transformModel = new TransformModel(){
            Position = transform.position,Rotation = transform.rotation,Scale = transform.localScale
        };
    }
    public CardData(){
        
    }
}