using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public Rigidbody2D RigidBody;
    public Animator animator;
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }

    #region Controller Variable
    public float RunSpeed;
    public float WalkSpeed;
    public float FirstJumpForce;
    public float SecondJumoForce;
    public float DodgeTime;
    public float DodgeCoolTime;
    public float DodgeForce;
    public float DodgeGravity;
    [HideInInspector] public float HorizontalMoveInput;
    [HideInInspector] public bool VerticalMoveInput;
    [HideInInspector] public int JumpCount;
    [HideInInspector] public int MaxJumpCount;    
    //[HideInInspector] public int AirState;
    //记录当前跳跃状态，0为未跳跃，1为上升，2为下降，3为即将落地
    [HideInInspector] public bool IsFacingRight;
    [HideInInspector] public bool IsGround;
    [HideInInspector] public bool IsDodging;
    [HideInInspector] public bool CanDodge;
    [HideInInspector] public bool DodgeInput;

    RaycastHit2D HitObject;
    // 这是被射线碰撞的结构体实例，包含有关碰撞的详细信息，如碰撞点的位置、碰撞点的表面法线、碰撞物体的引用
    Vector2 RayOrigin;
    // 射线的起始点
    Vector2 RayDirection;
    // 射线的方向
    float RayDistance;
    // 射线的长度,同时也是人物开始进入jump_state 3时的离地面的距离
    LayerMask RayLayer;
    // 使射线只检测30层里的碰撞ti

    #endregion

    #region State Machine Variable

    public PlayerStateMachine StateMachine { get; set; }
    public PlayerIdleState IdleState { get; set; }
    public PlayerRunState RunState { get; set; }
    public PlayerWalkState WalkState { get; set; }
    public PlayerJumpState JumpState { get; set; }
    public PlayerFallState FallState { get; set; }
    public PlayerDodgeState DodgeState { get; set; }

    #endregion

    private void Awake()
    {
        RigidBody  = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        IsFacingRight = true;
        IsGround = true;
        IsDodging = false;
        CanDodge = true;
        DodgeInput = false;
        JumpCount = 0;
        MaxJumpCount = 2;

        RayDirection = Vector2.down;
        
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine);
        RunState = new PlayerRunState(this, StateMachine);
        WalkState = new PlayerWalkState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
        FallState = new PlayerFallState(this, StateMachine);
        DodgeState = new PlayerDodgeState(this, StateMachine);
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;

        StateMachine.InitializeState(IdleState);
    }

    private void Update()
    {        
        StateMachine.CurrentPlayerState.FrameUpdate();
        
        //flip the player according to the direction he is facing
        if (HorizontalMoveInput > 0.01f && IsFacingRight == false)
        {
            IsFacingRight = !IsFacingRight;
            transform.Rotate(0, 180, 0);
        }
        else if (HorizontalMoveInput < -0.01f && IsFacingRight == true)
        {
            IsFacingRight = !IsFacingRight;
            transform.Rotate(0, 180, 0);        
        }
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentPlayerState.PhysicsUpdate();
    }

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    #region Animation Trigger

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentPlayerState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        PlayerDamaged,
    }

    #endregion

}