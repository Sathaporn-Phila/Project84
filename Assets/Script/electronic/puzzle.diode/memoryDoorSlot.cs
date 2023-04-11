using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class memoryDoorSlot : DoorSlot
{
    private void Update() {
        currerntState.UpdateState(skinnedMesh);
    }
}
