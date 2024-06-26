using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerVisual : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private const string RUNNING = "Running";
    private const string IS_GROUND = "IsGround";
    private const string CANT_JUMP = "CantJump";
    private const string Do_JUMP = "DoJump";
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _cameraFollowGo;

    public static PlayerVisual Instanse { get; private set; }


    private Vector2 _moveVec;



    private void Awake()
    {
        if (Instanse == null)
        {
            Instanse = this;
        }
       _rb = GetComponentInParent<Rigidbody2D>();

        _animator = GetComponent<Animator>();
        //_spriteRenderer = GetComponent<SpriteRenderer>();
        //_collider = GetComponent<Collider2D>();
        _player.startJumpAnim.AddListener(StartJumpAnim);

    }


    private void Update()
    {
        if (_animator != null)
        {
            _animator.SetBool(RUNNING, Player.Instance.IsRunning());
            _animator.SetBool(IS_GROUND, Player.Instance.IsGrounded());
            _animator.SetBool(CANT_JUMP, Player.Instance.CantJump());
        }
      
    }
    private void FixedUpdate()
    {
        //_moveVec = Player.Instance.GetMove();


    }

    private void StartJumpAnim()
    {
        if (_animator != null)
        {
            _animator.SetTrigger(Do_JUMP);
        }
    }




}

