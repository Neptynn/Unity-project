using UnityEngine;

public class NewFallPlatform : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private float alpha; //��������� ��������� �� 1 ��������
    [SerializeField] private float alphaInterval; //��� ��������
    [SerializeField] private float disappearedTime;//��� ���������

    private float timer;//������ �� ������� ��������
    void Start()
    {
        alpha = alphaInterval / disappearedTime;//���������� ���� ����� �� 1 �������� (�� 0 �� 1) 
        _spriteRenderer = GetComponent<SpriteRenderer>();//��������� �� ���������� ��'����
    }

    private void Update()
    {
        timer += Time.deltaTime;//��������� �������

        if (timer >= alphaInterval)//�������� �� ������ ��������
        {
            Color color = _spriteRenderer.color;//��������� �� ��'��� ������� ����������

            color.a -= alpha;// ���� �����-������(���������)

            _spriteRenderer.color = color;// ������������ ������ ����� ����� �� SpriteRenderer

            timer = 0f;//��������� �������
        }
    }
}
