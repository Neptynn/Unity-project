using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask; 
    [SerializeField] private bool _isGrounded = false;
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & groundMask) != 0 && PlayerModel.Instance._rb.velocity.y < 0.001 && PlayerModel.Instance._rb.velocity.y > -0.001) || PlayerModel.Instance._rb.velocity.y == 0)
        {
            _isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundMask) != 0)
        {
            _isGrounded = false;
        }

    }
    public bool GetIsGround()
    {
        return _isGrounded;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & groundMask) != 0 && PlayerModel.Instance._rb.velocity.y < 0.001 && PlayerModel.Instance._rb.velocity.y > -0.001))
        {
            _isGrounded = true;
        }
    }

    //void OnDrawGizmos()
    //{
    //    //������� � ��������
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(_position, _collider.size);
    //}
}

