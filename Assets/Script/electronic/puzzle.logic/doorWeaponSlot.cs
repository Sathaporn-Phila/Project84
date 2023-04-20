using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.VFX;
public class doorWeaponSlot : DoorSlot
{
    public List<normalGen> allgen = new();
    public VisualEffect vfx;
    public GradientEffect gradientEffect;
    public AudioSource audiosound;
    public AudioClip laserBeam;
    EnemyHealth enemyHealth;
    public override void setInitValue(){
        doorOpen = this.gameObject.AddComponent<doorOpenWeapon>();
        doorClose = this.gameObject.AddComponent<doorAnimClose>();
        wireQuery = this.gameObject.AddComponent<wireQuery>();
        skinnedMesh = this.gameObject.GetComponent<SkinnedMeshRenderer>();    
        currerntState = doorClose;
        currerntState.Enter(this.skinnedMesh);
        enemyHealth = GameObject.FindGameObjectWithTag("boss").GetComponent<EnemyHealth>();
        vfx = GetComponent<VisualEffect>();
        gradientEffect = new GradientEffect(Color.Lerp(new Color(255, 168, 54)*Random.Range(1.0f,2.5f),new Color(255, 79, 0)*Random.Range(1.0f,2.5f),Random.Range(0f,1.0f)),Color.white);
        vfx.SetGradient("Gradient",gradientEffect.gradient);
    }
    public override void setSlot()
    {
        query = this.gameObject.AddComponent<Query>();
        foreach(GameObject collider in query.queryByName(this.transform.parent.parent.gameObject,new Regex(@"\bcollider"))){
            allSlot.Add(collider.GetComponent<GateSlot>());
        }
    }
    public virtual void Update() {
        Ray ray = new Ray(transform.position,transform.TransformDirection(Vector3.forward));
        currerntState.UpdateState(this,skinnedMesh,wireQuery.findWireHit(ray,2,0));
        Vector3 relativePos = vfx.transform.InverseTransformDirection(enemyHealth.transform.position-this.transform.position)+Vector3.up;

        vfx.SetVector3("targetPosition",relativePos);
    }
    public void reset(){
        allgen.ForEach(item=>item.reset());
    }
}
