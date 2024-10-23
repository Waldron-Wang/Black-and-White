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
    public int moveSpeed;
    public float patrolStopTime;
    bool IsFacingRight;
    [HideInInspector] public StateMachine<Enemy> stateMachine { get; set; }

    void Start()
    {
        enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        enemyTransform = gameObject.GetComponent<Transform>();
        enemySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        enemyHealth = new UnitHealth (health, health);

        IsFacingRight = true;
    }

    void Update()
    {
        // flip the enemy according to the direction he is facing
        if (enemyRigidbody.velocity.x > 0.01f && IsFacingRight == false)
        {
            IsFacingRight = !IsFacingRight;
            enemySpriteRenderer.flipX = false;
        }
        else if (enemyRigidbody.velocity.x < -0.01f && IsFacingRight == true)
        {
            IsFacingRight = !IsFacingRight;
            enemySpriteRenderer.flipX = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
