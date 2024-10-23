using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : AbstractState<Enemy>
{
    public RaycastHit2D groundHit;
    public RaycastHit2D wallHit;
    Vector2 groundRayOrigin;
    Vector2 wallRayOrigin;
    Vector2 groundRayDirection;
    Vector2 wallRayDirection;
    float groundRayDistance;
    float wallRayDistance;
    bool shouldStopMove;
    bool isWaiting;

    public EnemyPatrolState(Enemy enemy, StateMachine<Enemy> characterStateMachine) : base(enemy, characterStateMachine)
    {
        groundRayDistance = 0.6f;
        wallRayDistance = 0.5f;
        groundRayDirection = Vector2.down;
        wallRayDirection = Vector2.right;

        shouldStopMove = false;
        isWaiting = false;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        groundRayOrigin = new Vector2(
            character.enemyTransform.position.x + (character.IsFacingRight ? 0.5f : -0.5f),
            character.enemyTransform.position.y
        );
        wallRayOrigin = new Vector2(
            character.enemyTransform.position.x,
            character.enemyTransform.position.y
        );

        groundHit = Physics2D.Raycast(groundRayOrigin, groundRayDirection, groundRayDistance, character.RayLayer);
        wallHit = Physics2D.Raycast(wallRayOrigin, wallRayDirection * (character.IsFacingRight ? 1 : -1), wallRayDistance, character.RayLayer);

        Debug.DrawLine(groundRayOrigin, groundRayOrigin + groundRayDirection * groundRayDistance, Color.red);
        Debug.DrawLine(wallRayOrigin, wallRayOrigin + wallRayDirection * wallRayDistance, Color.blue);

        if (groundHit.collider == null || wallHit.collider != null)
        {
            shouldStopMove = true;
        }
        else
        {
            shouldStopMove = false;
        }

        if (shouldStopMove && !isWaiting)
        {
            character.StartCoroutine(WaitAndFlip());
        }
        else if (!shouldStopMove && !isWaiting)
        {
            character.enemyRigidbody.velocity = new Vector2(character.moveSpeed * (character.IsFacingRight ? 1 : -1), character.enemyRigidbody.velocity.y);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private IEnumerator WaitAndFlip()
    {
        isWaiting = true;
        character.enemyRigidbody.velocity = Vector2.zero;
        yield return new WaitForSeconds(character.patrolStopTime);
        character.Flip();
        isWaiting = false;

        // Ensure the enemy starts moving again after flipping
        character.enemyRigidbody.velocity = new Vector2(character.moveSpeed * (character.IsFacingRight ? 1 : -1), character.enemyRigidbody.velocity.y);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            character.stateMachine.ChangeState(character.enemyChaseState);
        }
    }
}