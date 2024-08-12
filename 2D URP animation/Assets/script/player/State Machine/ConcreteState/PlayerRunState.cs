using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    public PlayerRunState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
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
            playerStateMachine.ChangeState(player.IdleState);

            player.ChangeAnimationState(Player.AnimationIdle);
        }    

        // switch to jump state
        if (player.VerticalMoveInput  == true)
        {
            playerStateMachine.ChangeState(player.JumpState);

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
            playerStateMachine.ChangeState(player.DodgeState);

            player.ChangeAnimationState(Player.AnimationDodge);
        }

        // switch to Attack state
        if (player.attack_first_input || player.attack_input)
        {
            playerStateMachine.ChangeState(player.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (player.HorizontalMoveInput > 0.1f || player.HorizontalMoveInput < -0.1f)
            player.PlayerRigidbody.velocity = new Vector3(player.HorizontalMoveInput * player.RunSpeed, player.PlayerRigidbody.velocity.y, 0f);
        else
            player.PlayerRigidbody.velocity = new Vector3(0f, player.PlayerRigidbody.velocity.y, 0f);
    }

}
