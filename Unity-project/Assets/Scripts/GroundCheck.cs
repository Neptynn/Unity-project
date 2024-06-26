using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GroundCheck : MonoBehaviour
{
    BoxCollider2D _collider;
    Transform _transform;
    private bool isGrounded = false;
    [SerializeField] private LayerMask Ground;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _transform = GetComponent<Transform>();
    }
    private void FixedUpdate()
    {
        Collider2D collider = Physics2D.OverlapBox(_transform.position, _collider.size, 0f, Ground);
        isGrounded = collider != null;
    }
    public bool GetIsGround()
    {
        return isGrounded;
    }
    //void OnDrawGizmos()
    //{
    //    // Коробка в редакторі 
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(GroundCheck.position, checkSize);
    //}
}

