using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerVisual : MonoBehaviour
{
    public static PlayerVisual Instance { get; private set; }

    private Animator _animator;
    private Rigidbody2D _rb;
    private const string RUNNING = "Running";
    private const string IS_GROUND = "IsGround";
    private const string CANT_JUMP = "CantJump";
    private const string Do_JUMP = "DoJump";
    private const string MOVE_UP = "MoveUp";
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _cameraFollowGo;


    public bool _isFacingRight = true;
    private Vector2 move;
    private PlayerFollowCam _followCam;
    private float _fallSpeedYDampingChangeTreshold;

    [SerializeField] private LayerMask _collisionMask;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _rb = GetComponentInParent<Rigidbody2D>();

        _animator = GetComponent<Animator>();
        //_spriteRenderer = GetComponent<SpriteRenderer>();
        //_collider = GetComponent<Collider2D>();
        _player.startJumpAnim.AddListener(StartJumpAnim);

    }
    private void Start()
    {
        _followCam = _cameraFollowGo.GetComponent<PlayerFollowCam>();
        _fallSpeedYDampingChangeTreshold = CameraManager.instance._fallSpeedDampingChargeTheshold;
    }

    private void Update()
    {
        if (_animator != null)
        {
            _animator.SetBool(RUNNING, Player.Instance.IsRunning());
            _animator.SetBool(IS_GROUND, Player.Instance.IsGrounded());
            _animator.SetBool(CANT_JUMP, Player.Instance.CantJump());
            _animator.SetBool(MOVE_UP, _rb.velocity.y > 0.001);
        }

    }
    private void FixedUpdate()
    {
        move = Player.Instance.GetMove();
        if (move.x != 0)
        {
            TurnCheck();
        }

        if (_rb.velocity.y < _fallSpeedYDampingChangeTreshold && !CameraManager.instance.IsLerpingYDaming && !CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }
        if (_rb.velocity.y > 0f && !CameraManager.instance.IsLerpingYDaming && CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }

    }

    private void TurnCheck()
    {
        if (move.x > 0 && !_isFacingRight)
        {
            Turn();
        }
        else if (move.x < 0 && _isFacingRight)
        {
            Turn();
        }
    }
    private void Turn()
    {
        if (_isFacingRight)
        {
            Vector3 _rotate = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(_rotate);
            _isFacingRight = !_isFacingRight;
            _followCam.CallTurn();
        }
        else
        {
            Vector3 _rotate = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(_rotate);
            _isFacingRight = !_isFacingRight;
            _followCam.CallTurn();
        }
    }
    private void StartJumpAnim()
    {
        //&& GroundCheck.Instance.GetCollisiosCollider().gameObject.layer != _collisionMask
        if (_animator != null)
        {
            _animator.SetTrigger(Do_JUMP);
        }
    }




}

