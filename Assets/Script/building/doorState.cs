using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorState : MonoBehaviour
{
    // Start is called before the first frame update
    Animator m_animator;
    Transform doorLeft;
    void Start()
    {
        m_animator = GetComponent<Animator>();
        doorLeft = transform.Find("door.armature/left.door/door.l");
    }

    // Update is called once per frame
    void Update()
    {
        m_animator.SetInteger("doorState",1);
        Debug.Log(doorLeft.position);
    }
}
