using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class console : MonoBehaviour
{
    public AudioSource audiosound;
    public AudioClip cardsound;
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.TryGetComponent<card>(out card owner)){
            audiosound.PlayOneShot(cardsound);
            this.transform.parent.Find("Wall_Door_02").GetComponent<doorRoom>().Open();
        }
    }
}
