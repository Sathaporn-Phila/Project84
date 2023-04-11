using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Nand : Gate
{
    // Start is called before the first frame update
    public override float calVoltage(float input1,float input2)
    {
        input1 = Mathf.InverseLerp(0,5,input1);
        input2 = Mathf.InverseLerp(0,5,input2);
        return this.baseVoltage*Convert.ToInt32(!(Convert.ToBoolean((int)input1) || Convert.ToBoolean((int)input2)));
    }
}
