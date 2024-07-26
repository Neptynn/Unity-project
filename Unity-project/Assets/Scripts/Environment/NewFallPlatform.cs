using UnityEngine;

public class NewFallPlatform : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private float alpha; //зменшення прозорості за 1 ітерацію
    [SerializeField] private float alphaInterval; //час ітерації
    [SerializeField] private float disappearedTime;//час зникнення

    private float timer;//таймер до початку ітерації
    void Start()
    {
        alpha = alphaInterval / disappearedTime;//розрахунок зміни альфи за 1 ітерацію (від 0 до 1) 
        _spriteRenderer = GetComponent<SpriteRenderer>();//посилання на зображення об'єкту
    }

    private void Update()
    {
        timer += Time.deltaTime;//оновлення таймеру

        if (timer >= alphaInterval)//перевірка на запуск ітерації
        {
            Color color = _spriteRenderer.color;//посилання на об'єкт кольору зображення

            color.a -= alpha;// Зміна альфа-канала(прозорості)

            _spriteRenderer.color = color;// Встановлення нового коліру назад до SpriteRenderer

            timer = 0f;//Обнулкння таймеру
        }
    }
}
