using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class safeboxMachine : safeBoxDoor
{
    // Start is called before the first frame update
    EnemyHealth enemyHealth;
    
    private void Awake() {
        safeboxPassword = new Password();
        meshRenderer = GetComponent<MeshRenderer>();
        Mpb.SetFloatArray("_IntArray",new List<float>(){-1f,-1f,-1f,-1f,-1f,-1f,-1f,-1f});
        meshRenderer.SetPropertyBlock(Mpb);
        enemyHealth = GameObject.FindGameObjectWithTag("boss").GetComponent<EnemyHealth>();
    }
    public override void UpdateState(string input)
    {
        if(input == "OK"){
            if(safeboxPassword.get() == safeboxPassword.current){
                action();   
            }
        }else{
            safeboxPassword.append(input);
        }
        mapColor();
    }
    private void mapColor(){
        float n;
        List<float> binaryInput = safeboxPassword.current.Select(c => float.TryParse(c.ToString(), out n) ? n : 0).ToList();
        List<float> emptyInput = Enumerable.Repeat(-1f,8-binaryInput.Count).ToList();
        binaryInput.AddRange(emptyInput);
        Mpb.SetFloatArray("_IntArray",binaryInput);
        meshRenderer.SetPropertyBlock(Mpb);
    }
    private void action(){
        enemyHealth.HP -= 10;
        reset();
    }
    private void reset(){
        safeboxPassword.current = "";
        safeboxPassword.randomPassword();
        this.transform.Find("paper").GetComponent<paper>().reset();
        mapColor();
    }
    public override void Update(){}
}
