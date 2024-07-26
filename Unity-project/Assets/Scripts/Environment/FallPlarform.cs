using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallPlarform : MonoBehaviour
{
    private Tilemap tilemap;// �������(���� ����)

    //[SerializeField] - ���� ��������� private ���� ���� ������� � ���������
    [SerializeField] private float delayBeforeFall = 1.0f; //�������� ����� �������
    [SerializeField] private float fallDuration = 2.0f; //�������� ����� ���������� ��'���� ������� ���������
    [SerializeField] private float respawnTime = 5.0f; //��� ��� ����������
    [SerializeField] private GameObject fallingTilePrefab;

    //������� �������. �������� ������ ���� ������ ���������� �� ��������� �� ����� ������ ��������
    //������� ��������������� ��� ������ ����� ��������, � �� ���� �������� �� ������ �����䳿
    private Dictionary<Vector3Int, Coroutine> activeCoroutines = new Dictionary<Vector3Int, Coroutine>();

    void Start()
    {
        tilemap = GetComponent<Tilemap>(); // ��������� ��������� �� ��������
    }

    //����������� ��� ��������� ���糿 �� ���������� �� ���� ���� ��'�����
    void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Player") //�������� �� ������� � �������
        {
            Vector3 hitPosition = Vector3.zero; //����� �� ������� �����
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                //����� � z � 2D �� ������� �� ���������� 0

                Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);//����� �� ��� �������� ������� ����
                if (!activeCoroutines.ContainsKey(tilePosition)) //����������� �� ���������� ������� ���������� ��������
                {
                    //��������� ��������
                    Coroutine coroutine = StartCoroutine(HandleTileFall(tilePosition));
                    activeCoroutines[tilePosition] = coroutine;
                }
            }
        }
    }

    IEnumerator HandleTileFall(Vector3Int tilePosition)
    {
        yield return new WaitForSeconds(delayBeforeFall);//�������� ����� �������

        TileBase tile = tilemap.GetTile(tilePosition);//��������� ����� � �������� �� �� ��������
        if (tile != null)
        {
            // ��������� ��'���� ��� ������ �� ���� �����. ������ ��'���� ��������� � ���������.
            GameObject fallingTile = Instantiate(fallingTilePrefab, tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0f, 0f), Quaternion.identity);
            fallingTile.GetComponent<SpriteRenderer>().sprite = tilemap.GetSprite(tilePosition); //������������ � �������� ��'��� ����� �� ������ �� � � �����
            tilemap.SetTile(tilePosition, null); //�������� ����� � ��������

            yield return new WaitForSeconds(fallDuration); //�������� ����� ���������� ������� ���������
            Destroy(fallingTile); //��������� ������� ���������

            yield return new WaitForSeconds(respawnTime); //�������� ����� ����������� �����
            tilemap.SetTile(tilePosition, tile); //���������� �����
            activeCoroutines.Remove(tilePosition); //�������� ������� �����
        }
    }

}


