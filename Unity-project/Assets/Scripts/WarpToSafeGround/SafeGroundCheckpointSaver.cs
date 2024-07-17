using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeGroundCheckpointSaver : MonoBehaviour
{
    //[SerializeField] private float safeFrequency = 3f;
    //[SerializeField] private PlayerMovement playerMovement;

    //public Vector2 SafeGroundLocation {  get; private set; } = Vector2.zero;

    //private Coroutine safeGroundCoroutine;

    //private void Start()
    //{
    //    safeGroundCoroutine = StartCoroutine(SaveGroundLocation());

    //    SafeGroundLocation = transform.position;
    //}

    //private IEnumerator SaveGroundLocation()
    //{
    //    float elapsedTime = 0f;
    //    while (elapsedTime < safeFrequency)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //    if (playerMovement.GetIsGrounded())
    //    {
    //        SafeGroundLocation = transform.position;
    //    }

    //    safeGroundCoroutine = StartCoroutine(SaveGroundLocation()); 
    //}

    [SerializeField] private LayerMask whatIsCheckPoint;
    public Vector2 SafeGroundLocation {  get; private set; } = Vector2.zero;

    private void Start()
    {
        SafeGroundLocation = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((whatIsCheckPoint.value & (1 << collision.gameObject.layer)) > 0) 
        {
            SafeGroundLocation = new Vector2(collision.bounds.center.x, collision.bounds.min.y);

        }
    }

    public void WarpPlayerToSafeGround()
    {
        transform.position = SafeGroundLocation;
    }
}
