using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyChaseState : AbstractState<Enemy>
{
    private Transform playerTransform;
    private float losePlayerTimer;

    public EnemyChaseState(Enemy character, StateMachine<Enemy> characterStateMachine) : base(character, characterStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        losePlayerTimer = 0.0f;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (playerTransform == null)
        {
            characterStateMachine.ChangeState(character.enemyPatrolState);
            return;
        }

        float distanceToPlayer = Vector2.Distance(character.enemyTransform.position, playerTransform.position);

        if (distanceToPlayer <= character.attackDistance)
        {
            // Switch to attack state (not implemented here)
            // characterStateMachine.ChangeState(character.enemyAttackState);
            return;
        }

        if (losePlayerTimer >= character.losePlayerTime)
        {
            characterStateMachine.ChangeState(character.enemyPatrolState);
            return;
        }

        Vector2 direction = (playerTransform.position - character.enemyTransform.position).normalized;
        direction.y = 0; // Ensure the enemy only moves horizontally
        character.enemyRigidbody.velocity = new Vector2(direction.x * character.chaseSpeed, 0);

        losePlayerTimer += Time.deltaTime;
    }

    public override void ExitState()
    {
        base.ExitState();
        character.enemyRigidbody.velocity = Vector2.zero;
    }

    public void ResetLosePlayerTimer()
    {
        losePlayerTimer = 0.0f;
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ResetLosePlayerTimer();
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ResetLosePlayerTimer();
        }
    }
}
