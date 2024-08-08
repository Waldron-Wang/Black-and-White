using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostWarriorController : MonoBehaviour
{
    Rigidbody2D rb_ghost;
    Animator animator_ghost;

    string current_state;
    const string animation_idle = "idle";
    const string animation_move = "move";
    const string animation_attack = "attack";
    const string animation_get_hit = "get_hit";
    const string animation_death = "death";

    [SerializeField] float move_speed;
    [SerializeField] float attack_range;
    [SerializeField] float detect_range;
    [SerializeField] LayerMask player_layer;

    bool is_facing_right = true;
    bool is_patrolling;
    bool is_chasing;
    bool is_attacking;
    bool is_hurt;
    bool is_dead;
    Transform player;

    void Start()
    {
        rb_ghost = GetComponent<Rigidbody2D>();
        animator_ghost = GetComponent<Animator>();

        is_patrolling = true;
        is_chasing = false;
        is_attacking = false;
        is_hurt = false;
        is_dead = false;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (is_dead) return;

        if (is_hurt)
        {
            ChangeAnimationState(animation_get_hit);
            return;
        }

        if (is_attacking)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attack_range)
        {
            is_patrolling = false;
            is_chasing = false;
            is_attacking = true;
            Attack();
        }
        else if (distanceToPlayer <= detect_range)
        {
            is_patrolling = false;
            is_chasing = true;
        }
        else
        {
            is_patrolling = true;
            is_chasing = false;
        }

        if (is_patrolling)
        {
            Patrol();
        }
        else if (is_chasing)
        {
            ChasePlayer();
        }
    }

    void Patrol()
    {
        ChangeAnimationState(animation_move);
        rb_ghost.velocity = new Vector2(move_speed, rb_ghost.velocity.y);
    }

    void ChasePlayer()
    {
        ChangeAnimationState(animation_move);
        Vector2 direction = (player.position - transform.position).normalized;
        rb_ghost.velocity = new Vector2(direction.x * move_speed, rb_ghost.velocity.y);

        if (direction.x > 0 && !is_facing_right)
        {
            Flip();
        }
        else if (direction.x < 0 && is_facing_right)
        {
            Flip();
        }
    }

    void Attack()
    {
        ChangeAnimationState(animation_attack);
    }

    public void TakeDamage()
    {
        if (is_dead) return;

        is_hurt = true;
        StartCoroutine(ResetHurt());
    }

    IEnumerator ResetHurt()
    {
        yield return new WaitForSeconds(1f);
        is_hurt = false;
    }

    public void Die()
    {
        if (is_dead) return;

        is_dead = true;
        ChangeAnimationState(animation_death);
        rb_ghost.velocity = Vector2.zero;
    }

    void ChangeAnimationState(string new_state)
    {
        if (new_state == current_state) return;

        animator_ghost.Play(new_state);
        current_state = new_state;
    }

    void Flip()
    {
        is_facing_right = !is_facing_right;
        transform.Rotate(0, 180, 0);
    }
}
