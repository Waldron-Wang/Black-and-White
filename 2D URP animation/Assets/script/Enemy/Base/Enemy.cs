using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D enemyRigidbody;
    public Transform enemyTransform;
    public UnitHealth enemyHealth;
    public int health;
    public int moveSpeed;
    [HideInInspector] public StateMachine<Enemy> stateMachine { get; set; }
    void Start()
    {
        enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        enemyTransform = gameObject.GetComponent<Transform>();

        enemyHealth = new UnitHealth (health, health);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
