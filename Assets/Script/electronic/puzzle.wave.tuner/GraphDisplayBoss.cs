using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphDisplayBoss : GraphDisplay
{
    EnemyHealth enemyHealth;
    public override void setEnemy() {
        enemyHealth = GameObject.FindGameObjectWithTag("boss").GetComponent<EnemyHealth>();
    }
    public override void checkSameVal(){
        if(originGraph.material.GetFloat("_Amplitude") == yourGraph.material.GetFloat("_Amplitude") && originGraph.material.GetFloat("_Position") == yourGraph.material.GetFloat("_Position")){
            action();
        }
    }
    private void action(){
        enemyHealth.HP -= 10;
        reset();
    }
    private void reset(){

        originGraph.material.SetFloat("_Amplitude",Random.Range(-5,5));

        float pos = Random.Range(0,360);
        originGraph.material.SetFloat("_Position",pos-pos%15f);
    }
}
