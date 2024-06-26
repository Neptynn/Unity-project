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
    public UnityEvent attackEvent = new();

    private void Awake()
    {
        Instance = this;
        _playerInputAction = new PlayerInputActions();
        _playerInputAction.Enable();
        //_playerInputAction.Player.Jump.performed += context => OnJump();
        //_playerInputAction.Player.Attack.performed += context => OnAttack();
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
        //Debug.Log(move);
        move = context.ReadValue<Vector2>();
    }

    public Vector2 GetMove()
    {
        return move;
    }
    public void OnJump()
    {
        Debug.Log("Yes");
        jumpEvent?.Invoke();
    }

    public void OnAttack()
    {
        attackEvent?.Invoke();
    }


    //private void PlayerAttack_started(InputAction.CallbackContext context)
    //{
    //   // OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    //}
}
