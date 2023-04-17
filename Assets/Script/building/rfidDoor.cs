using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rfidDoor : doorRoom
{
    private void Update() {
        if(currentState == doorOpen){
            StartCoroutine("internalDoorCloseTime");
        }
    }
    IEnumerator internalDoorCloseTime(){
        yield return new WaitForSeconds(5f);
        Close();
    }
}
