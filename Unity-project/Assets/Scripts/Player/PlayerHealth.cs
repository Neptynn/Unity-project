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
    //Викликається з 
    public void DamageHealth(int damage)
    {
        playerHealth = playerHealth - damage;

        //Завантаження головного меню після втрати всього здоров'я
        if(playerHealth == 0) 
        {
            SceneManager.LoadSceneAsync(0); 
        }

        //Виклик шейдеру миготіння при отриманні шкоди
        _damageFlash.CallDamageFlash();
    }    
    
}
