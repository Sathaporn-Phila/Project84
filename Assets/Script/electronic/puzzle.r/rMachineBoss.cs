using UnityEngine;
using UnityEngine.VFX;

public class rMachineBoss : rMachine
{
    float atk = 1;
    EnemyHealth enemyHealth;
    VisualEffect vfx;
    GradientEffect gradientEffect;
    private void Awake() {
        enemyHealth = GameObject.FindGameObjectWithTag("boss").GetComponent<EnemyHealth>();
        vfx = this.transform.Find("resistor.machine").GetComponent<VisualEffect>();
        gradientEffect = new GradientEffect(Color.Lerp(new Color(230,215,255)*Random.Range(1.0f,2.5f),Color.magenta*Random.Range(1.0f,2.5f),Random.Range(0f,1.0f)),Color.white);
        vfx.SetGradient("Gradient",gradientEffect.gradient);
    }
    public override void action(){
        enemyHealth.HP -= 10;
        vfx.SendEvent("PlayLaserBeam");
        box.respawn();
    }
    public override void Start() {
        box = this.transform.parent.Find("box").GetComponent<Box>();
        matchSlotGroup();
        
        //unlockCard();
    }
    private void Update() {
        Vector3 relativePos = transform.InverseTransformDirection(this.transform.position-enemyHealth.transform.position);
        vfx.SetVector3("targetPosition",relativePos);
        
    }
    public override void refresh(){
        slotGroups.ForEach(item=>item.slotObj.GetComponent<slot>().TurnLight(item.led,false));
        slotGroups.Clear();
        //await Task.Run(()=>this.transform.parent.Find("box").GetComponent<Box>().spawn());
        matchSlotGroup();
    }
    
}
