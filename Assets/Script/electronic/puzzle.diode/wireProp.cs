using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wireProp : MonoBehaviour
{
    public float voltage;
    public virtual float getVoltage(){
        return voltage;
    }
}
