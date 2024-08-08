using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public Rigidbody2D PlayerRigidbody;
    public Transform PlayerTransform;
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }

    #region Controller Variable
    public float RunSpeed;
    public float WalkSpeed;
    public float FirstJumpForce;
    public float SecondJumoForce;
    public float AirDrag;
    public float DodgeTime;
    public float DodgeCoolTime;
    public float DodgeForce;
    public float DodgeGravity;
    public float attack_waiting_time;
    //3连击之间的等待时间，超过该时间后再次攻击，则从第一段攻击重新开始。

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
    [HideInInspector] public int attack_count;
    [HideInInspector] public bool IsFacingRight;
    [HideInInspector] public bool IsFalling;
    [HideInInspector] public bool IsGround;
    [HideInInspector] public bool IsDodging;
    [HideInInspector] public bool CanDodge;
    [HideInInspector] public bool DodgeInput;
    [HideInInspector] public bool is_attacking;
    [HideInInspector] public bool attack_first_input;
    [HideInInspector] public bool attack_input;
    [HideInInspector] public bool is_checking_attack_input;

    #endregion

    #region ray cast variable

    [HideInInspector] public RaycastHit2D HitObject;
    // 这是被射线碰撞的结构体实例，包含有关碰撞的详细信息，如碰撞点的位置、碰撞点的表面法线、碰撞物体的引用
    [HideInInspector] public Vector2 RayOrigin;
    // 射线的起始点
    [HideInInspector] public Vector2 RayDirection;
    // 射线的方向
    [HideInInspector] public float RayDistance;
    // 射线的长度,同时也是人物开始进入jump_state 3时的离地面的距离
    public LayerMask RayLayer;
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

    #region Animation Variable

    [HideInInspector] public Animator PlayerAnimator;
    [HideInInspector] public string CurrentPlayerStateName;
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

    #region Main Unity Function

    private void Awake()
    {
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

        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        PlayerTransform = GetComponent<Transform>();

        DodgeCoolTimer = DodgeCoolTime;

        IsCheckingVerticalMoveInput = true;
        beginDodgeCoolTimer = false;
        IsFacingRight = true;
        IsFalling = false;
        IsGround = true;
        IsDodging = false;
        CanDodge = true;
        DodgeInput = false;
        attack_first_input = false;
        attack_input = false;
        is_attacking = false;
        is_checking_attack_input = false;
        JumpCount = 0;
        MaxJumpCount = 2;
        attack_count = 0;

        RayDirection = Vector2.down;
    }

    private void Update()
    {
        // check Jump Input
        if (IsCheckingVerticalMoveInput && JumpCount < 2)
            if (Input.GetButtonDown("Jump"))
                VerticalMoveInput = true;

        // check is ground
        if (PlayerRigidbody.velocity.y > 0.01f || PlayerRigidbody.velocity.y < -0.01f)
            IsGround = false;

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

        // check is falling
        if (PlayerRigidbody.velocity.y < 0.05f && !IsGround)
            IsFalling = true;
        else
            IsFalling = false;

        // check horizontal move input
        HorizontalMoveInput = Input.GetAxisRaw("Horizontal");

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

    private void FixedUpdate()
    {
        StateMachine.CurrentPlayerState.PhysicsUpdate();
    }

    #endregion

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

    #region Animation Manager

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentPlayerState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        PlayerDamaged,
    }

    public void ChangeAnimationState(string new_state, float start_time = 0f)
    //播放动画，第一个参数是要播放的动画名称对应的字符串，第二个参数是0到1的float变量，控制动画的播放的起始标准时间，默认为0
    {
        if (new_state == CurrentPlayerStateName)
        {
            return;
        }

        PlayerAnimator.Play(new_state, 0, start_time);
        CurrentPlayerStateName = new_state;
    }

    public bool IsAnimationPlaying(string state_name, float exit_time = 1.0f)
    //检查动画是否正在播放，第二个参数是0到1的float变量，规定了播放完成的终止标准时间，默认为1，
    {
        if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName(state_name) && PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < exit_time)
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
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (attack_count == 0)
            {
                attack_first_input = true;
            }
            else
            {
                if (is_checking_attack_input)
                {
                    attack_input = true;
                    //Debug.Log(attack_input);
                }
            }
        }
    }

    public void BeginCheckAttackInput()
    //call this method at the first frame of attack animation
    {
        is_checking_attack_input = true;
    }

    public IEnumerator EndCheckAttackInput()
    //call this method at the last frame of attack animation
    {
        if (!attack_input)
        {
            yield return StartCoroutine(WaitOrInterruptAttack(attack_waiting_time));
        }
        //Debug.Log("end");
        is_checking_attack_input = false;
    }

    public IEnumerator WaitOrInterruptAttack(float waitTime)
    //attack wait timer
    {
        float elapsed_time = 0.0f;
        while (elapsed_time < waitTime && !attack_input && attack_count != 3)
        {
            yield return null; 
            //暂停协程，到下一帧再运行
            elapsed_time += Time.deltaTime;
            //Debug.Log("inside attack input " + attack_input);
        }
    }

    #endregion
}