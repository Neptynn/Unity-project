using System;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instanse { get; private set; }

    [SerializeField] private GameInput _gameInput;
    [SerializeField] private PlayerVisual _playerVisual;
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private float jumpForce = 300f;
    private Rigidbody2D _rb;
    private float minMivingSpeed = 0.1f;
    private bool isRunning = false;
    private bool isGrounded = true;
    private bool isJump = false;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private Vector2 checkSize = new Vector2(0.8f, 0.1f);
    //[SerializeField] private float checkRadius = 0.1f;
    [SerializeField] private LayerMask Ground;
    //private bool jumpControl;
    //private int jumpIteration = 0;
    //private int jumpValueIteration = 60;
    //[SerializeField] private Collider2D _collider;
    private int jumpCount = 0;
    [SerializeField] private int maxJumpValue = 2;
    private Vector2 move;

    public UnityEvent startJumpAnim = new();
    //InputAction.CallbackContext context;

    private void Awake()
    {
   
        Instanse = this;
        _rb = GetComponent<Rigidbody2D>();
        _gameInput.jumpEvent.AddListener(OnJump);
        //_gameInput = GetComponent<GameInput>();
        //checkSize = new Vector2()
    }


    private void GameInput_OnPlayerAttack(object sender, EventArgs e)
    {
       // ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        CheckingGround();
        OnMove();
    }

    public void OnMove()
    {
        move = GameInput.Instance.GetMove();
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
    public bool IsJump()
    {
        Debug.Log(isJump);
        return isJump;

    }
    public void SetJump()
    {
        isJump = false;
    }
    public bool CantJump()
    {
        return jumpCount >= maxJumpValue - 1;
    }

    //public Vector3 GetPlayerScreenPosition()
    //{
    //    Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
    //    return playerScreenPosition;
    //}
    public void ChangeIsGround()
    {
        isGrounded = true;
    }

    public void OnJump()
    {
        //if (isGrounded)
        //{
        //    jumpControl = true;
        //}
        //else { jumpControl = false; }
        //if (jumpControl)
        //{
        //    if(jumpIteration++ < jumpValueIteration) 
        //    {
        //        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce/jumpIteration);
        //    }
        //}
        //else
        //{
        //    jumpIteration = 0;
        //}
        if (isGrounded || (++jumpCount < maxJumpValue))
        {
            startJumpAnim?.Invoke();
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            isJump = true;
        }
        if (isGrounded)
        {
            startJumpAnim?.Invoke();
            jumpCount = 0;
            isJump = true;
        }

        //_rb.AddForce(Vector2.up * jumpForce);
    }

    private void CheckingGround()
    {
        Collider2D collider = Physics2D.OverlapBox(GroundCheck.position, checkSize, 0f, Ground);
        //isGrounded = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Ground);
        isGrounded = collider != null;   
    }
    void OnDrawGizmos()
    {
        // Коробка в редакторі 
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GroundCheck.position, checkSize);
    }
}
