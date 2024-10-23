using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D enemyRigidbody;
    public Transform enemyTransform;
    public SpriteRenderer enemySpriteRenderer;
    public UnitHealth enemyHealth;
    public LayerMask RayLayer;
    public int health;
    public float moveSpeed;
    public float patrolStopTime;
    public bool IsFacingRight;
    public float chaseSpeed;
    public float attackDistance;
    public float losePlayerTime;
    [HideInInspector] public StateMachine<Enemy> stateMachine { get; set; }
    [HideInInspector] public EnemyPatrolState enemyPatrolState { get; set; }
    [HideInInspector] public EnemyChaseState enemyChaseState { get; set; }
    [HideInInspector] public EnemyAttackState enemyAttackState { get; set; }

    void Start()
    {
        enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        enemyTransform = gameObject.GetComponent<Transform>();
        enemySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

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

    public void Flip()
    {
        IsFacingRight = !IsFacingRight;
        enemyTransform.Rotate(0, 180, 0);
    }
}
