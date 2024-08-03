using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWayAreaCheck : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            _enemy.SetNeedMoveRight(!_enemy.IsFacingRight);
            _enemy.SetEndGround(true);
        }
    }
}
