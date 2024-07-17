using CustomEventBus.Signals;
using CustomEventBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerVisual : MonoBehaviour, IService
{
    private Animator _animator;
    private const string RUNNING = "Running";
    private const string IS_GROUND = "IsGround";
    private const string CANT_JUMP = "CantJump";
    private const string Do_JUMP = "DoJump";
    private const string MOVE_X = "MoveX";
    private const string CROUCHING = "Crouching";
    private const string UP_DOWN = "UpDown";
    private const string ON_WALL = "OnWall";

    [SerializeField] private Collider2D _colliderStay;
    [SerializeField] private Collider2D _colliderCrouch;

    private EventBus _eventBus;
    //[SerializeField] private LayerMask _collisionMask;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        //_eventBus.Subscribe<StartClimb>(StartClimb);
        _eventBus.Subscribe<StartMoveAnim>(StartMoveAnim);
        _eventBus.Subscribe<StartAnims>(StartAnims);
        _eventBus.Subscribe<StartAnimJump>(StartAnimJump);

    }
    private void Update()
    {
        if (_animator != null)
        {
            // _animator.SetBool(RUNNING, PlayerModel.Instance.IsRunning());
            // _animator.SetBool(IS_GROUND, PlayerModel.Instance.IsGrounded());
            // _animator.SetBool(CANT_JUMP, PlayerModel.Instance.CantJump());
            // _animator.SetFloat(MOVE_X, Mathf.Abs(PlayerModel.Instance._rb.velocity.x));
            // _animator.SetBool(ON_WALl, PlayerModel.Instance.GetISWall());
        }

    }

    private void StartMoveAnim(StartMoveAnim signal)
    {
        _animator.SetBool(RUNNING, signal.isRunning);
    }

    private void StartAnims(StartAnims signal)
    {
        if (signal.isDashing == true)
        {
            _animator.StopPlayback();
            _animator.Play("Dash");
        }
        _animator.SetBool(IS_GROUND, signal.isGround);
        _animator.SetBool(ON_WALL, signal.isTouchingWall);
        _animator.SetBool(CANT_JUMP, signal.cantJump);
    }

    private void StartAnimJump(StartAnimJump signal)
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

    public void LadgeGo()
    {
        _eventBus.Invoke(new PushSignal());
    }
    public bool blockMoveX;
    public bool GetBlokMove()
    {
        return blockMoveX;
    }

    private void StartClimb(StartClimb signal)
    {
        blockMoveX = true;
        //_rb.velocity = Vector2.zero;
        _animator.Play("Climb");
    }
}

