using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : AbstractState<Player>
{
    public PlayerRunState(Player player, StateMachine<Player> characterStateMachine) : base(player, characterStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Enter Run State");

        player.BeginVerticalMoveCheck();
    }

    public override void ExitState()
    {
        base.ExitState();

        player.EndVerticalMoveCheck();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        
        // switch to Idle state
        if (player.HorizontalMoveInput < 0.1f && player.HorizontalMoveInput > -0.1f)
        {
            characterStateMachine.ChangeState(player.IdleState);

            player.ChangeAnimationState(Player.AnimationIdle);
        }    

        // switch to jump state
        if (player.VerticalMoveInput  == true)
        {
            characterStateMachine.ChangeState(player.JumpState);

            player.ChangeAnimationState(Player.AnimationFirstRunJump);
        }

        // switch to Fall state
        if (player.IsFalling == true)
        {
            player.StateMachine.ChangeState(player.FallState);

            player.ChangeAnimationState(Player.AnimationFall);
        }

         // switch to Dodge state
        if (player.DodgeInput && player.CanDodge)
        {
            characterStateMachine.ChangeState(player.DodgeState);

            player.ChangeAnimationState(Player.AnimationDodge);
        }

        // switch to Attack state
        if (player.attack_first_input || GameManager.gameManager.IsAttackInputDetected())
        {
            characterStateMachine.ChangeState(player.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (player.HorizontalMoveInput > 0.1f || player.HorizontalMoveInput < -0.1f)
            player.playerRigidbody.velocity = new Vector3(player.HorizontalMoveInput * player.RunSpeed, player.playerRigidbody.velocity.y, 0f);
        else
            player.playerRigidbody.velocity = new Vector3(0f, player.playerRigidbody.velocity.y, 0f);
    }

}
