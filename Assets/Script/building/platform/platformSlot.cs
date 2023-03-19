using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformSlot : MonoBehaviour
{
    memoryPlatform platform;
    bool isAdd = false;
    memorybox memorybox;

    private void Awake() {
        platform = this.transform.parent.GetComponent<memoryPlatform>();
        GameObject obj = Resources.Load<GameObject>("Prefabs/electronic/gate.machine.module/memorybox");
        GameObject cloneObj = Instantiate(obj,transform.position + Vector3.up*10f,transform.rotation);
        cloneObj.GetComponent<memorybox>().fresnelColor = new Color(Random.value * 2f, Random.value * 2f, Random.value * 2f, 1f);
    }
    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent<memorybox>(out memorybox membox)&& !isAdd){
            other.transform.position = this.transform.position;
            platform.AddMembox(membox);
            isAdd = true;
            memorybox = membox;
            memorybox.transform.SetParent(this.transform.parent);
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.TryGetComponent<memorybox>(out memorybox membox)&& !isAdd){
            memorybox.transform.SetParent(membox.formerParent);
            memorybox = null;
            platform.RemoveMembox(membox);
            isAdd = false;
            other.GetComponent<Rigidbody>().useGravity = true;
        }
    }
    private void OnTriggerStay(Collider other) {
        if(other.TryGetComponent<memorybox>(out memorybox membox)){
            memorybox.SetBezierPoint();
        }
    }
}
