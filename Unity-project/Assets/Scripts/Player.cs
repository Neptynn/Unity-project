using System;
using UnityEngine.Events;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private PlayerInputActions _playerInputAction;

    [Header("Set in Inspector")]
    //[SerializeField] private PlayerVisual _playerVisual;
    [SerializeField] private GroundCheck _groundCheck;
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private int maxJumpValue = 2;


    private int jumpCount = 0;
    protected Rigidbody2D _rb;
    private float minMivingSpeed = 0.1f;
    private bool isRunning = false;
    private bool isGrounded = true;
    private bool _isFacingRight = true;
    private Vector2 move;

    public UnityEvent startJumpAnim = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _playerInputAction = new PlayerInputActions();
        _playerInputAction.Enable();
        _rb = GetComponent<Rigidbody2D>();
    }  

    private void FixedUpdate()
    {
        OnMove();
        isGrounded = _groundCheck.GetIsGround();
        if (isGrounded)
        {
            jumpCount = 0;
        }
    }
    private void Update()
    {

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

    public void OnJump()
    {
        isGrounded = _groundCheck.GetIsGround();
        if (isGrounded || ++jumpCount < maxJumpValue)
        {
            startJumpAnim?.Invoke();
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        }
        //_rb.AddForce(Vector2.up * jumpForce);
    }

    public bool GetIsFacingRight()
    {
        return _isFacingRight;
    }
}
