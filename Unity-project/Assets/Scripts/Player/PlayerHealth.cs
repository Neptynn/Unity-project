using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int playerHealth = 5;
    private DamageFlash _damageFlash;  

    private void Start()
    {
        _damageFlash = GetComponentInChildren<DamageFlash>();
    }
    //����������� � 
    public void DamageHealth(int damage)
    {
        playerHealth = playerHealth - damage;

        //������������ ��������� ���� ���� ������ ������ ������'�
        if(playerHealth == 0) 
        {
            SceneManager.LoadSceneAsync(0); 
        }

        //������ ������� �������� ��� �������� �����
        _damageFlash.CallDamageFlash();
    }    
    
}
