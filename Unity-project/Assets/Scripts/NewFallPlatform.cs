using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFallPlatform : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private float alpha;
    [SerializeField] private float alphaInterval;
    [SerializeField] private float disappearedTime;


    private float timer;
    void Start()
    {
        alpha = alphaInterval / disappearedTime;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("DeletePlatform", disappearedTime);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= alphaInterval)
        {
            Color color = _spriteRenderer.color;
            // Змінюємо альфа-канал
            color.a -= alpha;
            // Встановлюємо новий колір назад до SpriteRenderer
            _spriteRenderer.color = color;

            timer = 0f;
        }
    }

    private void DeletePlatform()
    {
        Destroy(gameObject);
    }
}
