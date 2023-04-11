using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class diodeMachine : MonoBehaviour
{
    List<WaveGenerator> gen;
    memoryDoorSlot doorSlot;
    float diffSignal;
    private void Awake() {
        gen = FindObjectsOfType<WaveGenerator>().Where(gen=>gen.m_IsVoltageGenerator == true).ToList();
        doorSlot = this.transform.parent.Find("memory.storage/wire.slot.withdoor.t2").GetComponent<memoryDoorSlot>();        
    }
    private void FixedUpdate() {
        if (gen.Any()) {
            diffSignal = gen.Aggregate(1.0f, (acc, next) => acc * next.voltage);
        }else {
            diffSignal = 0.0f; // or any other default value
        }

        if(diffSignal<0){
            doorSlot.changeState(doorSlot.doorOpen);
        }
    }
}
