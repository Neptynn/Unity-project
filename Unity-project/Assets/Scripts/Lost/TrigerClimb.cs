using CustomEventBus.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;

public class TrigerClimb : MonoBehaviour
{
    private BoxCollider2D box;
    [SerializeField] private LayerMask Detect;
    private EventBus _eventBus;
    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        box = GetComponents<BoxCollider2D>()[0];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((Detect.value & 1 << collision.gameObject.layer) != 0) { box.enabled = false; }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((Detect.value & 1 << collision.gameObject.layer) != 0) { box.enabled = true; }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((Detect.value & 1 << collision.gameObject.layer) != 0 && box.enabled)
        {
            _eventBus.Invoke(new StartClimb());
        }
    }

}
