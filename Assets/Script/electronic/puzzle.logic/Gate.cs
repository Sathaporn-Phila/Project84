using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gate : MonoBehaviour
{
   public float baseVoltage = 5;
   public abstract float getVoltage(float input1,float input2);

}
