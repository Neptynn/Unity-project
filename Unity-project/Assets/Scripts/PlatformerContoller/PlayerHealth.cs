using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int playerHealth = 5;
    private DamageFlash _damageFlash; 

    private void Start()
    {
        _damageFlash = GetComponentInChildren<DamageFlash>();
    }

    public void DamageHealth(int damage)
    {
        playerHealth = playerHealth - damage;

        _damageFlash.CallDamageFlash();
    }    
    
}
