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

        // If the player is not attacking and no attack input is detected, switch to Idle state
        if (!player.isAttacking && !GameManager.gameManager.IsAttackInputDetected())
        {
            characterStateMachine.ChangeState(player.IdleState);
            player.ChangeAnimationState(Player.AnimationIdle);
        }
    }

    private void StartAttack()
    {
        player.isAttacking = true;
        character.StartCoroutine(PerformAttack());
        Debug.Log("Attack " + player.currentAttackIndex + " Start"); 
    }

    private IEnumerator PerformAttack()
    {
        string attackAnimation = "attack_" + player.currentAttackIndex;
        character.ChangeAnimationState(attackAnimation);

        float animationLength = character.playerAnimator.GetCurrentAnimatorStateInfo(0).length;
        float detectionWindow = animationLength + 0.5f; // Detection window for the next attack

        GameManager.gameManager.StartDetectionWindow(detectionWindow);

        // Wait until the detection window is closed
        yield return new WaitUntil(() => !GameManager.gameManager.IsDetectionWindowActive());

        player.isAttacking = false;

        if (GameManager.gameManager.IsAttackInputDetected())
        {
            player.currentAttackIndex++;
            if (player.currentAttackIndex >= 3)
            {
                player.currentAttackIndex = 0; // Reset combo after third attack
            }
            Debug.Log("Attack input detected. Switching to Attack State with index " + player.currentAttackIndex);
            characterStateMachine.ChangeState(player.AttackState);
        }
        else
        {
            Debug.Log("No attack input detected. Resetting attack index and switching to Idle State.");
            player.currentAttackIndex = 0;
            characterStateMachine.ChangeState(player.IdleState);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        player.isAttacking = false;
        // Removed resetting currentAttackIndex here to maintain combo sequence
    }
}
