using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rMachineBoss : rMachine
{
    float atk = 1;
    public override void action(){
        //boss.hp -= 1;
        box.respawn();
    }
    public override void refresh(){
        slotGroups.Clear();
        //await Task.Run(()=>this.transform.parent.Find("box").GetComponent<Box>().spawn());
        matchSlotGroup();
    }
}
