using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class diodeMachine : MonoBehaviour
{
    List<WaveGenerator> gen;
    private void Awake() {
        gen = FindObjectsOfType<WaveGenerator>().Where(gen=>gen.m_IsVoltageGenerator == true).ToList();
        Debug.Log(gen.Count);
        
    }
}
