using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : AbstractState<Enemy>
{
    private int currentAttackIndex;
    private bool isAttacking;
    private bool isPlayerInRange;

    public EnemyAttackState(Enemy character, StateMachine<Enemy> characterStateMachine) : base(character, characterStateMachine)
    {
        currentAttackIndex = 0;
        isAttacking = false;
        isPlayerInRange = false;
    }

    public override void EnterState()
    {
        base.EnterState();
        currentAttackIndex = 0;
        isAttacking = false;
        isPlayerInRange = false;
        StartAttack();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        float distanceToPlayer = Vector2.Distance(character.enemyTransform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        isPlayerInRange = distanceToPlayer <= character.attackDistance;

        if (!isAttacking)
        {
            if (currentAttackIndex < 2 && isPlayerInRange)
            {
                StartAttack();
            }
            else if (currentAttackIndex >= 2)
            {
                character.stateMachine.ChangeState(character.enemyPatrolState);
            }
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        currentAttackIndex++;
        character.StartCoroutine(PerformAttack());
    }

    private IEnumerator PerformAttack()
    {
        string attackAnimation = "Attack" + currentAttackIndex;
        character.enemyAnimator.Play(attackAnimation);

        yield return new WaitForSeconds(character.enemyAnimator.GetCurrentAnimatorStateInfo(0).length);

        if (currentAttackIndex < 3 && isPlayerInRange)
        {
            yield return new WaitForSeconds(0.5f); // Detection window for the next attack
        }

        isAttacking = false;

        if (currentAttackIndex >= 3)
        {
            currentAttackIndex = 0;
            character.stateMachine.ChangeState(character.enemyPatrolState);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        isAttacking = false;
        currentAttackIndex = 0;
    }
}
