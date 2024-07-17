using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailAlfa : MonoBehaviour
{
    [SerializeField] private PlayerMovementStats MoveStats;
    [SerializeField] private float alpha;

    private SpriteRenderer spriteRenderer;
    private float alphaInterval;
    private float timer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = 0f;
        alphaInterval = MoveStats.DashTime / 4;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= alphaInterval)
        {
            Color color = spriteRenderer.color;
            // Змінюємо альфа-канал
            color.a -= alpha;
            // Встановлюємо новий колір назад до SpriteRenderer
            spriteRenderer.color = color;

            timer = 0f;
        }     
    }
}
