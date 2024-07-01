using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class RoofCheck : MonoBehaviour
{
    [SerializeField] private LayerMask roofMask; 
    [SerializeField] private bool _isRoofUp = false;
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & roofMask) != 0 ))
        {
            _isRoofUp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & roofMask) != 0)
        {
            _isRoofUp = false;
        }

    }
    public bool IsRoofUp()
    {
        return _isRoofUp;
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (Player.Instance._rb.velocity.y < 0.001 && Player.Instance._rb.velocity.y > -0.001)
    //    {
    //        Debug.Log(_isGrounded);
    //        _isGrounded = true;
    //    }
    //}

    //void OnDrawGizmos()
    //{
    //    //Коробка в редакторі
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(_position, _collider.size);
    //}
}

