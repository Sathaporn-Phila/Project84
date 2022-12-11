using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class resistor : MonoBehaviour
{
    public class Sticker {
        public string colorName{get;set;}
        public Color color{get;set;}
        public float Value{get;set;}
        public Sticker(string name,Color col,float val ){
            colorName = name;
            color = col;
            Value = val;
        }    
    }
    private class Attribute {
        public List<Sticker> allSticker{get;set;}
        public float val = 0;
        public Attribute(List<Sticker> stickers){
            allSticker = stickers;
            for(int i=0;i<allSticker.Count-1;i++){
                val = (i<2)? (val+allSticker[i].Value*Mathf.Pow(10,i)) : (val*Mathf.Pow(10,allSticker[i].Value));
            }
        }
    }
    private Dictionary<string,Sticker> oneOfthreeBar,fourthBar;
    private Dictionary<string,Sticker> stickerData = new Dictionary<string, Sticker>(){
        {"black",new Sticker("black",Color.black,0)},
        {"brown",new Sticker("brown",new Color(210,105,30),1)},
        {"red",new Sticker("red",Color.red,2)},
        {"orange",new Sticker("orange",new Color(255,165,0),3)},
        {"yellow",new Sticker("yellow",Color.yellow,4)},
        {"green",new Sticker("green",Color.green,5)},
        {"blue",new Sticker("blue",Color.blue,6)},
        {"magenta",new Sticker("magenta",Color.magenta,7)},
        {"grey",new Sticker("grey",Color.grey,8)},
        {"white",new Sticker("white",Color.white,9)},
        {"gold",new Sticker("gold",new Color(255,215,0),0.05f)},
        {"silver",new Sticker("silver",new Color(255,215,0),0.1f)},
    };
    
    void Awake(){
        oneOfthreeBar = stickerData;
        oneOfthreeBar = oneOfthreeBar.Where(x=>x.Key!="gold"||x.Key!="silver").ToDictionary(c => c.Key, c => c.Value);
        fourthBar = stickerData.Where(x=>!oneOfthreeBar.Contains(x)).ToDictionary(c => c.Key, c => c.Value);

        
        
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
