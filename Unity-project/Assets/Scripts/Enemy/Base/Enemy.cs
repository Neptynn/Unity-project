using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerChackable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public Rigidbody2D RB { get; set; }
    public Animator Anim { get; set; }
    public bool IsFacingRight { get; set; } = true;
    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }
    public bool IsEndGround { get; set; }
    public bool IsNeedMoveRight { get; set; }

    #region State Machine Variables

    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }   
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }


    #endregion

    #region

    [SerializeField] private EnemyIdleSOBase EnemyIdleBase;
    [SerializeField] private EnemyChaseSOBase ChaseStateBase;
    [SerializeField] private EnemyAttackSOBase AttackStateBase;

    public EnemyIdleSOBase EnemyIdleBaseInstance { get; set; }
    public EnemyChaseSOBase EnemyChaseBaseInstance { get; set; }
    public EnemyAttackSOBase EnemyAttackBaseInstance { get; set; }
    


    #endregion

    #region Idle Variables

    public float moveSpeed = 3f;

    #endregion

    private void Awake()
    {
        EnemyIdleBaseInstance = Instantiate(EnemyIdleBase);
        EnemyChaseBaseInstance = Instantiate(ChaseStateBase);
        EnemyAttackBaseInstance = Instantiate(AttackStateBase);


        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this,StateMachine);
        AttackState = new EnemyAttackState(this,StateMachine);
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;

        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();

        EnemyIdleBaseInstance.Initialize(gameObject, this);
        EnemyChaseBaseInstance.Initialize(gameObject, this);
        EnemyAttackBaseInstance.Initialize(gameObject, this);
 

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }
    private void LateUpdate()
    {
        IsEndGround = false;

    }

    #region Health / Die Function
    public void Damage(float damageAmount)
    {
       CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0f ) 
        {
            Die();
        }
    }

    public void Die()
    {

    }
    #endregion

    #region Movement Functions
    public void MoveEnemy(Vector2 velocity)
    {
        RB.velocity = velocity;
        CheckForLeftOrRightFacing(velocity);
    }

    public void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        if (IsFacingRight && velocity.x < 0f) 
        {
            Vector3 rotation = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotation);
            IsFacingRight = !IsFacingRight;
        }        
        else if (!IsFacingRight && velocity.x > 0f)
        {
            Vector3 rotation = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotation);
            IsFacingRight = !IsFacingRight;
        }
    }
    #endregion

    #region Animation Triggers

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        DisableMove,
        EnableMove,
        EnemyDamaged,
        PlayFootstepSound
    }
    #endregion

    #region Distance Checks

    public void SetAggroStatus(bool isAggroed)
    {
        IsAggroed = isAggroed;
    }

    public void SetStrikingDistanceBool(bool isWithinStrikingDistance)
    {
        IsWithinStrikingDistance = isWithinStrikingDistance;
    }

    public void SetEndGround(bool isEndGround)
    {
        IsEndGround = isEndGround; 
    }
    public void SetNeedMoveRight(bool isNeedMoveRight)
    {
        IsNeedMoveRight = isNeedMoveRight;
    }

    #endregion

    #region Way to use InvokeRepeating



    public void RepeatPath()
    {
        InvokeRepeating("Path", 0, 0.5f);
    }
    private void Path()
    {

    }

    #endregion


}
