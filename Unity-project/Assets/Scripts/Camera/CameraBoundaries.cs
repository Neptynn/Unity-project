using UnityEngine;
using Cinemachine;

public class CameraBoundaries : MonoBehaviour
{
    private int i = 0;
    private void LateUpdate()
    {
        if(i == 0)
        {
            StartFunk();
        }

    }
    private void StartFunk()
    {
        GameObject generatedLevel = GameObject.Find("Generated Level");
        if (generatedLevel != null)
        {
            // «находимо доч≥рн≥й об'Їкт Rooms
            Transform roomsTransform = generatedLevel.transform.Find("Rooms");
            GameObject _rooms = roomsTransform.gameObject;

            CompositeCollider2D _levelBoundaries = _rooms.GetComponent<CompositeCollider2D>();

            Rigidbody2D rb = _rooms.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;

            CompositeCollider2D _levelCollider = _rooms.AddComponent<CompositeCollider2D>();
            _levelCollider.isTrigger = true;
            _levelCollider.geometryType = CompositeCollider2D.GeometryType.Polygons;

            CinemachineConfiner2D confiner = GetComponent<CinemachineConfiner2D>();
            confiner.m_BoundingShape2D = _levelCollider;
        }
        i = 1;
    }

}
