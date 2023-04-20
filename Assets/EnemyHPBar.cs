using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] Image orangebar;
    [SerializeField] Image blackbar;
    [SerializeField] EnemyHealth enemy;
    
    float MaxHP;

    private void Start()
    {
        MaxHP = enemy.HP;
    }

    // Update is called once per frame
    void Update()
    {
        orangebar.fillAmount = enemy.HP / MaxHP;
    }
}
