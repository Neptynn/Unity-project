using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GroundCheck : MonoBehaviour
{

    //private bool isGrounded = false;
    //[SerializeField] private LayerMask Ground;

    //[SerializeField] private string COLLISION_WITH_PLATFORM = "Platform";
    ////[SerializeField] private string COLLISION_WITH_GROUND = "Ground";

    //private void FixedUpdate()
    //{
    //    //
    //    //Collider2D collider = Physics2D.OverlapBox(_position, _collider.size, 0f, Ground);
    //    //isGrounded = collider != null;
    //}
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //if (collision.tag == COLLISION_WITH_PLATFORM)
    //    //{

    //    //}
    //    if (((1 << collision.gameObject.layer) & Ground) != 0)
    //    {
    //        isGrounded = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (((1 << collision.gameObject.layer) & Ground) != 0)
    //    {
    //        isGrounded = false;
    //    }
    //    //if (collision.tag == COLLISION_WITH_PLATFORM)
    //    //{
    //    //    isGrounded = false;
    //    //}
    //}



    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    private Rigidbody2D rb;
    BoxCollider2D _collider;
    Transform _transform;
    [SerializeField] private LayerMask groundMask; // Маска слоя, устанавливается в Испекторе
    [SerializeField] private Vector2 groundCheckSize; // Размер "квадрата", проверяющего слой земли
    [SerializeField] private Transform groundCheckTransform; // Компонент Трансформ, объекта GroundCheck
    [SerializeField] private bool _isGrounded = false;
    private Vector3 _position;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        groundCheckSize = _collider.size;
        //_position = transform.position + new Vector3(_collider.offset.x, _collider.offset.y, 0f);
        ChekingGround();
    }


    private void ChekingGround()
    {
        if (_isGrounded == false)
        {
            _isGrounded = rb.IsTouchingLayers(groundMask) && Physics2D.OverlapBox(groundCheckTransform.position, groundCheckSize, 0f, groundMask);
        }
        else
        {
            _isGrounded = Physics2D.OverlapBox(groundCheckTransform.position, groundCheckSize, 0f, groundMask);
        }
    }

    private void OnDrawGizmos() // Отрисовка "квадрата", проверяющего слой земли
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckTransform.position, groundCheckSize);
    }

    public bool GetIsGround()
    {
        return _isGrounded;
    }



    //void OnDrawGizmos()
    //{
    //    //Коробка в редакторі
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(_position, _collider.size);
    //}
}

