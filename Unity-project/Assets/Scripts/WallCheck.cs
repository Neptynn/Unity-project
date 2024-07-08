using CustomEventBus.Signals;
using CustomEventBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WallCheck : MonoBehaviour
{
    [SerializeField] private LayerMask wallMask;
    private bool _isWall = false;
    private bool _canWallJump;

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
            _canWallJump = true;
            _isWall = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & wallMask) != 0)
        {
            _canWallJump = true;
            _isWall = false;
        }

    }
    public bool IsWall()
    {
        return _isWall;
    }
    private void PushIsWall(CheckState signal)
    {
        _canWallJump = signal.flagState && _isWall;
        _eventBus.Invoke(new IsWallState(_isWall, _canWallJump));
    }
}

