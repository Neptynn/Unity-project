using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class WallCheck : MonoBehaviour
{
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private bool _isWall = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & wallMask) != 0))
        {
            _isWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & wallMask) != 0)
        {
            _isWall = false;
        }

    }
    public bool IsWall()
    {
        return _isWall;
    }
}

