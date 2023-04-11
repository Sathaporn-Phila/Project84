using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorslide : MonoBehaviour
{
    Animator m_animator;
    public AudioClip Opendoor;
    public AudioClip Closedoor;
    AudioSource Audiosource;
    void Start()
    {
        m_animator = GetComponent<Animator>();
        Audiosource = GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        m_animator.SetInteger("doorState",1);
        if(Opendoor != null){
            Audiosource.PlayOneShot(Opendoor);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        m_animator.SetInteger("doorState",2);
        if(Closedoor != null){
            Audiosource.PlayOneShot(Closedoor);
        }
    }
}
