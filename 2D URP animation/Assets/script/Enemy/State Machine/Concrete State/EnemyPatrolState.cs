using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : AbstractState<Enemy>
{
    RaycastHit2D HitObject1;
    RaycastHit2D HitObject2;
    Vector2 RayOrigin1;
    Vector2 RayOrigin2;
    Vector2 RayDirection1;
    Vector2 RayDirection2;
    float RayDistance1;
    float RayDistance2;
    bool ShouldStopMove;
    
    public EnemyPatrolState(Enemy enemy, StateMachine<Enemy> characterStateMachine) : base(enemy, characterStateMachine)
    {
        RayDistance1 = 0.6f;
        RayDistance2 = 0.5f;
        RayDirection1 = Vector2.down;
        RayDirection2 = Vector2.right;

        ShouldStopMove = false;
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

        RayOrigin1 = new Vector2(enemy.enemyTransform.position.x, enemy.enemyTransform.position.y);
        RayOrigin1 = new Vector2(enemy.enemyTransform.position.x, enemy.enemyTransform.position.y);

        HitObject1 = Physics2D.Raycast(RayOrigin1, RayDirection1, RayDistance1, enemy.RayLayer);
        HitObject2 = Physics2D.Raycast(RayOrigin2, RayDirection2, RayDistance2, enemy.RayLayer);

        if (HitObject1.collider == null || HitObject2.collider != null)
        {
            ShouldStopMove = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
