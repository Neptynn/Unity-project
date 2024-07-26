using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallPlarform : MonoBehaviour
{
    private Tilemap tilemap;// Тайлмап(сітка шару)

    //[SerializeField] - надає можливість private полю бути зміненим в інспекторі
    [SerializeField] private float delayBeforeFall = 1.0f; //затримка перед падінням
    [SerializeField] private float fallDuration = 2.0f; //затримка перед зникненням об'єкту падаючої платформи
    [SerializeField] private float respawnTime = 5.0f; //час для відновлення
    [SerializeField] private GameObject fallingTilePrefab;

    //Словник корутин. Корутина виконує свою роботу паралельно та незалежно від інших частин програми
    //Словник використовується для падіння вірних платформ, а не лише останньої на момент взаємодії
    private Dictionary<Vector3Int, Coroutine> activeCoroutines = new Dictionary<Vector3Int, Coroutine>();

    void Start()
    {
        tilemap = GetComponent<Tilemap>(); // Отримання посилання на таййлмап
    }

    //Запускається при винекнині колізії між платформою та будь яеим об'єктом
    void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Player") //Пепевірка на взаємодію з гравцем
        {
            Vector3 hitPosition = Vector3.zero; //Точка де виникла колізія
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                //Точка з z в 2D не потрібна за замоченням 0

                Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);//Точка від якої будується клітинка сітки
                if (!activeCoroutines.ContainsKey(tilePosition)) //Переривання від повторного виклику активованої корутини
                {
                    //Активація корутини
                    Coroutine coroutine = StartCoroutine(HandleTileFall(tilePosition));
                    activeCoroutines[tilePosition] = coroutine;
                }
            }
        }
    }

    IEnumerator HandleTileFall(Vector3Int tilePosition)
    {
        yield return new WaitForSeconds(delayBeforeFall);//Затримка перед падінням

        TileBase tile = tilemap.GetTile(tilePosition);//Отримання тайлу з тайлмапу за її позицією
        if (tile != null)
        {
            // Створення об'єкта для падіння на місці тайлу. Шаблон об'єкту вказується в інспекторі.
            GameObject fallingTile = Instantiate(fallingTilePrefab, tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0f, 0f), Quaternion.identity);
            fallingTile.GetComponent<SpriteRenderer>().sprite = tilemap.GetSprite(tilePosition); //Встановлення в падаючий об'єкт такий же спрайт як і в тайла
            tilemap.SetTile(tilePosition, null); //Стирання тайлу з тайлмапу

            yield return new WaitForSeconds(fallDuration); //затримка перед видаленням падаючої платформи
            Destroy(fallingTile); //видалення падаючої платформи

            yield return new WaitForSeconds(respawnTime); //затримка перед відновленням тайлу
            tilemap.SetTile(tilePosition, tile); //відновлення тайлу
            activeCoroutines.Remove(tilePosition); //очищення позиції тайлу
        }
    }

}


