using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions _playerInputAction;
    
    //public event EventHandler OnPlayerAttack;
    public Vector2 move;
    public bool jump;
    public static GameInput Instance { get; private set; }

    public UnityEvent jumpEvent = new();

    private void Awake()
    {
        Instance = this;
        _playerInputAction = new PlayerInputActions();
        _playerInputAction.Enable();
        _playerInputAction.Player.Jump.performed += context => OnJump();
    }
    void OnEnable()
    {
        _playerInputAction.Enable();
    }

    void OnDisable()
    {
        _playerInputAction.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public Vector2 GetMove()
    {
        return move;
    }
    public void OnJump()
    {
        jumpEvent?.Invoke();
    }

    private void PlayerAttack_started(InputAction.CallbackContext context)
    {
       // OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }
}
