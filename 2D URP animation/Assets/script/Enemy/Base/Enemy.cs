using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D enemyRigidbody;
    public Transform enemyTransform;
    public SpriteRenderer enemySpriteRenderer;
    public Animator enemyAnimator;
    public UnitHealth enemyHealth;
    public LayerMask RayLayer;
    public int health;
    public float moveSpeed;
    public float patrolStopTime;
    public bool IsFacingRight;
    public float chaseSpeed;
    public float attackDistance;
    public float losePlayerTime;

    [HideInInspector] public string currentEnemyStateName;
    [HideInInspector] public string enemyPatrolAnimation;
    [HideInInspector] public string enemyChaseAnimation;
    [HideInInspector] public string enemyAttackAnimation1;
    [HideInInspector] public string enemyAttackAnimation2;
    [HideInInspector] public string enemyAttackAnimation3;

    [HideInInspector] public StateMachine<Enemy> stateMachine { get; set; }
    [HideInInspector] public EnemyPatrolState enemyPatrolState { get; set; }
    [HideInInspector] public EnemyChaseState enemyChaseState { get; set; }
    [HideInInspector] public EnemyAttackState enemyAttackState { get; set; }

    void Start()
    {
        enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        enemyTransform = gameObject.GetComponent<Transform>();
        enemySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        enemyAnimator = gameObject.GetComponent<Animator>();

        enemyHealth = new UnitHealth(health, health);

        IsFacingRight = true;

        stateMachine = new StateMachine<Enemy>();
        enemyPatrolState = new EnemyPatrolState(this, stateMachine);
        enemyChaseState = new EnemyChaseState(this, stateMachine);
        enemyAttackState = new EnemyAttackState(this, stateMachine);

        stateMachine.InitializeState(enemyPatrolState);
    }

    void Update()
    {
        stateMachine.CurrentPlayerState.FrameUpdate();

        // Flip the enemy according to the direction it is moving
        if (enemyRigidbody.velocity.x > 0.01f && !IsFacingRight)
        {
            Flip();
        }
        else if (enemyRigidbody.velocity.x < -0.01f && IsFacingRight)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        stateMachine.CurrentPlayerState.PhysicsUpdate();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        stateMachine.CurrentPlayerState.OnTriggerEnter2D(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        stateMachine.CurrentPlayerState.OnTriggerStay2D(other);
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        stateMachine.CurrentPlayerState.OnTriggerExit2D(other);
    }

    public void ChangeAnimationState(string new_state, float start_time = 0f)
    //播放动画，第一个参数是要播放的动画名称对应的字符串，第二个参数是0到1的float变量，控制动画的播放的起始标准时间，默认为0
    {
        if (new_state == currentEnemyStateName)
        {
            return;
        }

        enemyAnimator.Play(new_state, 0, start_time);
        currentEnemyStateName = new_state;
    }

    public bool IsAnimationPlaying(string state_name, float exit_time = 1.0f)
    //检查动画是否正在播放，第二个参数是0到1的float变量，规定了播放完成的终止标准时间，默认为1，
    {
        if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName(state_name) && enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < exit_time)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Flip()
    {
        IsFacingRight = !IsFacingRight;
        enemyTransform.Rotate(0, 180, 0);
    }
}
