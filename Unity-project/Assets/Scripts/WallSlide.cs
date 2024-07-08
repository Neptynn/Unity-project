using CustomEventBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlide : MonoBehaviour
{
    [SerializeField] private float distance = 2f;
    private EventBus _eventBus;
    private PlayerModel _playerModel;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _playerModel = ServiceLocator.Current.Get<PlayerModel>();
    }

    void Update()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance);
    
        //if(!_playerModel.)
    }
}
