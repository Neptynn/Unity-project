using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerVisual : MonoBehaviour
{
    private Animator _animator;
    //private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private const string RUNNING = "Running";
    private const string JUMP = "Jump";
    private const string IS_GROUND = "IsGround";
    private const string CANT_JUMP = "CantJump";
    private const string Do_JUMP = "DoJump";
    [SerializeField] private Player _player;

    private bool isMovingRight = true;
    private Vector2 moveVec;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        //_spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _player.startJumpAnim.AddListener(StartJumpAnim);
    }
    private void Update()
    {
        if (_animator != null)
        {
            _animator.SetBool(RUNNING, Player.Instanse.IsRunning());
            _animator.SetBool(JUMP, Player.Instanse.IsJump());
            Player.Instanse.SetJump();
            _animator.SetBool(IS_GROUND, Player.Instanse.IsGrounded());
            _animator.SetBool(CANT_JUMP, Player.Instanse.CantJump());

        }
        AdjustPlayerFacingDirrection();
    }

    private void StartJumpAnim()
    {
        if (_animator != null)
        {
            //_animator.SetTrigger(Do_JUMP);
            //_animator.SetBool(JUMP, Player.Instanse.IsJump());
            //Player.Instanse.SetJump();
        }
    }

    private void AdjustPlayerFacingDirrection()
    {
        moveVec = GameInput.Instance.GetMove();
        if ((moveVec.x > 0 && !isMovingRight) ||(moveVec.x < 0 && isMovingRight)) 
        {
            transform.localScale *= new Vector2(-1, 1);
            isMovingRight = !isMovingRight;
        }
    }
}

