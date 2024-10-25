using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D playerRigidbody;
    public Transform playerTransform;

    #region Controller Variable

    public float RunSpeed;
    public float WalkSpeed;
    public float FirstJumpForce;
    public float SecondJumpForce;
    public float AirDrag;
    public float DodgeTime;
    public float DodgeCoolTime;
    public float DodgeForce;
    public float DodgeGravity;
    public float ClimbSpeed;

    #endregion

    #region Player variable

    public int attackPower = 5;
    public int Defence = 5;

    #endregion

    #region player state variable

    [HideInInspector] public float HorizontalMoveInput;
    [HideInInspector] public float DodgeCoolTimer;
    [HideInInspector] public bool beginDodgeCoolTimer;
    [HideInInspector] public bool VerticalMoveInput;
    [HideInInspector] public bool IsCheckingVerticalMoveInput;
    [HideInInspector] public int JumpCount;
    [HideInInspector] public int MaxJumpCount;
    //[HideInInspector] public int AirState;
    //记录当前跳跃状态，0为未跳跃，1为上升，2为下降，3为即将落地
    [HideInInspector] public int currentAttackIndex;
    [HideInInspector] public bool IsFacingRight;
    [HideInInspector] public bool IsFalling;
    [HideInInspector] public bool IsGround;
    [HideInInspector] public bool IsDodging;
    [HideInInspector] public bool CanDodge;
    [HideInInspector] public bool DodgeInput;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool attack_first_input;
    [HideInInspector] public bool CanClimb;
    [HideInInspector] public bool isClimbing;

    #endregion

    #region ray cast variable

    [HideInInspector] public RaycastHit2D HitObject;
    // 这是被射线碰撞的结构体实例，包含有关碰撞的详细信息，如碰撞点的位置、碰撞点的表面法线、碰撞物体的引用
    [HideInInspector] public Vector2 RayOrigin;
    // 射线的起始点
    [HideInInspector] public Vector2 RayDirection;
    // 射线的方向
    public float RayDistance;
    // 射线的长度,同时也是人物开始进入jump_state 3时的离地面的距离
    public LayerMask RayLayer;
    // 使射线只检测30层里的碰撞ti

    #endregion

    #region State Machine Variable

    [HideInInspector] public StateMachine<Player> StateMachine { get; set; }
    [HideInInspector] public PlayerIdleState IdleState { get; set; }
    [HideInInspector] public PlayerRunState RunState { get; set; }
    [HideInInspector] public PlayerWalkState WalkState { get; set; }
    [HideInInspector] public PlayerJumpState JumpState { get; set; }
    [HideInInspector] public PlayerFallState FallState { get; set; }
    [HideInInspector] public PlayerDodgeState DodgeState { get; set; }
    [HideInInspector] public PlayerAttackState AttackState { get; set; }
    [HideInInspector] public PlayerClimbState ClimbState { get; set; }

    #endregion

    #region Animation Variable

    [HideInInspector] public Animator playerAnimator;
    [HideInInspector] public string currentPlayerStateName;
    [HideInInspector] public const string AnimationIdle = "idle";
    [HideInInspector] public const string AnimationRun = "run";
    [HideInInspector] public const string AnimationWalk = "walk";
    [HideInInspector] public const string AnimationRunEnd = "run_end";
    [HideInInspector] public const string AnimationFirstJump = "jump_up_1";
    [HideInInspector] public const string AnimationSecondJump = "jump_roll";
    [HideInInspector] public const string AnimationFall = "fall";
    [HideInInspector] public const string AnimationJumpEnd = "jump_end";
    [HideInInspector] public const string AnimationFirstRunJump = "run_jump_up_1";
    [HideInInspector] public const string AnimationRunJumpEnd = "run_jump_end";
    [HideInInspector] public const string AnimationDodge = "dodge";
    [HideInInspector] public const string AnimationAttack1 = "attack_1";
    [HideInInspector] public const string AnimationAttack2 = "attack_2";
    [HideInInspector] public const string AnimationAttack3 = "attack_3";

    #endregion

    #region Awake

    private void Awake()
    {
        StateMachine = new StateMachine<Player>();

        IdleState = new PlayerIdleState(this, StateMachine);
        RunState = new PlayerRunState(this, StateMachine);
        WalkState = new PlayerWalkState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
        FallState = new PlayerFallState(this, StateMachine);
        DodgeState = new PlayerDodgeState(this, StateMachine);
        AttackState = new PlayerAttackState(this, StateMachine);
        ClimbState = new PlayerClimbState(this, StateMachine);

        StateMachine.InitializeState(IdleState);
    }

    #endregion

    #region Start

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();

        DodgeCoolTimer = DodgeCoolTime;

        CanClimb = false;
        isClimbing = false;
        IsCheckingVerticalMoveInput = true;
        beginDodgeCoolTimer = false;
        IsFacingRight = true;
        IsFalling = false;
        IsGround = true;
        IsDodging = false;
        CanDodge = true;
        DodgeInput = false;
        attack_first_input = false;
        isAttacking = false;
        JumpCount = 0;
        MaxJumpCount = 2;
        currentAttackIndex = 0;

        RayDirection = Vector2.down;
    }

    #endregion

    #region Update

    private void Update()
    {
        // check Jump Input
        if (IsCheckingVerticalMoveInput && JumpCount < 2)
            if (Input.GetButtonDown("Jump"))
                VerticalMoveInput = true;

        RayOrigin = new Vector2(transform.position.x, transform.position.y);

        // check is falling
        if (playerRigidbody.velocity.y < 0f && !IsGround)
        {
            HitObject = Physics2D.Raycast(RayOrigin, RayDirection, RayDistance, RayLayer);
            if (HitObject.collider == null)
                IsFalling = true;
        }
        else
            IsFalling = false;

        // reset Jump Count
        if (IsGround == true)
        {
            JumpCount = 0;
        }

        // check dodge input
        if (Input.GetKeyDown(KeyCode.LeftShift))
            DodgeInput = true;

        // reset Dodge cool timer
        if (DodgeCoolTimer <= 0)
        {
            DodgeCoolTimer = DodgeCoolTime;
            beginDodgeCoolTimer = false;
            CanDodge = true;
        }

        // being Dodge coll timer
        if (beginDodgeCoolTimer)
            DodgeCoolTimer -= Time.deltaTime;

        // check horizontal move input
        HorizontalMoveInput = Input.GetAxisRaw("Horizontal");

        // check attack input
        CheckAttackInput();

        // check is attack end
        // if (is_attacking_end)
        // {
        //     is_attacking_end = false;
        //     StartCoroutine(EndCheckAttackInput());
        // }



        // flip the player according to the direction he is facing
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

        StateMachine.CurrentPlayerState.FrameUpdate();

    }

    #endregion

    #region FixedUpdate

    private void FixedUpdate()
    {
        StateMachine.CurrentPlayerState.PhysicsUpdate();
    }

    #endregion

    #region Animation Manager

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentPlayerState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        PlayerDamaged,
        PlayerDamage,
        EnemyDamaged,
        EnemyDamage,
    }

    public void ChangeAnimationState(string new_state, float start_time = 0f)
    //播放动画，第一个参数是要播放的动画名称对应的字符串，第二个参数是0到1的float变量，控制动画的播放的起始标准时间，默认为0
    {
        if (new_state == currentPlayerStateName)
        {
            return;
        }

        playerAnimator.Play(new_state, 0, start_time);
        currentPlayerStateName = new_state;
    }

    public bool IsAnimationPlaying(string state_name, float exit_time = 1.0f)
    //检查动画是否正在播放，第二个参数是0到1的float变量，规定了播放完成的终止标准时间，默认为1，
    {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(state_name) && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < exit_time)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region Collision

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platforms")
            IsGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platforms")
            IsGround = false;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
        }
    }

    #endregion

    #region Player Input Check

    public void BeginVerticalMoveCheck()
    {
        IsCheckingVerticalMoveInput = true;
    }

    public void EndVerticalMoveCheck()
    {
        IsCheckingVerticalMoveInput = false;
    }

    void CheckAttackInput()
    {
        if (Input.GetButtonDown("Attack") && currentAttackIndex ==0)
        {
            attack_first_input = true;
        }
    }

    #endregion
}