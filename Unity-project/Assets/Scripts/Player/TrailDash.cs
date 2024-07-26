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

    private Coroutine _returnToPoolTimerCoroutine;

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
    [SerializeField] private float alpha;

    private float alphaInterval;
    private float timerAlfa;
    Color color;
    private IEnumerator ReturnToPoolAfterTimer(GameObject trail, SpriteRenderer trailSpriteRenderer)
    {
        alphaInterval = MoveStats.DashTime / 4;
        float elapsedTime = 0f;
        while (elapsedTime < timerEnd)
        {
            elapsedTime += Time.deltaTime;
            timerAlfa += Time.deltaTime;
            if (timerAlfa >= alphaInterval)
            {
                color = trailSpriteRenderer.color;
                // Змінюємо альфа-канал
                color.a -= alpha;
                // Встановлюємо новий колір назад до SpriteRenderer
                trailSpriteRenderer.color = color;

                timerAlfa = 0f;
            }
            yield return null;
        }
        //color.a = 1;
        //trailSpriteRenderer.color = color;
        ObjectPoolManager.ReturnObjectToPool(trail);

    }

    void CreateTrail()
    {
        GameObject trail = ObjectPoolManager.SpawnObject(trailRenderer, transform.position, transform.rotation);
        SpriteRenderer trailSpriteRenderer = trail.GetComponent<SpriteRenderer>();
        trailSpriteRenderer.sprite = playerRenderer.sprite;
        _returnToPoolTimerCoroutine = StartCoroutine(ReturnToPoolAfterTimer(trail, trailSpriteRenderer));
    }
}
