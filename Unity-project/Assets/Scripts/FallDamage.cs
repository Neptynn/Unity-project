using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
    private PlayerHealth playerHealth;
    //[SerializeField] private SafeGroundCheckpointSaver safeGroundCheckpointSaver;

    private void Awake()
    {
        //safeGroundCheckpointSaver = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<SafeGroundCheckpointSaver>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //Debug.Log(1);
            playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth>();   
            playerHealth.DamageHealth(1);

            //safeGroundCheckpointSaver.WarpPlayerToSafeGround();
        }
    }
}
