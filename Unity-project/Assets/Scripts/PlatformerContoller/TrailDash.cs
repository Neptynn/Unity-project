using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailDash : MonoBehaviour
{
    [SerializeField] private PlayerMovementStats MoveStats;
    [SerializeField] private GameObject trailRenderer;
    
    private float trailInterval; // Інтервал появи сліду
    private SpriteRenderer playerRenderer;
    private float timer;
    private float timerEnd;

    private void Awake()
    {
        trailInterval = MoveStats.DashTime / 4f;
        timerEnd = MoveStats.DashTime * 2;
    }

    void Start()
    {

        playerRenderer = GetComponent<SpriteRenderer>();
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= trailInterval)
        {
            CreateTrail();
            timer = 0f;
        }
    }

    void CreateTrail()
    {
        GameObject trail = Instantiate(trailRenderer, transform.position, transform.rotation);
        SpriteRenderer trailSpriteRenderer = trail.GetComponent<SpriteRenderer>();
        trailSpriteRenderer.sprite = playerRenderer.sprite;
        Destroy(trail, timerEnd);
    }
}
