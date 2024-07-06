using CustomEventBus.Signals;
using CustomEventBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WallCheck : MonoBehaviour
{
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private bool _isWall = false;
    private EventBus _eventBus;
    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<CheckState>(PushIsWall);
    }
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
    private void PushIsWall(CheckState signal)
    {
        _eventBus.Invoke(new IsWallState(_isWall));
    }
}

