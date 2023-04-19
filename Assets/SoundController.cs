using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource source;

    public AudioClip footstep, run, attack, hit, detect, scream, dying;

    public void footSound()
    {
        source.clip = footstep;
        source.Play();
    }

    public void runSound()
    {
        source.clip = run;
        source.Play();
    }

    public void attackSound()
    {
        source.clip = attack;
        source.Play();
    }

    public void hitSound()
    {
        source.clip = hit;
        source.Play();
    }

    public void detectSound()
    {
        source.clip = detect;
        source.Play();
    }

    public void screamSound()
    {
        source.clip = scream;
        source.Play();
    }

    public void dyingSound()
    {
        source.clip = dying;
        source.Play();
    }
}
