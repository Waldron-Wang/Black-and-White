using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }
    
    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Enter Idle State");

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

        // switch to run state
        if (player.HorizontalMoveInput > 0.1f || player.HorizontalMoveInput < -0.1f)
        {
            playerStateMachine.ChangeState(player.RunState);

            player.ChangeAnimationState(Player.AnimationRun);
        }

        // switch to jump state
        if (player.VerticalMoveInput  == true)
        {
            playerStateMachine.ChangeState(player.JumpState);

            player.ChangeAnimationState(Player.AnimationFirstJump);
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
        if(player.isClimbing)
        {
            playerStateMachine.ChangeState(player.ClimbState);
        }
    }
}

