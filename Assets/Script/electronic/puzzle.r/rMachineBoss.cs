using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rMachineBoss : rMachine
{
    float atk = 1;
    EnemyHealth enemyHealth;
    private void Awake() {
        enemyHealth = GameObject.FindGameObjectWithTag("boss").GetComponent<EnemyHealth>();
    }
    public override void action(){
        enemyHealth.HP -= 10;
        box.respawn();
    }
    public override void refresh(){
        slotGroups.ForEach(item=>item.slotObj.GetComponent<slot>().TurnLight(item.led,false));
        slotGroups.Clear();
        //await Task.Run(()=>this.transform.parent.Find("box").GetComponent<Box>().spawn());
        matchSlotGroup();
    }
}
