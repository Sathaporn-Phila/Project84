using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] Image greenbar;
    [SerializeField] Image blackbar;
    [SerializeField] PlayerHealth player;
    
    float MaxHP;

    private void Start()
    {
        MaxHP = player.HP;
    }

    // Update is called once per frame
    void Update()
    {
        greenbar.fillAmount = player.HP / MaxHP;
    }
}
