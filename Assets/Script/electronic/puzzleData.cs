using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;
using MongoDB.Bson;

class resistorBox : RealmObject
{
    [PrimaryKey]
    private string path{get;set;}
    public IList<resistorData> inside{get;}
    public resistorBox(string pathname){
        path = pathname;
        
    }
    public resistorBox(){}
}
class resistorData:EmbeddedObject{
    [PrimaryKey]
    public ObjectId id { get; } = ObjectId.GenerateNewId();
    public string BoxPath{get;set;}
    public string curPath{get;set;}
    public TransformModel transformModel{get;set;}
    public resistor.Attribute attribute{get;set;}
    public resistorData(string parentPath,Transform transform,string currentPath,resistor.Attribute attr){
        BoxPath = parentPath;
        transformModel = new TransformModel(){
            Position = transform.position,Rotation = transform.rotation,Scale = transform.localScale
        };
        curPath = currentPath;
        attribute = attr;
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
    public string name;
    public TransformModel transformModel;
    public GateData(string objName,Transform transform){
        name = objName;
        transformModel = new TransformModel(){
            Position = transform.position,Rotation = transform.rotation,Scale = transform.localScale
        };
    }
    private GateData(){}
}