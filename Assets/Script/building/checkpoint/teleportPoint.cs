using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class teleportPoint : MonoBehaviour
{
    // Start is called before the first frame update
    VisualEffect vfx;
    [SerializeField]bool isBlackHole;
    void Awake(){
        vfx = GetComponent<VisualEffect>();
        if(isBlackHole){
            vfx.SendEvent("PlayBlackhole");
        }else{
            vfx.SendEvent("PlayWhitehole");
        }
    }
}
