using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerVisual : MonoBehaviour, IService
{
    public static PlayerVisual Instance { get; private set; }

    private Animator _animator;
    private Rigidbody2D _rb;
    private const string RUNNING = "Running";
    private const string IS_GROUND = "IsGround";
    private const string CANT_JUMP = "CantJump";
    private const string Do_JUMP = "DoJump";
    private const string MOVE_X = "MoveX";
    private const string CROUCHING = "Crouching";
    private const string UP_DOWN = "UpDown";
    private const string ON_WALl = "OnWall"; 
    [SerializeField] private PlayerModel _player;
    [SerializeField] private GameObject _cameraFollowGo;

    [SerializeField] private Collider2D _colliderStay;
    [SerializeField] private Collider2D _colliderCrouch;


    public bool _isFacingRight = true;
    private Vector2 move;
    private PlayerFollowCam _followCam;
    private float _fallSpeedYDampingChangeTreshold;

    //[SerializeField] private LayerMask _collisionMask;
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
        _player.startDashAnim.AddListener(StartDashAnim);
        _player.startCrouchingAnim.AddListener(StartCrouchingAnim);

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
            _animator.SetBool(RUNNING, PlayerModel.Instance.IsRunning());
            _animator.SetBool(IS_GROUND, PlayerModel.Instance.IsGrounded());
            _animator.SetBool(CANT_JUMP, PlayerModel.Instance.CantJump());
            _animator.SetFloat(MOVE_X, Mathf.Abs(PlayerModel.Instance._rb.velocity.x));
            _animator.SetBool(ON_WALl, PlayerModel.Instance.GetISWall());
        }

    }
    private void FixedUpdate()
    {
        move = PlayerModel.Instance.GetMove();
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
            Debug.Log(1);
            Vector3 _rotate = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(_rotate);
            _isFacingRight = !_isFacingRight;
            _followCam.CallTurn();
        }
        else
        {
            Debug.Log(2);
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

    private void StartDashAnim()
    {
        if (_animator != null)
        {
            _animator.StopPlayback();
            _animator.Play("Dash");
        }
    }

    private void StartCrouchingAnim()
    {
        if (_animator != null)
        {
            _colliderStay.enabled = !_colliderStay.enabled;
            _colliderCrouch.enabled = !_colliderCrouch.enabled;
            _animator.SetBool(CROUCHING, PlayerModel.Instance.GetIsCrouching());
        }
    }
    public void StartUpDownWallAnim(Vector2 moveY)
    {
        if (_animator != null)
        {
            _animator.SetFloat(UP_DOWN, moveY.y);
            _animator.StopPlayback();
            _animator.Play("UpDown");

        }
    }
}

