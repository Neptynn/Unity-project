using System;
using UnityEngine.Events;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using CustomEventBus;
using CustomEventBus.Signals;

public class PlayerModel : MonoBehaviour, IService
{
    public static PlayerModel Instance { get; private set; }
    private PlayerInputActions _playerInputAction;

    [Header("Set in Inspector")]
    //[SerializeField] private PlayerVisual _playerVisual;
    [SerializeField] private GroundCheck _groundCheck;
    [SerializeField] private RoofCheck _isRoofUp;
    [SerializeField] private WallCheck _wallCheck;
    [SerializeField] private float movingSpeed = 10f;
    [SerializeField] private float crochSpeed = 5f;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private int maxJumpValue = 2;
    [SerializeField] private int _dashImpulse = 5000;
    [SerializeField] private float _cooldownDashTime = 2f;
    [SerializeField] private float _upDownSpeed = 4f;
    [SerializeField] private float _slideSpeed = -1;
    [SerializeField] private float _gravityDef;
    private bool _lockDash = false;
    private float currentSpeed;

    private int jumpCount = 0;
    public Rigidbody2D _rb;
    private float minMivingSpeed = 0.1f;
    private bool isRunning = false;
    private bool isGrounded = false;
    private bool isCrouch = false;
    private bool isRoofUp = false;
    private bool isWall = false;

    // private bool _isFacingRight = true;
    private Vector2 move;
    private Vector2 moveY;
    private bool _jumpOffEnable = true;
    private bool isSKeyPressed = false;


    public UnityEvent startJumpAnim = new();
    public UnityEvent startDashAnim = new();
    public UnityEvent startCrouchingAnim = new();
    public UnityEvent startUpDownWallAnim = new();


    private EventBus _eventBus;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _playerInputAction = new PlayerInputActions();
        _playerInputAction.Enable();
        _rb = GetComponent<Rigidbody2D>();
        currentSpeed = movingSpeed;

        // _eventBus.Subscribe<IsRoofUpState>();

    }
    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<IsGroundState>(UpdateIsGrounded);
        _eventBus.Subscribe<IsWallState>(UpdateIsWall);
        _eventBus.Subscribe<IsRoofUpState>(UpdateIsCrouch);
        _gravityDef = _rb.gravityScale;
    }
    private void FixedUpdate()
    {
        OnMove();
        //isGrounded = _groundCheck.GetIsGround();
        //isWall = _wallCheck.IsWall();
        if (isGrounded && (_rb.velocity.y < 0.0001 && _rb.velocity.y > -0.0001))
        {
            isGrounded = true;

            jumpCount = 0;
        }
        OnWall();

    }
    private void Update()
    {
        isSKeyPressed = Keyboard.current.sKey.isPressed;

        //Debug.Log("Update " + isWall);
    }

    public void OnMove()
    {
        move = _playerInputAction.Player.Move.ReadValue<Vector2>();
        _rb.velocity = new Vector2(move.x * movingSpeed, _rb.velocity.y);
        isRunning = Mathf.Abs(move.x) > minMivingSpeed || Mathf.Abs(move.y) > minMivingSpeed;
    }

    public bool IsRunning()
    {
        return isRunning;
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }
    public bool CantJump()
    {
        return jumpCount >= maxJumpValue - 1;
    }

    public void ChangeIsGround()
    {
        isGrounded = true;
    }
    public Vector2 GetMove()
    {
        return move;
    }
    public bool GetIsCrouching()
    {
        return isCrouch;
    }
    public bool GetISWall()
    {
        return isWall;
    }

    public void OnJump()
    {
        if (!isSKeyPressed && (isGrounded || ++jumpCount < maxJumpValue))
        {
            startJumpAnim?.Invoke();
            //_rb.AddForce(Vector2.up * jumpForce);
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        }
        //_rb.AddForce(Vector2.up * jumpForce);
    }
    public void OnJumpDown()
    {
        if(_jumpOffEnable)
        {
            StartCoroutine("JumpOff");
        }

    }
    IEnumerator JumpOff()
    {
        _jumpOffEnable = false;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        yield return new WaitForSeconds(0.3f);
        Physics2D.IgnoreLayerCollision(10, 11, false);
        _jumpOffEnable = true;
    }

    private void OnDash()
    {
        if(!_lockDash && !isCrouch)
        {
            startDashAnim?.Invoke();
            _lockDash = true;
            Invoke("LockDash", _cooldownDashTime);
            _rb.velocity = new Vector2(0.1f, 0.1f);
            if (PlayerVisual.Instance.transform.rotation.y > 0)
            {
                _rb.AddForce(Vector2.left * _dashImpulse);

            }
            else { _rb.AddForce(Vector2.right * _dashImpulse); }
        }
    }
    private void LockDash()
    {
        _lockDash = false;
    }

    private void OnCrouch()
    {
        if (!isRoofUp && isGrounded)
        {
            currentSpeed = crochSpeed;
            isCrouch = !isCrouch;
            startCrouchingAnim?.Invoke();
        }
        if(!isCrouch)
        {
            currentSpeed = movingSpeed;
        }
    }

    private void OnMoveWall()
    {

    }
    private void OnWall()
    {
       // Debug.Log("OnWall");
        if (isWall && !isGrounded)
        {
            moveY = _playerInputAction.Player.MoveWall.ReadValue<Vector2>();
            PlayerVisual.Instance.StartUpDownWallAnim(moveY);
            if (moveY.y == 0)
            {
                _rb.gravityScale = 0;
                _rb.velocity = new Vector2(0, _slideSpeed);

            }
            if (moveY.y > 0)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, moveY.y * _upDownSpeed/2);
            }
            else if (moveY.y != 0) { _rb.velocity = new Vector2(_rb.velocity.x, moveY.y * _upDownSpeed);}
        } else if(!isWall && !isGrounded)
        {
            _rb.gravityScale = _gravityDef;
        }
    }

    public void UpdateIsGrounded(IsGroundState signal)
    {
        Debug.Log(isGrounded);
        isGrounded = signal.isGround;
    }

    public void UpdateIsWall(IsWallState signal)
    {
        isWall = signal.isWall;
        Debug.Log(isWall);
    }
    public void UpdateIsCrouch(IsRoofUpState signal)
    {
        isRoofUp = signal.isRoofUp;
    }
}
