using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;
class PlayerData : RealmObject
{
    [PrimaryKey]
    public string player{get;set;}
    public TransformModel transformModel{get;set;}
    public PlayerData(){}
    public PlayerData(string val,Transform t){
        player = val;
        transformModel = new TransformModel(){
            Position = t.position,Rotation = t.rotation,Scale = t.localScale
        };
    }
    
}
public class BoxSpawn{
    public Dictionary<Transform,resistor.Attribute> items;   
}

public class Vector3Model : EmbeddedObject
{
    // Casing of the properties here is unusual for C#,
    // but consistent with the Unity casing.
    private float x { get; set; }
    private float y { get; set; }
    private float z { get; set; }

    public Vector3Model(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }
    public Vector3Model(){}
    

    public Vector3 ToVector3() => new Vector3(x, y, z);
}

public class Vector4Model : EmbeddedObject
{
    // Casing of the properties here is unusual for C#,
    // but consistent with the Unity casing.
    private float x { get; set; }
    private float y { get; set; }
    private float z { get; set; }
    private float w { get; set; }

    public Vector4Model(Vector4 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
        w = vector.w;
    }
    public Vector4Model(){}
    public Vector4 ToVector4() => new Vector4(x, y, z, w);
    public Quaternion ToQuarternion() => new Quaternion(x, y, z, w);

}

public class TransformModel : EmbeddedObject
{
    [MapTo("Position")]
    private Vector3Model _Position { get; set; }
    [MapTo("Rotation")]
    private Vector4Model _Rotation { get; set; }
    [MapTo("Scale")]
    private Vector3Model  _scale { get; set; }
    public Vector3 Position
    {
        get => _Position?.ToVector3() ?? Vector3.zero;
        set => _Position = new Vector3Model(value);
    }
    public Quaternion Rotation
    {
        get => _Rotation?.ToQuarternion() ?? new Quaternion(0,0,0,0);
        set => _Rotation = new Vector4Model(new Vector4(value.x,value.y,value.z,value.w));
    }
    public Vector3 Scale
    {
        get => _scale?.ToVector3() ?? Vector3.zero;
        set => _scale = new Vector3Model(value);
    }
    
}