using UnityEngine;

public class SafeGroundCheckpointSaver : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsCheckPoint; //������ ���� �� ���������� �� �������
    public Vector2 SafeGroundLocation {  get; private set; } = Vector2.zero; //������� ��������

    private void Start()
    {
        SafeGroundLocation = transform.position;//������������ ����������� �������� �� ���� ����� ������
    }

    private void OnTriggerEnter2D(Collider2D collision) //��������� ���� ������� �������� �������� �� � ��������
    {
        //����������� ��� ��������� ���� �� ������� ����� � ������ �� ������ � �����
        if ((whatIsCheckPoint.value & (1 << collision.gameObject.layer)) > 0) 
        {
            //���������� ���� ������� ��� ��������
            SafeGroundLocation = new Vector2(collision.bounds.center.x, collision.bounds.min.y);
            Debug.Log(SafeGroundLocation);
        }
    }

    public void WarpPlayerToSafeGround() //���������� ������ �� ���������� ��� �����. ����������� �����
    {
        transform.position = SafeGroundLocation;
    }
}
