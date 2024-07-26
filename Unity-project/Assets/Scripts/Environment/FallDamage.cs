using UnityEngine;

public class FallDamage : MonoBehaviour
{
    private PlayerHealth playerHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) // Перевірка за зікнення колайдеру тригера з гравцем
        {
            //Виклик в об'єкту функції отримання шкоди
            playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth>();   
            playerHealth.DamageHealth(1);
        }
    }
}
