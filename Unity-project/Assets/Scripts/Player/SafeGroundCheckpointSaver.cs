using UnityEngine;

public class SafeGroundCheckpointSaver : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsCheckPoint; //Перелік шарів що вважаються за чекпоінт
    public Vector2 SafeGroundLocation {  get; private set; } = Vector2.zero; //Позиція чекпоінту

    private void Start()
    {
        SafeGroundLocation = transform.position;//Встановлення початкового чекпоінту на місці появи гравця
    }

    private void OnTriggerEnter2D(Collider2D collision) //Спрацьовує коли гравець перетинає колайдер що є тригером
    {
        //Конструкція для порівняння шару що утворив колізію з шарами що вказані в масці
        if ((whatIsCheckPoint.value & (1 << collision.gameObject.layer)) > 0) 
        {
            //Збереження нової позиції для чекпоінта
            SafeGroundLocation = new Vector2(collision.bounds.center.x, collision.bounds.min.y);
            Debug.Log(SafeGroundLocation);
        }
    }

    public void WarpPlayerToSafeGround() //Переміщення гравця до останнього чек поінту. Викликається ззовні
    {
        transform.position = SafeGroundLocation;
    }
}
