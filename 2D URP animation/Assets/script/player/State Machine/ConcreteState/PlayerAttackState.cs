using System.Collections;
using UnityEngine;

public class PlayerAttackState : AbstractState<Player>
{
    public PlayerAttackState(Player character, StateMachine<Player> characterStateMachine) : base(character, characterStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Enter Attack State " + player.currentAttackIndex);
        
        player.isAttacking = false;
        StartAttack();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // if (!player.isAttacking && GameManager.gameManager.IsAttackInputDetected())
        // {
        //     StartAttack();
        // }

        // switch to other state
        if (!player.isAttacking && !GameManager.gameManager.IsAttackInputDetected())
        {
            // switch to Idle state
            if (player.HorizontalMoveInput < 0.1f && player.HorizontalMoveInput > -0.1f)
            {
                characterStateMachine.ChangeState(player.IdleState);

                player.ChangeAnimationState(Player.AnimationIdle);
            }
        }
    }

    private void StartAttack()
    {
        player.isAttacking = true;
        player.currentAttackIndex++;
        character.StartCoroutine(PerformAttack());

        Debug.Log("Attack is performed");
    }

    private IEnumerator PerformAttack()
    {
        string attackAnimation = "attack_" + player.currentAttackIndex;
        character.ChangeAnimationState(attackAnimation);

        float animationLength = character.playerAnimator.GetCurrentAnimatorStateInfo(0).length;
        float detectionWindow = animationLength + 0.5f; // Detection window for the next attack

        GameManager.gameManager.StartDetectionWindow(detectionWindow);

        yield return new WaitForSeconds(animationLength);

        player.isAttacking = false;

        if (player.currentAttackIndex >= 3)
        {
            player.currentAttackIndex = 0; // Reset combo after third attack
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        player.isAttacking = false;
    }
}
