using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class teleportPoint : MonoBehaviour
{
    public AudioSource audiosound;
    public AudioClip teleportsound;
    // Start is called before the first frame update
    VisualEffect vfx;
    [SerializeField]bool isBlackHole;
    void Awake(){
        audiosound.PlayOneShot(teleportsound);
        vfx = GetComponent<VisualEffect>();
        if(isBlackHole){
            vfx.SendEvent("PlayBlackhole");
        }else{
            vfx.SendEvent("PlayWhitehole");
        }
    }
}
