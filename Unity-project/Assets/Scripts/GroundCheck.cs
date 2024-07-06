using CustomEventBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using CustomEventBus.Signals;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask; 
    [SerializeField] private bool _isGrounded = false;
    private EventBus _eventBus;
    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<CheckState>(PushIsGround);
    }

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

    private void PushIsGround(CheckState signal)
    {
        _eventBus.Invoke(new IsGroundState(_isGrounded));
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
    //    //Коробка в редакторі
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(_position, _collider.size);
    //}
}

