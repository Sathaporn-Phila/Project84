using UnityEngine;
using UnityEngine.VFX;
public class GraphDisplayBoss : GraphDisplay
{
    EnemyHealth enemyHealth;
    VisualEffect vfx;
    GradientEffect gradientEffect;
    public AudioSource laserBeam;
    public override void setEnemy() {
        enemyHealth = GameObject.FindGameObjectWithTag("boss").GetComponent<EnemyHealth>();
        vfx = GetComponent<VisualEffect>();
        gradientEffect = new GradientEffect(Color.Lerp(new Color(198, 230, 251)*Random.Range(1.0f,2.5f),new Color(0, 0, 128)*Random.Range(1.0f,2.5f),Random.Range(0f,1.0f)),Color.white);
        vfx.SetGradient("Gradient",gradientEffect.gradient);
    }
    public override void checkSameVal(){
        if(originGraph.material.GetFloat("_Amplitude") == yourGraph.material.GetFloat("_Amplitude") && originGraph.material.GetFloat("_Position") == yourGraph.material.GetFloat("_Position")){
            action();
        }
    }
    private void action(){
        enemyHealth.HP -= 10;
        vfx.SendEvent("PlayLaserBeam");
        laserBeam.Play();
        reset();
    }
    private void reset(){

        originGraph.material.SetFloat("_Amplitude",Random.Range(-5,5));

        float pos = Random.Range(0,360);
        originGraph.material.SetFloat("_Position",pos-pos%15f);
    }
    private void Update() {
        Vector3 relativePos = transform.InverseTransformDirection(this.transform.position-enemyHealth.transform.position);
        vfx.SetVector3("targetPosition",relativePos);
    }
}
