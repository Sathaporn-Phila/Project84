using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rfidDoor : doorRoom
{
    private void Update() {
        StartCoroutine("internalDoorCloseTime");
    }
    IEnumerator internalDoorCloseTime(){
        yield return new WaitForSeconds(5f);
        if(currentState == doorOpen){
            base.Close();
        }
    }
}
