using UnityEngine;

public class FallDamage : MonoBehaviour
{
    private PlayerHealth playerHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) // �������� �� ������� ��������� ������� � �������
        {
            //������ � ��'���� ������� ��������� �����
            playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth>();   
            playerHealth.DamageHealth(1);
        }
    }
}
